using System;
using Midir;
using UnityEngine;
using UnityEngine.AI;

namespace Midir
{
    public class AI : EnemyAnimatorManager
    {
        [SerializeField]
        private float DistanceToTrigger = 80;

        [SerializeField]
        private float DistanceToEnterInFight = 20;

        private float DistanceToExitPersue;
        private float DistanceToExitFight;
        
        private NavMeshAgent _agent;
        private Rigidbody rb;

        [SerializeField]
        private Transform player;

        private float moveAsideSpeed = 0.4f;

        private bool alreadyAttacked;

        public EnemyAction[] enemyAttacks;
        private EnemyAction currentAttack;

        private float timer;
        private int randomTimer;
        private bool canAttack = true;

        private EnemyStats enemyStats;

        private PlayerStats playerStats;

        private int nbAttackUsed = 2;

        public bool isSleeping;

        private float speed, acceleration;

        [HideInInspector]
        public bool ambush;

        private int[] arrayIndex = new int[2];

        public enum Behaviour
        {
            Idle,
            Persue,
            Combat,
            Dead,
        }

        public Behaviour behaviour = Behaviour.Idle;

        private void Awake()
        {
            DistanceToExitPersue = DistanceToTrigger;
            DistanceToExitFight = DistanceToEnterInFight;

            anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<PlayerLocomotion>().transform;
            rb = GetComponent<Rigidbody>();

            enemyStats = GetComponent<EnemyStats>();
            playerStats = FindObjectOfType<PlayerStats>();

            if (isSleeping)
                PlayTargetAnimation("Sleep", true);

            speed = _agent.speed;
            acceleration = _agent.acceleration;

            arrayIndex[0] = nbAttackUsed + 1;
            arrayIndex[1] = nbAttackUsed + 1;

            _agent.stoppingDistance = DistanceToEnterInFight;
            ambush = isSleeping;
        }

        #region Distances To trigger next Behaviour

        #region Enter Distances

        private bool DetectEnemy()
        {
            return DistanceToPlayer() < DistanceToTrigger;
        }
        
        private bool InRangeToEnterInCombat()
        {
            return DistanceToPlayer() < DistanceToEnterInFight && !ambush;
        }
        
        #endregion
        
        #region Exit Distances
        
        private bool InRangeToExitPersue()
        {
            return DistanceToPlayer() > DistanceToExitPersue;
        }
        
        private bool InRangeToExitCombat()
        {
            return DistanceToPlayer() > DistanceToExitFight;
        }

        #endregion
        
        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, PlayerManager.Singleton.transform.position);
        }
        
        #endregion
        
        private void Update()
        {
            if (enemyStats.isDead)
            {
                behaviour = Behaviour.Dead;
                Invoke(nameof(DestroyEnemy), 3f);
            }

            if (playerStats.isDead)
                return;

            if (enemyStats.currentHealth <= enemyStats.maxHealth / 2 && enemyStats.isBoss)
                SecondPhase();

            switch (behaviour)
            {
                case Behaviour.Idle:
                    Idle();
                    break;

                case Behaviour.Persue:
                    Persue();
                    break;

                case Behaviour.Combat:
                    Combat();
                    break;
            }

            enemyStats.CancelDelai();

            Ambush();
        }

        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        private void SetState(Behaviour state)
        {
            behaviour = state;
        }

        private void ResetAttackTimer()
        {
            canAttack = true;
        }
        
        private void Idle()
        {
            if (DetectEnemy())
            {
                SetState(Behaviour.Persue);

                if(enemyStats.isBoss)
                    enemyStats.bossHealthBar.gameObject.SetActive(true);

                _agent.ResetPath();
            }

            anim.SetFloat("Vertical", 0, 1f, Time.deltaTime);
            _agent.SetDestination(transform.position);
        }

        private void Persue()
        {
            if (InRangeToEnterInCombat())
            {
                moveAsideSpeed = -moveAsideSpeed;
                SetState(Behaviour.Combat);
            }

            if (InRangeToExitPersue())
            {
                SetState(Behaviour.Idle);
            }

            if (_agent.pathPending == false && _agent.path.corners.Length == 2 && !ambush)
                transform.LookAt(player);

            anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

            anim.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);

            _agent.SetDestination(player.position);
        }

        private void Combat()
        {
            if (InRangeToExitCombat())
            {
                SetState(Behaviour.Persue);
            }

            anim.SetFloat("Vertical", 0, 0.5f, Time.deltaTime);

            //anim.SetFloat("Horizontal", moveAsideSpeed, 0.1f, Time.deltaTime);

            Vector3 direction = player.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 3f * Time.deltaTime);

            if (canAttack)
            {
                canAttack = false;

                randomTimer = UnityEngine.Random.Range(0, 3);

                Invoke(nameof(ResetAttackTimer), randomTimer);
            }

            timer += Time.deltaTime;

            if (timer >= randomTimer)
            {
                timer = 0;

                if (!alreadyAttacked)
                {
                    alreadyAttacked = true;
                    Attack();    
                }
            }
        }

        private void SecondPhase()
        {
            nbAttackUsed = 4;
            enemyStats.canCancel = false;
        }

        private void Attack()
        {
            int i = UnityEngine.Random.Range(0, nbAttackUsed);

            #region Avoid Attack Repetition
            if (arrayIndex.Length == 0)
                arrayIndex[0] = i;
            else if (arrayIndex.Length == 1)
                arrayIndex[1] = i;
            else
            {
                if (arrayIndex[0] == arrayIndex[1] && arrayIndex[0] == i)
                {
                    while (i == arrayIndex[0])
                        i = UnityEngine.Random.Range(0, nbAttackUsed);
                }

                arrayIndex[0] = arrayIndex[1];
                arrayIndex[1] = i;
            }
            #endregion


            EnemyAction enemyAttackAction = enemyAttacks[i];

            _agent.velocity = Vector3.zero;

            currentAttack = enemyAttackAction;
            anim.Play(currentAttack.actionAnimation);
        }

        private void Ambush()
        {   
            if (isSleeping && DetectEnemy() == true)
            {
                isSleeping = false;
                PlayTargetAnimation("Wake", true);
            } 
        }

        public void ApplyRoot()
        {
            anim.applyRootMotion = true;

            _agent.speed = 0;
            _agent.acceleration = 0;
        }

        public void RemoveRoot()
        {
            ambush = false;

            alreadyAttacked = false;

            anim.applyRootMotion = false;

            _agent.speed = speed;
            _agent.acceleration = acceleration;
        }
    }
}
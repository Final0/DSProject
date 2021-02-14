using System;
using Midir;
using UnityEngine;
using UnityEngine.AI;

namespace AI.CustomAI
{
    public class AI : MonoBehaviour
    {
        public float DistanceToExitPersue = 60;
        public float DistanceToTrigger = 80;
        public float DistanceToEnterInFight = 20;
        public float DistanceToExitFight = 40;
        
        private Animator _animator;
        private NavMeshAgent _agent;
        private Rigidbody rb;

        [SerializeField]
        private Transform player;

        [SerializeField]
        private float timeBetweenAttacks;

        private float moveAsideSpeed = 0.4f;

        private bool alreadyAttacked;

        public EnemyAction[] enemyAttacks;
        private EnemyAction currentAttack;

        private float timer;
        private int randomTimer;
        private bool canAttack = true;

        private EnemyStats enemyStats;

        private PlayerStats playerStats;

        private enum Behaviour
        {
            Idle,
            Persue,
            Combat,
            Dead,
        }

        private Behaviour behaviour = Behaviour.Idle;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<PlayerLocomotion>().transform;
            rb = GetComponent<Rigidbody>();

            enemyStats = GetComponent<EnemyStats>();
            playerStats = FindObjectOfType<PlayerStats>();
        }

        #region Distances To trigger next Behaviour

        #region Enter Distances

        private bool DetectEnemy()
        {
            return DistanceToPlayer() < DistanceToTrigger;
        }
        
        private bool InRangeToEnterInCombat()
        {
            return DistanceToPlayer() < DistanceToEnterInFight;
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

            if (enemyStats.currentHealth <= enemyStats.maxHealth / 2)
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
            }
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

            transform.LookAt(player);
            rb.velocity /= 2f;

            _animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            _agent.SetDestination(player.position);

            _animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
        }

        private void Combat()
        {
            if (InRangeToExitCombat())
            {
                SetState(Behaviour.Persue);
            }

            _animator.SetFloat("Vertical", 0, 1f, Time.deltaTime);
            rb.velocity = Vector3.zero;

            _animator.SetFloat("Horizontal", moveAsideSpeed, 0.1f, Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) <= DistanceToEnterInFight)
            {
                _agent.destination = player.position;
                _agent.stoppingDistance = DistanceToEnterInFight;
            }

            Vector3 direction = player.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 3f * Time.deltaTime);

            if (canAttack)
            {
                canAttack = false;

                randomTimer = UnityEngine.Random.Range(1, 5);

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

        private int nbAttackUsed = 2;

        private void SecondPhase()
        {
            nbAttackUsed = 4;
        }

        private void Attack()
        {
            int i = UnityEngine.Random.Range(0, nbAttackUsed);

            EnemyAction enemyAttackAction = enemyAttacks[i];

            currentAttack = enemyAttackAction;
            _animator.Play(currentAttack.actionAnimation);

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }
    }
}
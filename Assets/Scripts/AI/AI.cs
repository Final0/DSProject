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

        public EnemyAction[] enemyAttacksPhase1;
        public EnemyAction[] enemyAttacksPhase2;

        private EnemyAction currentAttack;

        private float timer;
        private int randomTimer;
        private bool canAttack = true;

        private EnemyStats enemyStats;

        private PlayerStats playerStats;

        private int nbAttackUsed;

        public bool isSleeping;

        private float speed, acceleration;

        [HideInInspector]
        public bool ambush;

        private int[] arrayIndex = new int[2];

        private GameObject enemyCollider;

        private bool disableNavMesh = false;

        private BoxCollider legCollider;

        private int chanceCombo = 3;

        private float waitBeforePersue = 0f;

        private bool stopLookAt = false;

        private AudioManager audioManager;

        [SerializeField]
        private GameObject SlashFX;

        public enum Behaviour
        {
            Idle,
            Persue,
            Combat,
            Dead,
        }

        private Behaviour behaviour = Behaviour.Idle;

        private void Awake()
        {
            DistanceToExitPersue = DistanceToTrigger * 1.5f;
            DistanceToExitFight = DistanceToEnterInFight;

            anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<PlayerLocomotion>().transform;
            rb = GetComponent<Rigidbody>();
            enemyCollider = GameObject.FindGameObjectWithTag("Character Collision Blocker");

            enemyStats = GetComponent<EnemyStats>();
            playerStats = FindObjectOfType<PlayerStats>();

            if (enemyStats.isBoss)
            {
                legCollider = GameObject.FindGameObjectWithTag("Kick").GetComponent<BoxCollider>();
                legCollider.enabled = false;
            }
                

            if (isSleeping)
                PlayTargetAnimation("Sleep", true);

            speed = _agent.speed;
            acceleration = _agent.acceleration;

            arrayIndex[0] = nbAttackUsed + 1;
            arrayIndex[1] = nbAttackUsed + 1;

            _agent.stoppingDistance = DistanceToEnterInFight;
            ambush = isSleeping;

            nbAttackUsed = enemyAttacksPhase1.Length;

            audioManager = FindObjectOfType<AudioManager>();
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

            if (playerStats.isDead && !disableNavMesh)
            {
                _agent.SetDestination(transform.position);
                anim.SetFloat("Vertical", 0, 1f, Time.deltaTime);
                anim.SetFloat("Horizontal", 0, 1f, Time.deltaTime);
                return;
            }

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

        private void MoveAside()
        {
            if (rb.isKinematic)
            {
                transform.position += Vector3.right * Time.deltaTime * moveAsideSpeed * 1.5f;

                anim.SetFloat("Horizontal", moveAsideSpeed * 2, 0.1f, Time.deltaTime);
            }
        }

        private void Idle()
        {
            if (DetectEnemy())
            {             
                if (enemyStats.isBoss)
                {
                    enemyStats.bossHealthBar.gameObject.SetActive(true);
                    audioManager.BossMusic();
                }

                if(!disableNavMesh)
                    _agent.ResetPath();

                SetState(Behaviour.Persue);
            }

            anim.SetFloat("Vertical", 0, 1f, Time.deltaTime);

            if(!disableNavMesh)
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
            
            if (_agent.pathPending == false && _agent.path.corners.Length == 2 && !ambush && !stopLookAt)
                transform.LookAt(player);

            anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

            anim.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);

            if (!disableNavMesh)
                _agent.SetDestination(player.position);
        }

        private void Combat()
        {
            if (InRangeToExitCombat())
            {
                int i = UnityEngine.Random.Range(5, 10);
                waitBeforePersue += Time.deltaTime;

                anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                anim.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);

                MoveAside();

                if (rb.isKinematic && !stopLookAt)
                    transform.LookAt(player);

                if (waitBeforePersue >= i)
                {
                    if (enemyStats.isBoss && UnityEngine.Random.Range(0, 10) <= 3)
                        DistantAttack();

                    waitBeforePersue = 0f;

                    SetState(Behaviour.Persue);
                }
            }
            else
            {
                anim.SetFloat("Vertical", 0, 0.5f, Time.deltaTime);

                MoveAside();

                if (rb.isKinematic && !stopLookAt)
                    transform.LookAt(player);

                if (canAttack)
                {
                    canAttack = false;

                    randomTimer = UnityEngine.Random.Range(1, 3);

                    Invoke(nameof(ResetAttackTimer), randomTimer);
                }

                timer += Time.deltaTime;

                if (timer >= randomTimer)
                {
                    timer = 0;

                    if (!alreadyAttacked)
                    {
                        Attack();
                    }
                }
            }
        }

        private void SecondPhase()
        {
            nbAttackUsed = enemyAttacksPhase1.Length + enemyAttacksPhase2.Length;
            enemyStats.canCancel = false;

            chanceCombo += 2;
        }

        private void Attack()
        {
            int i = UnityEngine.Random.Range(0, nbAttackUsed);

            #region Avoid Attack Repetition
            if ((arrayIndex[0] == arrayIndex[1] && arrayIndex[0] == i) && nbAttackUsed != 1)
            {
                while (i == arrayIndex[0])
                    i = UnityEngine.Random.Range(0, nbAttackUsed);
            }

            arrayIndex[0] = arrayIndex[1];
            arrayIndex[1] = i;
            #endregion

            _agent.velocity = Vector3.zero;

            if (i >= 0 && i <= enemyAttacksPhase1.Length - 1)
            {
                EnemyAction enemyAttackAction = enemyAttacksPhase1[i];

                currentAttack = enemyAttackAction;
                anim.Play(currentAttack.actionAnimation);
            }
            else if (i >= enemyAttacksPhase1.Length && i <= (enemyAttacksPhase1.Length + enemyAttacksPhase2.Length) - 1)
            {
                i -= enemyAttacksPhase1.Length;
                EnemyAction enemyAttackAction = enemyAttacksPhase2[i];

                currentAttack = enemyAttackAction;
                anim.Play(currentAttack.actionAnimation);
            }
        }

        private void DistantAttack()
        {
            int i = UnityEngine.Random.Range(0, enemyAttacksPhase2.Length);

            EnemyAction enemyAttackAction = enemyAttacksPhase2[i];

            currentAttack = enemyAttackAction;
            anim.Play(currentAttack.actionAnimation);
        }

        public void AttackCombo()
        {
            int i = UnityEngine.Random.Range(0, 10);

            if (currentAttack.canCombo && i <= chanceCombo)
            {
                currentAttack = currentAttack.comboNextAttack;
                anim.Play(currentAttack.actionAnimation);
            }
        }

        private void Ambush()
        {   
            if (isSleeping && DetectEnemy() == true)
            {
                stopLookAt = true;
                PlayTargetAnimation("Wake", true);
                isSleeping = false; 
            } 
        }
        
        public void StopRotation()
        {
            stopLookAt = true;
        }

        public void ApplyRoot()
        {
            alreadyAttacked = true;

            rb.isKinematic = false;

            anim.applyRootMotion = true;

            _agent.speed = 0;
            _agent.acceleration = 0;
        }

        public void RemoveRoot()
        {
            rb.isKinematic = true;

            ambush = false;

            alreadyAttacked = false;

            anim.applyRootMotion = false;

            _agent.speed = speed;
            _agent.acceleration = acceleration;

            stopLookAt = false;
        }

        public void MovingAttack()
        {
            disableNavMesh = true;

            _agent.enabled = false;

            enemyCollider.SetActive(false);
        }

        public void StopMovingAttack()
        {
            disableNavMesh = false;

            _agent.enabled = true;

            enemyCollider.SetActive(true);
        }

        public void Kick()
        {
            legCollider.enabled = true;
        }

        public void StopKick()
        {
            legCollider.enabled = false;
        }

        public void SlashAttack()
        {
            GameObject Slash = Instantiate(SlashFX, transform.position + transform.forward * 1.5f, 
                Quaternion.LookRotation(-player.transform.position + transform.position));

            Slash.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
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

        private bool alreadyAttacked;

        public EnemyAction[] enemyAttacks;
        private EnemyAction currentAttack;

        public enum Behaviour
        {
            Idle,
            Persue,
            Combat,
        }

        private Behaviour behaviour = Behaviour.Idle;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<PlayerLocomotion>().transform;
            rb = GetComponent<Rigidbody>();
            
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

        private void SetState(Behaviour state)
        {
            behaviour = state;
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
                SetState(Behaviour.Combat);
            }

            if (InRangeToExitPersue())
            {
                SetState(Behaviour.Idle);
            }

            // TODO 
            transform.LookAt(player);

            _animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            _agent.SetDestination(player.position);
        }

        private void Combat()
        {
            if (InRangeToExitCombat())
            {
                SetState(Behaviour.Persue);
            }

            // TODO
            _animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            rb.velocity = Vector3.zero;

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                Attack();
            }
        }

        private void Attack()
        {
            int i = UnityEngine.Random.Range(0, enemyAttacks.Length);

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
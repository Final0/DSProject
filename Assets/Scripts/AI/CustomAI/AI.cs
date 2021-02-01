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
        }


        #region Distances To trigger next Behaviour

        #region Enter Distances
        
        bool DetectEnemy()
        {
            return DistanceToPlayer() < DistanceToTrigger;
        }
        
        bool InRangeToEnterInCombat()
        {
            return DistanceToPlayer() < DistanceToEnterInFight;
        }
        
        #endregion
        
        #region Exit Distances
        
        bool InRangeToExitPersue()
        {
            return DistanceToPlayer() > DistanceToExitPersue;
        }
        
        bool InRangeToExitCombat()
        {
            return DistanceToPlayer() > DistanceToExitFight;
        }

        #endregion
        
        
        

        float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, PlayerManager.Singleton.transform.position);
        }
        
        #endregion
        

        void Update()
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

        void SetState(Behaviour state)
        {
            behaviour = state;
        }
        

        void Idle()
        {
            if (DetectEnemy())
            {
                SetState(Behaviour.Persue);
            }
        }

        void Persue()
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
            
        }

        void Combat()
        {
            if (InRangeToExitCombat())
            {
                SetState(Behaviour.Persue);
            }
            
            // TODO
        }
    }
}
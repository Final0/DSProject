using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PersueTargetState persueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }
            else if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return persueTargetState;
            }
            else
            {
                return this;
            } 
        }
    }
}
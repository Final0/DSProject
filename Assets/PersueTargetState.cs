using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class PersueTargetState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }
}
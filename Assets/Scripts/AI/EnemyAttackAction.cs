using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]

    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2, maximumAttackAngle = 35, minimumAttackAngle = -35, minimumDistanceNeededToAttack = 0, maximumDistanceNeededToAttack = 3;    
    }
}
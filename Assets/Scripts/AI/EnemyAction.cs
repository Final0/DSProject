using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]

    public class EnemyAction : ScriptableObject
    {
        public bool canCombo;

        public EnemyAction comboNextAttack;

        public string actionAnimation;
    }
}
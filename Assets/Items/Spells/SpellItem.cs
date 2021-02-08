using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class SpellItem : Item
    {
        public GameObject spellCastFX;

        public string spellAnimation;

        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Type")]
        public bool isFaithSpeel;
        public bool isMagicSpeel;
        public bool isPyroSpeel;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        public virtual void AttemptToCastSpell(AnimatorHandler animatorHandler , PlayerStats playerStats)
        {

        }

        public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            playerStats.DeductFocusPoints(focusPointCost);
        }
    }
}
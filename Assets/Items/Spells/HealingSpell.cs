using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]

    public class HealingSpell : SpellItem
    {
        public int healAmount;
        GameObject instantiatedSpellFX;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
        }

        public override void DestroySpell()
        {
            Destroy(instantiatedSpellFX);
        }
    }
}
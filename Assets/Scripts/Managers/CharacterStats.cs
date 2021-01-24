using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10, maxHealth, currentHealth, staminaLevel = 10; 
        public float currentStamina, maxStamina;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public bool isDead;
    }
}
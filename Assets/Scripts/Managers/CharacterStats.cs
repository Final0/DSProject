using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10, maxHealth, currentHealth; 

        public float staminaLevel = 10, maxStamina, currentStamina;

        public int focusLevel = 10;

        public float maxFocusPoints, currentFocusPoints;

        public bool isDead;
    }
}
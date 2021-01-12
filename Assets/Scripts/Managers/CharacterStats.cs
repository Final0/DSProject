using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10, maxHealth, currentHealth, staminaLevel = 10; 
        public float currentStamina, maxStamina;

        public bool isDead;
    }
}
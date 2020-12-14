using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class PlayerStats : CharacterStats
    {
        public HealthBar healthBar;

        StaminaBar staminaBar;
        PlayerManager playerManager;
        AnimatorHandler animatorHandler;

        public bool canUseStamina = true;

        private void Awake()
        {
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;

            InvokeRepeating("RegenStamina", 0f, 1f);
        }

        private void Update()
        {
            NoStamina();
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death_01", true);
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenStamina()
        {
            if (currentStamina < maxStamina && !playerManager.isInteracting)
            {
                currentStamina += 5;
                staminaBar.SetCurrentStamina(currentStamina);
            }
        }

        public void NoStamina()
        {
            if (currentStamina <= 0)
            {
                StartCoroutine("NoStaminaAction");
            }
        }

        IEnumerator NoStaminaAction()
        {   
            currentStamina = 0;

            canUseStamina = false;

            yield return new WaitForSeconds(4f);

            canUseStamina = true;
        }
    }
}
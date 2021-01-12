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

        public float staminaRegenerationAmount = 1, staminaRegenTimer = 0f;

        public bool canUseStamina = true;

        private void Awake()
        {
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
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

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death_01", true);
                isDead = true;
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenStamina()
        {
            if(playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
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

            yield return new WaitForSeconds(3f);

            canUseStamina = true;
        }
    }
}
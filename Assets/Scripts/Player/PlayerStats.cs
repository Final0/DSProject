﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class PlayerStats : CharacterStats
    {
        public HealthBar healthBar;
        private StaminaBar staminaBar;
        private FocusPointBar focusPointBar;

        private PlayerManager playerManager;
        private AnimatorHandler animatorHandler;

        [SerializeField]
        private float staminaRegenerationAmount = 30f; 
            
        private float staminaRegenTimer = 0f;

        private float timerInv = 0f;

        private bool invulnerable = false;

        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();

            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);

            InvokeRepeating(nameof(RegenStamina), 0f, 1f);
        }

        private void Update()
        {
            TimerInvulnerability();
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

        private float SetMaxFocusPointFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }

        private void TimerInvulnerability()
        {
            if (invulnerable)
                timerInv += Time.deltaTime;

            if (timerInv >= 0.75f)
                invulnerable = false;
        }

        public void TakeDamage(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            if (!invulnerable)
            {
                currentHealth -= damage;
                healthBar.SetCurrentHealth(currentHealth);
            }

            invulnerable = true;

            animatorHandler.PlayTargetAnimation("PlayerDamage", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("PlayerDeath", true);
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

                if (currentStamina < maxStamina && staminaRegenTimer > 0.5f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth += healAmount;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints -= focusPoints;

            if (currentFocusPoints < 0)
                currentFocusPoints = 0;

            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class Respawn : MonoBehaviour
    {
        private PlayerStats playerStats;

        private InputHandler inputHandler;
        private CameraHandler cameraHandler;
        private AnimatorHandler animatorHandler;

        private HealthBar healthBar;
        private StaminaBar staminaBar;
        private FocusPointBar focusPointBar;

        [SerializeField]
        private Transform respawnPoint;

        private void Start()
        {
            playerStats = GetComponent<PlayerStats>();

            inputHandler = GetComponent<InputHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();

            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
        }

        private void Update()
        {
            if(playerStats.isDead)
            {
                Invoke(nameof(RespawnPlayer), 3f);
            }
        }

        private void RespawnPlayer()
        {
            animatorHandler.anim.Rebind();

            ResetStats();

            transform.position = respawnPoint.position;       
        }

        private void ResetStats()
        {
            playerStats.isDead = false;

            inputHandler.lockOnFlag = false;
            cameraHandler.ClearLockOnTargets();

            playerStats.currentHealth = playerStats.maxHealth;
            playerStats.currentStamina = playerStats.maxStamina;
            playerStats.currentFocusPoints = playerStats.maxFocusPoints;

            healthBar.SetCurrentHealth(playerStats.maxHealth);
            staminaBar.SetCurrentStamina(playerStats.maxStamina);
            focusPointBar.SetCurrentFocusPoints(playerStats.maxFocusPoints);
        }
    }
}
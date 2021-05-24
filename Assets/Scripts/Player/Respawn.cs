using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class Respawn : MonoBehaviour
    {
        private PlayerStats playerStats;
        private EnemyStats enemyStats;
        private AI ai;

        private InputHandler inputHandler;
        private CameraHandler cameraHandler;
        private AnimatorHandler animatorHandler;

        private HealthBar healthBar;
        private StaminaBar staminaBar;
        private FocusPointBar focusPointBar;

        [SerializeField]
        private Transform respawnPoint;

        private GameObject[] enemies;

        [SerializeField]
        private Animator respawnAnim;

        public bool stopBool = false;

        private Vector3[] enemiesTransform;

        private int i = 0;

        private PotionScript potionScript;

        private void Awake()
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            enemiesTransform = new Vector3[enemies.Length];

            for (int i = 0; i < enemies.Length; i++)
            {
                enemiesTransform[i] = enemies[i].gameObject.transform.position;
            }

        }

        private void Start()
        {
            playerStats = GetComponent<PlayerStats>();
           
            inputHandler = GetComponent<InputHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            potionScript = GetComponentInChildren<PotionScript>();

            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
        }

        private void Update()
        {
            if (playerStats.isDead)
                Invoke(nameof(RespawnPlayer), 3f);

            StopPlayer();
        }

        private void RespawnPlayer()
        {
            ResetStats();
            
            animatorHandler.anim.Rebind();

            respawnAnim.Play("Respawn");
            stopBool = true;
            Invoke(nameof(StartPlayer), 5f);

            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;

            Invoke(nameof(RespawnEnemy), 1f);
        }

        private void StartPlayer()
        {
            stopBool = false;
        }

        private void StopPlayer()
        {
            if (stopBool)
            {
                inputHandler.inventoryFlag = false;

                inputHandler.uiManager.CloseSelectWindow();
                inputHandler.uiManager.CloseAllInventoryWindows();
                inputHandler.uiManager.hudWindow.SetActive(true);

                inputHandler.horizontal = 0f;
                inputHandler.vertical = 0f;

                inputHandler.mouseX = 0f;
                inputHandler.mouseY = 0f;
            }
        }

        private void ResetStats()
        {
            playerStats.isDead = false;

            if(potionScript.nbPotion < 5)
                potionScript.nbPotion = 5;

            inputHandler.lockOnFlag = false;
            cameraHandler.ClearLockOnTargets();

            playerStats.currentHealth = playerStats.maxHealth;
            playerStats.currentStamina = playerStats.maxStamina;
            playerStats.currentFocusPoints = playerStats.maxFocusPoints;

            healthBar.SetCurrentHealth(playerStats.maxHealth);
            staminaBar.SetCurrentStamina(playerStats.maxStamina);
            focusPointBar.SetCurrentFocusPoints(playerStats.maxFocusPoints);
        }

        private void RespawnEnemy()
        {
            i = 0;

            foreach (GameObject enemy in enemies)
            {
                enemy.transform.position = enemiesTransform[i];

                enemyStats = enemy.GetComponent<EnemyStats>();

                enemyStats.currentHealth = enemyStats.maxHealth;
                enemyStats.isDead = false;
                enemyStats.ai.behaviour = enemyStats.ai.resetBehaviour;

                if (enemyStats.isBoss)
                {
                    enemyStats.bossHealthBar.SetCurrentHealth(enemyStats.currentHealth);
                    enemyStats.bossHealthBar.gameObject.SetActive(false);
                }

                enemy.SetActive(true);

                i++;
            }
        }
    }
}
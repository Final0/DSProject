using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class PotionScript : MonoBehaviour
    {
        private PlayerStats playerStats;
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;
        private PlayerManager playerManager;

        public int nbPotion = 5;

        [SerializeField]
        private GameObject potionGO;

        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();
            animatorHandler = GetComponent<AnimatorHandler>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();

            potionGO.SetActive(false);
        }

        private void Update()
        {
            if (inputHandler.drink_Input && !playerManager.isInteracting)
            {
                if (nbPotion != 0)
                {
                    animatorHandler.PlayTargetAnimation("Drink", true);

                    potionGO.SetActive(true);
                }    
                else
                    animatorHandler.PlayTargetAnimation("Shrug", true);
            }

            ChangeUI();
        }

        public void Potion()
        {
            nbPotion--;

            playerStats.currentHealth += 40;

            if (playerStats.currentHealth > playerStats.maxHealth)
                playerStats.currentHealth = playerStats.maxHealth;

            playerStats.healthBar.SetCurrentHealth(playerStats.currentHealth);
        }

        public void DestroyPotion()
        {
            potionGO.SetActive(false);
        }


        [SerializeField]
        private Text nbPotionText;

        private void ChangeUI()
        {
            nbPotionText.text = nbPotion.ToString();
        }
    }
}

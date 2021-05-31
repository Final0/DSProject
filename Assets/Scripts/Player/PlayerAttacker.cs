using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;
        private PlayerStats playerStats;
        private PlayerInventory playerInventory;
        private InputHandler inputHandler;
        private WeaponSlotManager weaponSlotManager;

        private string lastAttack;

        [SerializeField]
        private GameObject rightHand;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animatorHandler = GetComponent<AnimatorHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();

            rightArmCol.enabled = false;
            leftArmCol.enabled = false;
        }

        private void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if (lastAttack == weapon.TH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                }
            }
        }

        private void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                lastAttack = weapon.TH_Light_Attack_1;
            }
            else
            {
                weaponSlotManager.attackingWeapon = weapon;
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
                lastAttack = weapon.TH_Heavy_Attack_1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                lastAttack = weapon.OH_Heavy_Attack_1;
            }
        }

        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }

        public void HandleRTAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRTMeleeAction();
            }
        }

        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (!inputHandler.inventoryFlag)
            {
                if (playerManager.canDoCombo)
                {
                    inputHandler.comboFlag = true;
                    HandleWeaponCombo(playerInventory.rightWeapon);
                    inputHandler.comboFlag = false;
                }
                else
                {
                    animatorHandler.anim.SetBool("isUsingRightHand", true);
                    HandleLightAttack(playerInventory.rightWeapon);
                }
            }
        }

        private void PerformRTMeleeAction()
        {
            if (!inputHandler.inventoryFlag)
            {
                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;

            if (!inputHandler.inventoryFlag)
            {
                if (weapon.isFaithCaster)
                {
                    if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpeel)
                    {
                        if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                            playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                        else
                            animatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }

        public void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        }

        public void DestroySpell()
        {
            playerInventory.currentSpell.DestroySpell();
        }
        #endregion

        public BoxCollider rightArmCol;

        public BoxCollider leftArmCol;

        public void RightArm()
        {
            rightArmCol.enabled = true;
        }

        public void LeftArm()
        {
            leftArmCol.enabled = true;
        }

        public void StopArms()
        {
            rightArmCol.enabled = false;
            leftArmCol.enabled = false;
        }
    }
}
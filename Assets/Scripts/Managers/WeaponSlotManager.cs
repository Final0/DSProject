using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Midir
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;

        private PlayerManager playerManager;
        
        private WeaponHolderSlot leftHandSlot, rightHandSlot, backSlot;

        private DamageCollider leftHandDamageCollider, rightHandDamageCollider;

        private QuickSlotsUI quickSlotsUI;

        private PlayerStats playerStats;

        private InputHandler inputHandler;

        private PlayerAttacker playerAttacker;

        private Animator animator;

        private void Awake()
        {
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerAttacker = GetComponent<PlayerAttacker>();

            animator = GetComponent<Animator>();
            
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);

                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);

                    /*#region Handle Right Weapon Idle Animations
                    animator.CrossFade("Both Arms Empty", 0.2f);*/

                    backSlot.UnloadWeaponAndDestroy();

                    /*if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion*/
                }
            }     
        }

        #region Handle Weapon's Damage Collider
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
                rightHandDamageCollider.EnableDamageCollider();
            else if (playerManager.isUsingLeftHand)
                leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseHandDamageCollider()
        {
            if (rightHandDamageCollider != null)
                rightHandDamageCollider.DisableDamageCollider();
            else
            {
                playerAttacker.rightArmCol.enabled = false;
                playerAttacker.leftArmCol.enabled = false;
            }
            //leftHandDamageCollider.DisableDamageCollider();
        }
        #endregion

        #region Handle Weapon's Stamina Drainage
        public void DrainStaminaLightattack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyattack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion
    }
}
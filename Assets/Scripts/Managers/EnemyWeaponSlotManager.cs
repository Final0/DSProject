using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        private Animator anim;

        [SerializeField]
        private WeaponItem rightHandWeapon, leftHandWeapon;

        private WeaponHolderSlot rightHandSlot, leftHandSlot;

        private DamageCollider leftHandDamageCollider, rightHandDamageCollider;

        [SerializeField]
        private bool twoHandSlot;

        private void Awake()
        {
            anim = GetComponent<Animator>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        private void Start()
        {
            LoadWeaponsOnBothHands();
        }

        private void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);

                /*#region Handle Left Weapon Idle Animations
                if (weapon != null)
                {
                    anim.CrossFade(weapon.left_hand_idle, 0.2f);
                }
                else
                {
                    anim.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion*/
            }
            else
            {
                if (twoHandSlot)
                {
                    anim.CrossFade(weapon.th_idle, 0.2f);
                    rightHandSlot.currentWeapon = weapon;
                    rightHandSlot.LoadWeaponModel(weapon);
                    LoadWeaponsDamageCollider(false);
                }
                else
                {
                    rightHandSlot.currentWeapon = weapon;
                    rightHandSlot.LoadWeaponModel(weapon);
                    LoadWeaponsDamageCollider(false);

                    #region Handle Right Weapon Idle Animations
                    if (weapon != null)
                    {
                        anim.CrossFade(weapon.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        anim.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion
                }
            }
        }

        private void LoadWeaponsOnBothHands()
        {
                if (rightHandWeapon != null)
                {
                    LoadWeaponOnSlot(rightHandWeapon, false);
                }
                if (leftHandWeapon != null)
                {
                    LoadWeaponOnSlot(leftHandWeapon, true);
                }
        }

        private void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        [SerializeField]
        private BoxCollider KickCol;

        public void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
                rightHandDamageCollider.DisableDamageCollider();
            else if (KickCol != null)
                KickCol.enabled = false;

        }

        public void DrainStaminaLightattack()
        {

        }

        public void DrainStaminaHeavyattack()
        {

        }

        public void EnableCombo()
        {
            //anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            //anim.SetBool("canDoCombo", false);
        }
    }
}

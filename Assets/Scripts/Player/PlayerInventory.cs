using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponSlotManager weaponSlotManager;

        public SpellItem currentSpell;

        public WeaponItem rightWeapon, leftWeapon, unarmedWeapon;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1], weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = 0, currentLeftWeaponIndex = 0;

        public List<WeaponItem> weaponsInventory;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }

        public void ChangeRightWeapon()
        {
            if(weaponsInRightHandSlots.Length != 1)
                currentRightWeaponIndex += 1;

            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1 && currentRightWeaponIndex != 0)
            {
                currentRightWeaponIndex = 0;
                rightWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            }

            if (currentRightWeaponIndex == 0)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 1)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }

            
        }

        public void ChangeLeftWeapon()
        {
            if (weaponsInLeftHandSlots.Length != 1)
                currentLeftWeaponIndex += 1;
            
            if (currentLeftWeaponIndex == 0)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 1)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1 && currentLeftWeaponIndex != 0)
            {
                currentLeftWeaponIndex = 0;
                leftWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            }
        }
    }
}
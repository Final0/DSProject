using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class HandEquipementSlotUI : MonoBehaviour
    {
        public Image icon;
        WeaponItem weapon;

        public bool rightHandSlot01, rightHandSlot02, leftHandSlot01, leftHandSlot02;

        public void AddItem(WeaponItem newWeapon)
        {
            weapon = newWeapon;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
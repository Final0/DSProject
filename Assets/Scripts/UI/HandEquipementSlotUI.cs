using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class HandEquipementSlotUI : MonoBehaviour
    {
        private UIManager uiManager;
        
        private WeaponItem weapon;

        [SerializeField]
        private Image icon;

        public bool rightHandSlot01, rightHandSlot02, leftHandSlot01, leftHandSlot02;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

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

        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uiManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uiManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uiManager.leftHandSlot01Selected = true;
            }
            else if (leftHandSlot02)
            {
                uiManager.leftHandSlot02Selected = true;
            }
        }
    }
}
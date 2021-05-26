using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] 
        private PlayerInventory playerInventory;

        [Header("UI Windows")]
        [SerializeField] private GameObject selectWindow;
        [SerializeField] private GameObject equipementScreenWindow;
        [SerializeField] private GameObject weaponInventoryWindow;
        [SerializeField] private GameObject optionsWindow;
        public GameObject hudWindow;

        [HideInInspector]
        public bool rightHandSlot01Selected;

        [HideInInspector]
        public bool rightHandSlot02Selected;

        [HideInInspector]
        public bool leftHandSlot01Selected;

        [HideInInspector]
        public bool leftHandSlot02Selected;

        [Header("WeaponInventory")]
        [SerializeField] private GameObject weaponInventorySlotPrefab;
        [SerializeField] private Transform weaponInventorySlotsParent;

        private WeaponInventorySlot[] weaponInventorySlots;

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        }

        public void UpdateUi()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }

                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipementScreenWindow.SetActive(false);
            optionsWindow.SetActive(false);
            Time.timeScale = 1f;
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class EquipementWindowUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected, rightHandSlot02Selected, leftHandSlot01Selected, leftHandSlot02Selected;

        HandEquipementSlotUI[] handEquipementSlotUI;

        private void Start()
        {
            handEquipementSlotUI = GetComponentsInChildren<HandEquipementSlotUI>();
        }

        public void LoadWeaponsOnEquipementScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipementSlotUI.Length; i++)
            {
                if (handEquipementSlotUI[i].rightHandSlot01)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (handEquipementSlotUI[i].rightHandSlot02)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (handEquipementSlotUI[i].leftHandSlot01)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else if (handEquipementSlotUI[i].leftHandSlot02)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }
    }
}
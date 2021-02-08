using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class EquipementWindowUI : MonoBehaviour
    {
        public HandEquipementSlotUI[] handEquipementSlotUI;

        public void LoadWeaponsOnEquipementScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipementSlotUI.Length; i++)
            {
                if (handEquipementSlotUI[i].rightHandSlot01)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                /*else if (handEquipementSlotUI[i].rightHandSlot02)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }*/
                else if (handEquipementSlotUI[i].leftHandSlot01)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                /*else if (handEquipementSlotUI[i].leftHandSlot02)
                {
                    handEquipementSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }*/
            }
        }
    }
}
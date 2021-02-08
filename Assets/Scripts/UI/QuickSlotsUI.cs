using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class QuickSlotsUI : MonoBehaviour
    {
        [SerializeField] 
        private Image leftWeaponIcon, rightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
        {
            if (isLeft == false)
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                } 
            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }   
            }
        }
    }
}
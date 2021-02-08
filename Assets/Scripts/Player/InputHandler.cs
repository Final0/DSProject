using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class InputHandler : MonoBehaviour
    {
        [HideInInspector]
        public float horizontal, vertical, moveAmount, mouseX, mouseY, rollInputTimer;

        [HideInInspector]
        public bool b_Input, a_Input, y_Input, rb_Input, rt_Input, jump_Input, inventory_Input, lockOnInput, right_Stick_Right_Input;

        [HideInInspector]
        public bool right_Stick_Left_Input;

        [HideInInspector]
        public bool d_Pad_Up, d_Pad_Right, d_Pad_Down, d_Pad_Left;

        [HideInInspector]
        public bool rollFlag, twoHandFlag, sprintFlag, comboFlag, inventoryFlag, lockOnFlag;

        private PlayerControls inputActions;
        private PlayerLocomotion playerLocomotion;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;
        private WeaponSlotManager weaponSlotManager;
        private CameraHandler cameraHandler;
        private UIManager uiManager;
        private PlayerStats playerStats;
        private WeaponHolderSlot weaponHolderSlot;

        private Vector2 movementInput, cameraInput;

        private void Awake()
        {
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerStats = FindObjectOfType<PlayerStats>();
            weaponHolderSlot = FindObjectOfType<WeaponHolderSlot>();
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();

                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput();
            HandleRollInput(delta);
            HandleAttackInput();
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
        }

        private void HandleMoveInput()
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            if (playerLocomotion.rollingStamina > playerStats.currentStamina)
                return;

            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            sprintFlag = b_Input;

            if (b_Input)
            {
                rollInputTimer += delta;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput()
        {
            if ((playerManager.isInteracting && !playerManager.canDoCombo) || !playerStats.canUseStamina)
                return;

            if (rb_Input && weaponHolderSlot.currentWeapon.baseStamina * weaponHolderSlot.currentWeapon.lightAttackMultiplier < playerStats.currentStamina)
            {
                playerAttacker.HandleRBAction();
            }

            if (rt_Input && weaponHolderSlot.currentWeapon.baseStamina * weaponHolderSlot.currentWeapon.heavyAttackMultiplier < playerStats.currentStamina)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag; 

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUi();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && !lockOnFlag)
            {
                lockOnInput = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if (y_Input && playerInventory.rightWeapon != playerInventory.unarmedWeapon)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }
    }
}
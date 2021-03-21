using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class PlayerManager : CharacterManager
    {
        private Animator anim;

        private PlayerStats playerStats;
        private InputHandler inputHandler;
        private CameraHandler cameraHandler;
        private PlayerLocomotion playerLocomotion;
        private InteractableUI interactableUI;
        private AnimatorHandler animatorHandler;

        [SerializeField]
        private GameObject interactableUIGameObject;

        public GameObject itemInteractableGameObject;

        [HideInInspector]
        public bool isInteracting;

        [HideInInspector]
        public bool isSprinting;

        [HideInInspector]
        public bool isInAir;

        [HideInInspector]
        public bool isGrounded;

        [HideInInspector]
        public bool canDoCombo;

        [HideInInspector]
        public bool isUsingRightHand;

        [HideInInspector]
        public bool isUsingLeftHand;

        [HideInInspector]
        public bool isInvulnerable;

        public static PlayerManager Singleton;
        
        private void Awake()
        {
            Singleton = this;

            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerStats = GetComponent<PlayerStats>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
    
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            animatorHandler.canRotate = anim.GetBool("canRotate");

            inputHandler.TickInput(delta);
            
            playerLocomotion.HandleJumping();
            playerStats.RegenStamina();

            playerLocomotion.HandleRollingAndSprinting();

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            playerLocomotion.HandleMovement();
            playerLocomotion.HandleFalling(playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation();
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.d_Pad_Up= false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);

                float normalizeMouseXspeed = 2f;//1167 / (float)Screen.width;
                float normalizeMouseYspeed = 2f;//579 / (float)Screen.height;
                
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX * normalizeMouseXspeed, inputHandler.mouseY * normalizeMouseYspeed);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        public void CheckForInteractableObject()
        {
            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out RaycastHit hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactbleText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }
    }
}
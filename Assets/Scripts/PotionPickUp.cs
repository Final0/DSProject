using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class PotionPickUp : Interactable
    {
        [SerializeField]
        private Texture potionIcon;

        [SerializeField]
        private int nbPotionAdded;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpPotion(playerManager);
        }

        public void PickUpPotion(PlayerManager playerManager)
        {
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;
            PotionScript potionScript;

            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            potionScript = playerManager.GetComponentInChildren<PotionScript>();

            potionScript.nbPotion += nbPotionAdded;

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = "Potion x" + nbPotionAdded.ToString();
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = potionIcon;
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
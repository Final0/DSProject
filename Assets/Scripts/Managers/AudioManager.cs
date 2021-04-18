using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Midir
{
    public class AudioManager : MonoBehaviour
    {
        private PlayerManager playerManager;
        private InputHandler inputHandler;

        private AudioSource footsteps;
        private AudioSource grassFootsteps;
        private AudioSource roll;
        private AudioSource sword;

        [SerializeField]
        private AudioClip footstepsClip;
        [SerializeField]
        private AudioClip grassFootstepsClip;
        [SerializeField]
        private AudioClip rollClip;
        [SerializeField]
        private AudioClip swordClip;

        private LayerMask grass = 12;
        private LayerMask ground = 13;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            inputHandler = GetComponentInParent<InputHandler>();

            footsteps = gameObject.AddComponent<AudioSource>();
            grassFootsteps = gameObject.AddComponent<AudioSource>();
            roll = gameObject.AddComponent<AudioSource>();
            sword = gameObject.AddComponent<AudioSource>();

            footsteps.clip = footstepsClip;
            grassFootsteps.clip = grassFootstepsClip;
            roll.clip = rollClip;
            sword.clip = swordClip;
        }
        
        private void RandomVolumeAndPlay(AudioSource audioSource)
        {
            audioSource.volume = Random.Range(0.8f, 1);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();
        }

        public void SwordAudio()
        {
            RandomVolumeAndPlay(sword);
        }

        public void RollAudio()
        {
            RandomVolumeAndPlay(roll);
        }

        public void FootstepAudio()
        {
            Vector3 origin = transform.position + new Vector3(0f, 1f, 0f);

            if (Physics.Raycast(origin, -Vector3.up, out RaycastHit hit))
            {
                if (!playerManager.isInteracting && inputHandler.moveAmount > 0 && hit.transform.gameObject.layer == grass)
                {
                    RandomVolumeAndPlay(grassFootsteps);
                }
                else if (!playerManager.isInteracting && inputHandler.moveAmount > 0 && hit.transform.gameObject.layer == ground)
                {
                    RandomVolumeAndPlay(footsteps);
                }
            }     
        }
    }
}
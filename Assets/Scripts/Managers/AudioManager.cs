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
        private AudioSource landing;
        private AudioSource pickUpItem;
        private AudioSource bossMusic;
        private AudioSource death;
        private AudioSource defeat;
        private AudioSource forestMusic;
        private AudioSource potion;
        private AudioSource heal;
        private AudioSource punch;
        private AudioSource changeWeapon;

        [SerializeField]
        private AudioClip footstepsClip;
        [SerializeField]
        private AudioClip grassFootstepsClip;
        [SerializeField]
        private AudioClip rollClip;
        [SerializeField]
        private AudioClip swordClip;
        [SerializeField]
        private AudioClip landingClip;
        [SerializeField]
        private AudioClip pickUpItemClip;
        [SerializeField]
        private AudioClip bossMusicClip;
        [SerializeField]
        private AudioClip deathClip;
        [SerializeField]
        private AudioClip defeatClip;
        [SerializeField]
        private AudioClip forestMusicClip;
        [SerializeField]
        private AudioClip potionClip;
        [SerializeField]
        private AudioClip healClip;
        [SerializeField]
        private AudioClip punchClip;
        [SerializeField]
        private AudioClip changeWeaponClip;

        [SerializeField]
        private AudioMixer audioMixer;

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
            landing = gameObject.AddComponent<AudioSource>();
            pickUpItem = gameObject.AddComponent<AudioSource>();
            bossMusic = gameObject.AddComponent<AudioSource>();
            death = gameObject.AddComponent<AudioSource>();
            defeat = gameObject.AddComponent<AudioSource>();
            forestMusic = gameObject.AddComponent<AudioSource>();
            potion = gameObject.AddComponent<AudioSource>();
            heal = gameObject.AddComponent<AudioSource>();
            punch = gameObject.AddComponent<AudioSource>();
            changeWeapon = gameObject.AddComponent<AudioSource>();

            footsteps.clip = footstepsClip;
            grassFootsteps.clip = grassFootstepsClip;
            roll.clip = rollClip;
            sword.clip = swordClip;
            landing.clip = landingClip;
            pickUpItem.clip = pickUpItemClip;
            bossMusic.clip = bossMusicClip;
            death.clip = deathClip;
            defeat.clip = defeatClip;
            forestMusic.clip = forestMusicClip;
            potion.clip = potionClip;
            heal.clip = healClip;
            punch.clip = punchClip;
            changeWeapon.clip = changeWeaponClip;

            AudioMixerGroup[] SFXGroup = audioMixer.FindMatchingGroups("SFX");
            AudioMixerGroup[] MusicGroup = audioMixer.FindMatchingGroups("Music");

            footsteps.outputAudioMixerGroup = SFXGroup[0];
            grassFootsteps.outputAudioMixerGroup = SFXGroup[0];
            roll.outputAudioMixerGroup = SFXGroup[0];
            sword.outputAudioMixerGroup = SFXGroup[0];
            landing.outputAudioMixerGroup = SFXGroup[0];
            pickUpItem.outputAudioMixerGroup = SFXGroup[0];
            bossMusic.outputAudioMixerGroup = MusicGroup[0];
            death.outputAudioMixerGroup = SFXGroup[0];
            defeat.outputAudioMixerGroup = SFXGroup[0];
            forestMusic.outputAudioMixerGroup = MusicGroup[0];
            potion.outputAudioMixerGroup = SFXGroup[0];
            heal.outputAudioMixerGroup = SFXGroup[0];
            punch.outputAudioMixerGroup = SFXGroup[0];
            changeWeapon.outputAudioMixerGroup = SFXGroup[0];

            ForestMusic();
        }

        [SerializeField]
        private GameObject options;

        private void Update()
        {
            ChangeWeaponAudio();

            if (options.activeSelf && inputHandler.inventory_Input)
            {
                if (forestPause)
                    forestMusic.Play();

                if (bossPause)
                    bossMusic.Play();
            }
        }

        private void RandomVolumeAndPlay(AudioSource audioSource)
        {
            audioSource.volume = Random.Range(0.8f, 1);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();
        }

        private void PlayMusic(AudioSource audioSource)
        {
            audioSource.Play();
            audioSource.loop = true;
        }

        private void ChangeWeaponAudio()
        {
            if (inputHandler.d_Pad_Right && !inputHandler.inventoryFlag && !inputHandler.playerManager.isInteracting)
                changeWeapon.Play();
        }

        private bool forestPause = false;

        private bool bossPause = false;

        public void PauseMusic()
        {
            if (forestMusic.isPlaying)
            {
                forestMusic.Pause();
                forestPause = true;
            }
            else
                forestPause = false;

            if (bossMusic.isPlaying)
            {
                bossMusic.Pause();
                bossPause = true;
            }
            else
                bossPause = false;      
        }

        public void ResumeMusic()
        {
            if (forestPause)
                forestMusic.Play();

            if (bossPause)
                bossMusic.Play();
        }

        public void ClearAudio()
        {
            forestMusic.Stop();
            bossMusic.Stop();
        }

        private void ForestMusic()
        {
            PlayMusic(forestMusic);
        }

        public void DeathAudio()
        {
            death.Play();
        }

        public void DefeatAudio()
        {
            defeat.Play();
        }

        public void BossMusic()
        {
            PlayMusic(bossMusic);
        }

        public void PickUpItemAudio()
        {
            pickUpItem.Play();
        }

        public void LandingAudio()
        {
            RandomVolumeAndPlay(landing);
        }

        public void SwordAudio()
        {
            RandomVolumeAndPlay(sword);
        }

        public void RollAudio()
        {
            RandomVolumeAndPlay(roll);
        }

        public void DrinkPotion()
        {
            potion.Play();
        }

        public void Healing()
        {
            heal.Play();
        }

        public void PunchSound()
        {
            punch.Play();
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
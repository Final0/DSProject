using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Midir
{
    public class EnemyAudioManager : MonoBehaviour
    {
        private AudioSource kick;
        private AudioSource swordSwoosh;
        private AudioSource slash;

        [SerializeField]
        private AudioClip kickClip;
        [SerializeField]
        private AudioClip swordSwooshClip;
        [SerializeField]
        private AudioClip slashClip;

        [SerializeField]
        private AudioMixer audioMixer;

        private void Awake()
        {
            kick = gameObject.AddComponent<AudioSource>();
            swordSwoosh = gameObject.AddComponent<AudioSource>();
            slash = gameObject.AddComponent<AudioSource>();

            kick.clip = kickClip;
            swordSwoosh.clip = swordSwooshClip;
            slash.clip = slashClip;

            AudioMixerGroup[] SFXGroup = audioMixer.FindMatchingGroups("SFX");

            kick.outputAudioMixerGroup = SFXGroup[0];
        }

        private void RandomVolumeAndPlay(AudioSource audioSource)
        {
            audioSource.volume = Random.Range(0.8f, 1);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();
        }

        public void SwooshAudio()
        {
            RandomVolumeAndPlay(swordSwoosh);
        }

        public void KickAudio()
        {
            kick.Play();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class ForestMusic : MonoBehaviour
    {
        private AudioManager audioManager;

        [SerializeField]
        private bool enter;

        [SerializeField]
        private bool exit;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && enter)
            {
                audioManager.forestMusic.Stop();
            }
            else if (other.gameObject.CompareTag("Player") && exit)
            {
                if(!audioManager.forestMusic.isPlaying)
                    audioManager.forestMusic.Play();
            }
        }
    }
}
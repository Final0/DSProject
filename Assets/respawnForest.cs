using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class respawnForest : MonoBehaviour
    {
        AudioManager audioManager;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        public void MusicRespawn()
        {
            audioManager.forestMusic.Play();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class DestroySlashFX : MonoBehaviour
    {
        private ParticleSystem SlashFX;

        private void Awake()
        {
            SlashFX = GetComponent<ParticleSystem>();

            Destroy(transform.parent.gameObject, SlashFX.main.duration + SlashFX.main.startLifetimeMultiplier);
        }
    }
}
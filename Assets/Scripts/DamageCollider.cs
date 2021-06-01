using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;

        private AudioManager audioManager;

        [SerializeField]
        private int currentWeaponDamage = 25;

        public bool isSword = true;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;

            audioManager = FindObjectOfType<AudioManager>();
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player")) 
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                
                if (playerStats != null)
                {
                    audioManager.SwordAudio();
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if (collision.CompareTag("Enemy"))
            {
                if (transform.root.GetComponent<PlayerStats>() != null)
                {
                    EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                    
                    if (enemyStats != null)
                    {
                        if (isSword)
                            audioManager.SwordAudio();
                        else
                            audioManager.PunchSound();

                        enemyStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }
        }
    }
}
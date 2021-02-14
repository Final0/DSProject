using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class EnemyStats : CharacterStats
    {
        private Animator animator;

        private BossHealthBar bossHealthBar;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            bossHealthBar = FindObjectOfType<BossHealthBar>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            bossHealthBar.SetMaxHealth(maxHealth);
            bossHealthBar.SetCurrentHealth(currentHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            bossHealthBar.SetCurrentHealth(currentHealth);

            animator.Play("Damage_01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death_01");
                isDead = true;
            }
        }
    }
}
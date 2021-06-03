using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10, maxHealth, currentHealth;

        public bool isDead;

        private Animator animator;

        [HideInInspector]
        public BossHealthBar bossHealthBar;

        public AI ai;

        public bool canCancel = false;

        private float timerCancel = 0f;

        private int hitCombo = 0;

        public bool isBoss = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            ai = GetComponent<AI>();
            
            if (isBoss)
                bossHealthBar = FindObjectOfType<BossHealthBar>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;

            if (isBoss)
            {
                bossHealthBar.SetMaxHealth(maxHealth);
                bossHealthBar.SetCurrentHealth(currentHealth);

                bossHealthBar.gameObject.SetActive(false);
            }  
        }

        [SerializeField]
        private GameObject end;

        [SerializeField]
        private GameObject[] desactivates;

        private void Update()
        {
            if(isBoss && isDead)
            {
                bossHealthBar.gameObject.SetActive(false);

                Invoke(nameof(End), 3f);
                
            }
        }

        private void End()
        {
            end.SetActive(true);
            Cursor.visible = true;
            foreach (GameObject desactivate in desactivates)
            {
                desactivate.SetActive(false);
            }
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            hitCombo++;

            ai.ambush = false;

            if (isDead)
                return;

            currentHealth -= damage;

            if (isBoss)
                bossHealthBar.SetCurrentHealth(currentHealth);

            if(canCancel)
                animator.Play("EnemyDamage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("EnemyDeath");
                isDead = true;
            }
        }

        public void CancelDelai()
        {
            timerCancel += Time.deltaTime;

            if (hitCombo == 1)
                canCancel = true;
            else if (timerCancel >= 5f)
            {
                canCancel = false;
                timerCancel = 0f;
                hitCombo = 0;
            }
        }
    }
}
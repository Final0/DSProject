using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class BossInvisibleWall : MonoBehaviour
    {
        private BoxCollider checkPlayer;

        public BoxCollider bossFightWall;

        private void Awake()
        {
            checkPlayer = GetComponent<BoxCollider>();

            bossFightWall.enabled = false;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.CompareTag("Player"))
            {
                bossFightWall.enabled = true;
            }
        }
    }
}
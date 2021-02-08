using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        private float radius = 0.6f;

        public string interactbleText;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {

        }
    }
}
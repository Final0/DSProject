using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        [SerializeField]
        private CapsuleCollider characterCollider, characterCollisionBlockerCollider;

        private void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }
    }
}
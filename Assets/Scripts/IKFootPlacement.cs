using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Midir
{
    public class IKFootPlacement : MonoBehaviour
    {
        private Animator anim;

        [SerializeField]
        private LayerMask layerMask;

        [Range (0, 1f)] [SerializeField]
        private float DistanceToGround;

        private InputHandler inputHandler;

        private void Start()
        {
            anim = GetComponent<Animator>();

            inputHandler = GetComponentInParent<InputHandler>();
        }

        private void OnAnimatorIK()
        {
            if (anim && !inputHandler.lockOnFlag)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

                RaycastHit hit;
                Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

                if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
                {
                    if (hit.transform.CompareTag("Walkable"))
                    {
                        Vector3 footPosition = hit.point;
                        footPosition.y += DistanceToGround;
                        anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                        anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                    }
                }

                ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

                if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
                {
                    if (hit.transform.CompareTag("Walkable"))
                    {
                        Vector3 footPosition = hit.point;
                        footPosition.y += DistanceToGround;
                        anim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                        anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                    }
                }
            }
            /*else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);

                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
            }*/
        }
    }
}
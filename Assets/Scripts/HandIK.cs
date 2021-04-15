using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    [Range(0, 1)]
    private float leftValue = 1f;

    [SerializeField]
    [Range(0, 1)]
    private float rightValue = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (anim)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftValue);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftValue);

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightValue);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightValue);
        }
    }
}

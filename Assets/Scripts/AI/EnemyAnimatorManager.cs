﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class EnemyAnimatorManager : MonoBehaviour
    {
        [HideInInspector]
        public Animator anim;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}

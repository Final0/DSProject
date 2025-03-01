﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class AnimatorManager : MonoBehaviour
    {
        [HideInInspector]
        public Animator anim;

        public bool canRotate;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", false);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}

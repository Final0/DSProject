using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class FocusPointBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxFocusPoints(float maxFocusPoints)
        {
            slider.maxValue = maxFocusPoints;
            slider.value = maxFocusPoints;
        }

        public void SetCurrentFocusPoints(float currentFocusPoints)
        {
            slider.value = currentFocusPoints;
        }
    }
}
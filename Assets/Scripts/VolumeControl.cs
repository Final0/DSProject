using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Midir
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField]
        private string volumeParameter = "MasterVolume";

        [SerializeField]
        private AudioMixer mixer;

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private Toggle toggle;

        private float multiplier = 30f;

        private bool disableToggleEvent;

        private void Awake()
        {
            slider.onValueChanged.AddListener(HandleSliderValueChanged);

            toggle.onValueChanged.AddListener(HandleToggleValueChanged);
        }

        private void HandleToggleValueChanged(bool eneableSound)
        {
            if (disableToggleEvent)
                return;

            if (eneableSound)
                slider.value = slider.maxValue;
            else
                slider.value = slider.minValue;
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(volumeParameter, slider.value);
        }

        private void HandleSliderValueChanged(float value)
        {
            mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);

            disableToggleEvent = true;
            toggle.isOn = slider.value > slider.minValue;
            disableToggleEvent = false;
        }

        private void Start()
        {
            slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);

            mixer.SetFloat(volumeParameter, Mathf.Log10(slider.value) * multiplier);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class SettingManager : MonoBehaviour
    {
        [SerializeField]
        private int resolutionIndex;

        [SerializeField]
        private Toggle fullscreenToggle;

        [SerializeField]
        private Dropdown resolutionDropdown;

        [SerializeField]
        private Resolution[] resolutions;

        private void Start()
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreenToggle.isOn") == 1;
        }

        private void OnEnable()
        {
            fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
            resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });

            resolutions = Screen.resolutions;

            foreach(Resolution resolution in resolutions)
            {
                resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
            }
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("fullscreenToggle.isOn", fullscreenToggle.isOn ? 1 : 0);
        }

        private void OnResolutionChange()
        {
            Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        }

        private void OnFullscreenToggle()
        {
            Screen.fullScreen = fullscreenToggle.isOn;
        }
    }
}
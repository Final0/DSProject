using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Midir
{
    public class MenuManager : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ReturnMain()
        {
            SceneManager.LoadScene(0);
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void UnPause()
        {
            Time.timeScale = 1;
        }
    }
}
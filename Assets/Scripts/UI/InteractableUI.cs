using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Midir
{
    public class InteractableUI : MonoBehaviour
    {
        public Text interactableText;

        [SerializeField]
        private Text itemText;

        [SerializeField]
        private RawImage itemImage;
    }
}
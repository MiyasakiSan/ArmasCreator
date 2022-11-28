using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArmasCreator.Gameplay.UI
{
    public class PresetBarController : MonoBehaviour
    {
        [SerializeField]
        private Button presetBarButton;
        [SerializeField]
        private GameObject presetBar;

        private bool isShowBar = true;

        private void Awake()
        {
            
        }

        public void IsShowBar()
        {
            isShowBar = !isShowBar;
            presetBar.SetActive(isShowBar);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.UI
{
    public class OptionPanelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainContent;
        
        public void ShowOptionPanel()
        {
            mainContent.SetActive(true);
        }

        public void DeactivateOptionPanel()
        {
            mainContent.SetActive(false);
        }
    }
}

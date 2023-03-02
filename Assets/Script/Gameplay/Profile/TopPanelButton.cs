using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArmasCreator.UI
{
    public class TopPanelButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject blackOutline;
        [SerializeField]
        private GameObject redOutline;

        public void OnSelected()
        {
            redOutline.SetActive(true);
            blackOutline.SetActive(false);
        }

        public void OnDeSelected()
        {
            redOutline.SetActive(false);
            blackOutline.SetActive(true);
        }
    }
}

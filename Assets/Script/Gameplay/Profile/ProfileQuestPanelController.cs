using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.UI
{
    public class ProfileQuestPanelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainContent;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetUpQuestPanel()
        {
            mainContent.SetActive(true);
        }

        public void DeactivateQuestPanel()
        {
            mainContent.SetActive(false);
        }

        public void PopulateSideQuestNode()
        {

        }
    }
}

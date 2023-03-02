using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.UI
{
    public class UIRotateTowardPlayer : MonoBehaviour
    {
        [SerializeField]
        private GameObject content;

        private GameObject targetCam;

        private void Awake()
        {
            targetCam = GameObject.Find("NormalVcam");
        }

        private void Update()
        {
            if (!content.activeSelf) { return; }

            var target = new Vector3(targetCam.transform.position.x, transform.position.y, targetCam.transform.position.z);

            this.transform.LookAt(target);
        }

        public void Show()
        {
            content.SetActive(true);
        }

        public void Hide()
        {
            content.SetActive(false);
        }
    }
}


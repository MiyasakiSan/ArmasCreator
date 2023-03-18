using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ArmasCreator.UI
{
    public class TopPanelButton : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
    {
        [SerializeField]
        private GameObject blackOutline;
        [SerializeField]
        private GameObject redOutline;
        [SerializeField]
        private List<RectTransform> childObjects;

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (redOutline.activeSelf)
            {
                return;
            }
            SetChildSize(1.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (redOutline.activeSelf)
            {
                return;
            }
            SetChildSize(1);
        }

        public void SetChildSize(float childSize)
        {
            foreach (RectTransform child in childObjects)
            {
                child.localScale = new Vector3 (childSize, childSize , childSize);
            }
        }
    }
}

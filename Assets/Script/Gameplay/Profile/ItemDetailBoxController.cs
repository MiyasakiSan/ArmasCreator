using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArmasCreator.UI
{
    public class ItemDetailBoxController : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private Image iconTypeImage;
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private TMP_Text detailText;

        public void OnMouseEnterItemSlot(Sprite newIconSprite,Sprite newIconTypeSprite,string newNameText,string newDetailText)
        {
            this.gameObject.SetActive(true);
            SetItemDetailBox(newIconSprite, newIconTypeSprite, newNameText, newDetailText);
        }

        public void OnMouseExitItemSlot()
        {
            this.gameObject.SetActive(false);
        }

        private void SetItemDetailBox(Sprite newIconSprite, Sprite newIconTypeSprite, string newNameText, string newDetailText)
        {
            iconImage.sprite = newIconSprite;
            //iconTypeImage.sprite = newIconTypeSprite;
            nameText.text = newNameText;
            detailText.text = newDetailText;
        }
    }
}

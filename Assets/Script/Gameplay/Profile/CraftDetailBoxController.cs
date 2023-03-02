using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArmasCreator.UI
{
    public class CraftDetailBoxController : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private Image iconTypeImage;
        [SerializeField]
        private TMP_Text nameText;

        public void OnMouseEnterItemSlot(Sprite newIconSprite,Sprite newIconTypeSprite,string newNameText,string newDetailText)
        {
            this.gameObject.SetActive(true);
            SetCraftDetailBox(newIconSprite, newIconTypeSprite, newNameText);
        }

        public void OnMouseExitItemSlot()
        {
            this.gameObject.SetActive(false);
        }

        private void SetCraftDetailBox(Sprite newIconSprite, Sprite newIconTypeSprite, string newNameText)
        {
            iconImage.sprite = newIconSprite;
            iconTypeImage.sprite = newIconTypeSprite;
            nameText.text = newNameText;
        }
    }
}

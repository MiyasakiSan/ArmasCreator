using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArmasCreator.UI
{
    public class EquipmentDetailBoxController : MonoBehaviour
    {
        /*[SerializeField]
        private Image iconImage;*/
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private TMP_Text Atk;
        [SerializeField]
        private TMP_Text Def;
        [SerializeField]
        private TMP_Text Vit;
        [SerializeField]
        private TMP_Text Crit;

        public void OnMouseEnterItemSlot(Sprite newIconSprite, string newNameText, float newAtk, float newDef, float newVit, float newCrit)
        {
            this.gameObject.SetActive(true);
            SetItemDetailBox(newIconSprite, newNameText, newAtk, newDef, newVit, newCrit);
        }

        public void OnMouseExitItemSlot()
        {
            this.gameObject.SetActive(false);
        }

        private void SetItemDetailBox(Sprite newIconSprite, string newNameText, float newAtk, float newDef, float newVit, float newCrit)
        {
            //iconImage.sprite = newIconSprite;
            nameText.text = newNameText;
            Atk.text = "ATK  : " + newAtk.ToString();
            Def.text = "DEF  : " + newDef.ToString();
            Vit.text = "VIT   : " + newVit.ToString();
            Crit.text = "CRIT : " + newCrit.ToString();
        }
    }
}

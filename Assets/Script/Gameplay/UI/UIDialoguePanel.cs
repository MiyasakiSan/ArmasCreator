using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace ArmasCreator.UI 
{
    public class UIDialoguePanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image displayImage;

        [SerializeField]
        private TextMeshProUGUI headerText;

        [SerializeField]
        private TextMeshProUGUI dialogueText;

        private const float delay = 0.1f;
        private bool isShowingDialogue;

        private Sprite defaultSprite;

        public delegate void OnDialogueClickCallback();
        public event OnDialogueClickCallback OnDialogueClick;

        [SerializeField]
        private GameObject dialoguePopup;

        private void Awake()
        {
            defaultSprite = displayImage.sprite;
        }

        public void Reset()
        {
            displayImage.sprite = defaultSprite;
            dialogueText.text = string.Empty;
            headerText.text = string.Empty;
        }

        public void Show()
        {
            dialoguePopup.SetActive(true);
        }

        public void Hide()
        {
            dialoguePopup.SetActive(false);
        }

        public void SetDisplayImage(Sprite sprite)
        {
            displayImage.sprite = sprite;
        }

        public void SetHeaderText(string name)
        {
            headerText.text = name;
        }

        public void SetDisplayText(string text)
        {
            StartCoroutine(ShowText(text));
        }

        IEnumerator ShowText(string text)
        {
            isShowingDialogue = true;

            for(int i = 0; i < text.Length; i++)
            {
                if (!isShowingDialogue) { break; }

                dialogueText.text = text.Substring(0, i);
                yield return new WaitForSeconds(delay);
            }

            isShowingDialogue = false;
            dialogueText.text = text;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isShowingDialogue)
            {
                isShowingDialogue = false;
                return;
            }

            OnDialogueClick?.Invoke();
        }
    }
}


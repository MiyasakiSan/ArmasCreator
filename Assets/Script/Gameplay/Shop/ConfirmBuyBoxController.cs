using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ArmasCreator.UI
{
    public class ConfirmBuyBoxController : MonoBehaviour
    {
        [SerializeField]
        private Button arrowUpButton;
        [SerializeField]
        private Button arrowDownButton;
        [SerializeField]
        private Button buyButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Image itemIcon;
        [SerializeField]
        private TMP_Text itemNameText;
        [SerializeField]
        private TMP_Text priceText;
        [SerializeField]
        private TMP_InputField buyAmountText;

        private int buyAmount;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

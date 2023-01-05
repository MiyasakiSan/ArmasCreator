using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArmasCreator.UI
{
    public class BuyShopPanelController : MonoBehaviour
    {
        [SerializeField]
        private BuyItemNode buyItemNode;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateBuyItemNode()
        {
            var insBuyItemNode = Instantiate(buyItemNode.gameObject);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.UI;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;
using ArmasCreator.UserData;

namespace ArmasCreator.UI
{
    public class EquipmentPanelController : MonoBehaviour
    {
        [SerializeField]
        private EquipmentItemButton equipmentItemButtonPrefab;
        [SerializeField]
        private GameObject blankItemNodePrefab;
        [SerializeField]
        private Transform inventoryBoxContentTransform;
        [SerializeField]
        private GameObject mainContent;
        [SerializeField]
        private EquipmentBoxController equipmentBoxController;

        private UserDataManager userDataManager;

        private GameDataManager gameDataManager;

        private List<GameObject> listEquipment = new List<GameObject>();
        // Start is called before the first frame update
        void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        public void SetUpEquipmentPanel()
        {
            if (!mainContent.activeSelf)
            {
                mainContent.SetActive(true);
            }
            PopulateInventoryNode();
            equipmentBoxController.SetAllEquipmentData();
        }

        public void DeactiveEquipmentPanel()
        {
            ClearAllNode();
        }

        public void ClearAllNode()
        {
            if (listEquipment != null)
            {
                foreach (GameObject equipmentNode in listEquipment)
                {
                    Destroy(equipmentNode);
                }
                listEquipment.Clear();
            }
        }

        public void PopulateInventoryNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;
            foreach (SubType EquipmentType in EquipmentItems.Keys)
            {
                List<string> equipmentIDList = EquipmentItems[EquipmentType].UnlockIds;
                foreach (string itemId in equipmentIDList)
                {
                    GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                    EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                    ins_equipmentItem.SetDisplayItem(itemId);
                    ins_equipmentItem.SetThisItemID(itemId);
                    ins_equipmentItem.SetThisItemSubType(EquipmentType);
                    ins_equipmentItem.onClick.AddListener(() =>
                    {
                        ins_equipmentItem.UpdateUserData();
                        equipmentBoxController.SetAllEquipmentData();
                        ClearAllNode();
                        PopulateInventoryNode();
                    });
                    listEquipment.Add(ins_equipmentItem.gameObject);
                }
            }
            int listEquipmentAmount = listEquipment.Count;
            for (int BlankNode = 0; BlankNode < 48 - listEquipmentAmount; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }

        public void PopulateWeaponNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;

            List<string> equipmentIDList = EquipmentItems[SubType.Weapon].UnlockIds;
            foreach (string itemId in equipmentIDList)
            {
                GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                ins_equipmentItem.SetDisplayItem(itemId);
                ins_equipmentItem.SetThisItemID(itemId);
                ins_equipmentItem.SetThisItemSubType(SubType.Weapon);
                ins_equipmentItem.onClick.AddListener(() =>
                {
                    ins_equipmentItem.UpdateUserData();
                    equipmentBoxController.SetAllEquipmentData();
                    ClearAllNode();
                    PopulateInventoryNode();
                });
                listEquipment.Add(ins_equipmentItem.gameObject);
            }
            int listEquipmentAmount = listEquipment.Count;
            for (int BlankNode = 0; BlankNode < 48 - listEquipmentAmount; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }

        public void PopulateHelmetNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;

            List<string> equipmentIDList = EquipmentItems[SubType.Helmet].UnlockIds;
            foreach (string itemId in equipmentIDList)
            {
                GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                ins_equipmentItem.SetDisplayItem(itemId);
                ins_equipmentItem.SetThisItemID(itemId);
                ins_equipmentItem.SetThisItemSubType(SubType.Helmet);
                ins_equipmentItem.onClick.AddListener(() =>
                {
                    ins_equipmentItem.UpdateUserData();
                    equipmentBoxController.SetAllEquipmentData();
                    ClearAllNode();
                    PopulateInventoryNode();
                });
                listEquipment.Add(ins_equipmentItem.gameObject);
            }
            int listEquipmentAmount = listEquipment.Count;
            for (int BlankNode = 0; BlankNode < 48 - listEquipmentAmount; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }

        public void PopulateShirtNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;

            List<string> equipmentIDList = EquipmentItems[SubType.Shirt].UnlockIds;
            foreach (string itemId in equipmentIDList)
            {
                GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                ins_equipmentItem.SetDisplayItem(itemId);
                ins_equipmentItem.SetThisItemID(itemId);
                ins_equipmentItem.SetThisItemSubType(SubType.Shirt);
                ins_equipmentItem.onClick.AddListener(() =>
                {
                    ins_equipmentItem.UpdateUserData();
                    equipmentBoxController.SetAllEquipmentData();
                    ClearAllNode();
                    PopulateInventoryNode();
                });
                listEquipment.Add(ins_equipmentItem.gameObject);
            }
            int listEquipmentAmount = listEquipment.Count;
            for (int BlankNode = 0; BlankNode < 48 - listEquipmentAmount; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }

        public void PopulateGloveNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;

            List<string> equipmentIDList = EquipmentItems[SubType.Glove].UnlockIds;
            foreach (string itemId in equipmentIDList)
            {
                GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                ins_equipmentItem.SetDisplayItem(itemId);
                ins_equipmentItem.SetThisItemID(itemId);
                ins_equipmentItem.SetThisItemSubType(SubType.Glove);
                ins_equipmentItem.onClick.AddListener(() =>
                {
                    ins_equipmentItem.UpdateUserData();
                    equipmentBoxController.SetAllEquipmentData();
                    ClearAllNode();
                    PopulateInventoryNode();
                });
                listEquipment.Add(ins_equipmentItem.gameObject);
            }
            int listEquipmentAmount = listEquipment.Count;
            for (int BlankNode = 0; BlankNode < 48 - listEquipmentAmount; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }

        public void PopulatePantNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;

            List<string> equipmentIDList = EquipmentItems[SubType.Pant].UnlockIds;
            foreach (string itemId in equipmentIDList)
            {
                GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                ins_equipmentItem.SetDisplayItem(itemId);
                ins_equipmentItem.SetThisItemID(itemId);
                ins_equipmentItem.SetThisItemSubType(SubType.Pant);
                ins_equipmentItem.onClick.AddListener(() =>
                {
                    ins_equipmentItem.UpdateUserData();
                    equipmentBoxController.SetAllEquipmentData();
                    ClearAllNode();
                    PopulateInventoryNode();
                });
                listEquipment.Add(ins_equipmentItem.gameObject);
            }
            int listEquipmentAmount = listEquipment.Count;
            for (int BlankNode = 0; BlankNode < 48 - listEquipmentAmount; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }

        public void PopulateShoesNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;

            List<string> equipmentIDList = EquipmentItems[SubType.Shoes].UnlockIds;
            foreach (string itemId in equipmentIDList)
            {
                GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                ins_equipmentItem.SetDisplayItem(itemId);
                ins_equipmentItem.SetThisItemID(itemId);
                ins_equipmentItem.SetThisItemSubType(SubType.Shoes);
                ins_equipmentItem.onClick.AddListener(() =>
                {
                    ins_equipmentItem.UpdateUserData();
                    equipmentBoxController.SetAllEquipmentData();
                    ClearAllNode();
                    PopulateInventoryNode();
                });
                listEquipment.Add(ins_equipmentItem.gameObject);
            }
            int listEquipmentAmount = listEquipment.Count;
            for (int BlankNode = 0; BlankNode < 48 - listEquipmentAmount; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }
    }
}

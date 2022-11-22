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
                PopulateInventoryNode();
                equipmentBoxController.SetAllEquipmentData();
            }
        }

        public void DeactiveEquipmentPanel()
        {
            mainContent.SetActive(false);
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
            
            foreach (string itemId in EquipmentItems.Keys)
            {
                GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                ins_equipmentItem.SetDisplayItem(itemId);
                ins_equipmentItem.SetThisItemID(itemId);
                ins_equipmentItem.onClick.AddListener(() =>
                {
                    ins_equipmentItem.UpdateUserData();
                    equipmentBoxController.SetAllEquipmentData();
                });
                listEquipment.Add(ins_equipmentItem.gameObject);
            }
            for (int BlankNode = 0; BlankNode < 48 - EquipmentItems.Keys.Count; BlankNode++)
            {
                listEquipment.Add(Instantiate(blankItemNodePrefab, inventoryBoxContentTransform));
            }
        }

        public void PopulateWeaponNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;

            foreach (string itemId in EquipmentItems.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(itemId, ItemType.Equipable, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + itemId);
                    return;
                }
                if (itemInfo.SubType == SubType.Weapon)
                {
                    GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                    EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                    ins_equipmentItem.SetDisplayItem(itemId);
                    ins_equipmentItem.SetThisItemID(itemId);
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

        public void PopulateHelmetNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;
            
            foreach (string itemId in EquipmentItems.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(itemId, ItemType.Equipable, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + itemId);
                    return;
                }
                if (itemInfo.SubType == SubType.Helmet)
                {
                    GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                    EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                    ins_equipmentItem.SetDisplayItem(itemId);
                    ins_equipmentItem.SetThisItemID(itemId);
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

        public void PopulateShirtNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;
            
            foreach (string itemId in EquipmentItems.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(itemId, ItemType.Equipable, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + itemId);
                    return;
                }
                if (itemInfo.SubType == SubType.Shirt)
                {
                    GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                    EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                    ins_equipmentItem.SetDisplayItem(itemId);
                    ins_equipmentItem.SetThisItemID(itemId);
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

        public void PopulateGloveNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;
            
            foreach (string itemId in EquipmentItems.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(itemId, ItemType.Equipable, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + itemId);
                    return;
                }
                if (itemInfo.SubType == SubType.Glove)
                {
                    GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                    EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                    ins_equipmentItem.SetDisplayItem(itemId);
                    ins_equipmentItem.SetThisItemID(itemId);
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

        public void PopulatePantNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;
            
            foreach (string itemId in EquipmentItems.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(itemId, ItemType.Equipable, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + itemId);
                    return;
                }
                if (itemInfo.SubType == SubType.Pant)
                {
                    GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                    EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                    ins_equipmentItem.SetDisplayItem(itemId);
                    ins_equipmentItem.SetThisItemID(itemId);
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

        public void PopulateShoesNode()
        {
            var EquipmentItems = userDataManager.UserData.UserDataInventory.EquipableItems;
            
            foreach (string itemId in EquipmentItems.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(itemId, ItemType.Equipable, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + itemId);
                    return;
                }
                if (itemInfo.SubType == SubType.Shoes)
                {
                    GameObject insInventoryNode = Instantiate(equipmentItemButtonPrefab.gameObject, inventoryBoxContentTransform);
                    EquipmentItemButton ins_equipmentItem = insInventoryNode.GetComponent<EquipmentItemButton>();
                    ins_equipmentItem.SetDisplayItem(itemId);
                    ins_equipmentItem.SetThisItemID(itemId);
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
    }
}

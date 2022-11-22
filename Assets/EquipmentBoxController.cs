using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class EquipmentBoxController : MonoBehaviour
    {
        [SerializeField]
        private List<EquipmentButton> listEquipmentButton;

        GameDataManager gameDataManager;

        UserDataManager userDataManager;

        [SerializeField]
        private EquipmentPanelController equipmentPanelController;

        void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }
        void Start()
        {
            SetUpEquipmentButton();
        }

        public void SetAllEquipmentData()
        {
            var equipmentItemData = userDataManager.UserData.UserDataInventory.EquipableItems;
            foreach (string equipmentID in equipmentItemData.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(equipmentID, ItemType.Equipable, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + equipmentID);
                    return;
                }
                if (equipmentItemData[equipmentID])
                {
                    switch (itemInfo.SubType)
                    {
                        case SubType.Weapon:
                            {
                                SetWeaponEquipment(equipmentID);
                                break;
                            }
                        case SubType.Helmet:
                            {
                                SetHelmetEquipment(equipmentID);
                                break;
                            }
                        case SubType.Shirt:
                            {
                                SetShirtEquipment(equipmentID);
                                break;
                            }
                        case SubType.Glove:
                            {
                                SetGloveEquipment(equipmentID);
                                break;
                            }
                        case SubType.Pant:
                            {
                                SetPantEquipment(equipmentID);
                                break;
                            }
                        case SubType.Shoes:
                            {
                                SetShoesEquipment(equipmentID);
                                break;
                            }
                    }
                }
            }
        }

        public void SetUpEquipmentButton()
        {
            listEquipmentButton[0].onClick.AddListener(() =>
            {
                equipmentPanelController.ClearAllNode();
                equipmentPanelController.PopulateWeaponNode();
            });
            listEquipmentButton[1].onClick.AddListener(() =>
            {
                equipmentPanelController.ClearAllNode();
                equipmentPanelController.PopulateHelmetNode();
            });
            listEquipmentButton[2].onClick.AddListener(() =>
            {
                equipmentPanelController.ClearAllNode();
                equipmentPanelController.PopulateShirtNode();
            });
            listEquipmentButton[3].onClick.AddListener(() =>
            {
                equipmentPanelController.ClearAllNode();
                equipmentPanelController.PopulateGloveNode();
            });
            listEquipmentButton[4].onClick.AddListener(() =>
            {
                equipmentPanelController.ClearAllNode();
                equipmentPanelController.PopulatePantNode();
            });
            listEquipmentButton[5].onClick.AddListener(() =>
            {
                equipmentPanelController.ClearAllNode();
                equipmentPanelController.PopulateShoesNode();
            });
        }

        public void SetWeaponEquipment(string equipmentID)
        {
            listEquipmentButton[0].SetDisplayItem(equipmentID);
        }

        public void SetHelmetEquipment(string equipmentID)
        {
            listEquipmentButton[1].SetDisplayItem(equipmentID);
        }

        public void SetShirtEquipment(string equipmentID)
        {
            listEquipmentButton[2].SetDisplayItem(equipmentID);
        }

        public void SetGloveEquipment(string equipmentID)
        {
            listEquipmentButton[3].SetDisplayItem(equipmentID);
        }

        public void SetPantEquipment(string equipmentID)
        {
            listEquipmentButton[4].SetDisplayItem(equipmentID);
        }

        public void SetShoesEquipment(string equipmentID)
        {
            listEquipmentButton[5].SetDisplayItem(equipmentID);
        }
    }
}

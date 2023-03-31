using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using System;

namespace ArmasCreator.UI
{
    public class ItemPreviewCameraController : MonoBehaviour
    {
        public enum equipmentTypes
        {
            gunblade, 
            helmet, 
            shirt, 
            glove, 
            pant, 
            shoes
        }

        [Serializable]
        public struct previewItem
        {
            public equipmentTypes equipmentType;
            public GameObject previewItemPrefab;
        }

        [Header("Tiger")]
        [SerializeField]
        private List<previewItem> tigerPreviewItemPrefabList;
        [Header("Shrimp")]
        [SerializeField]
        private List<previewItem> shrimpPreviewItemPrefabList;

        [SerializeField]
        private Transform rotator;

        private GameObject currentItemPreview;
        public void OnChangeItemPreview(SubType currentSubType,string currentEquipmentType)
        {
            if(currentItemPreview != null)
            {
                Destroy(currentItemPreview);
            }
            equipmentTypes equipmentType = Enum.Parse<equipmentTypes>(currentEquipmentType);
            currentItemPreview = Instantiate(GetPreviewItemPrefabByType(currentSubType, equipmentType), rotator);
        }

        public GameObject GetPreviewItemPrefabByType(SubType currentSubType,equipmentTypes equipmentType)
        {
            if(SubType.Lion == currentSubType)
            {
                foreach(previewItem previewItem in tigerPreviewItemPrefabList)
                {
                    if(previewItem.equipmentType == equipmentType)
                    {
                        return previewItem.previewItemPrefab;
                    }
                }
            }

            if(SubType.Shrimp == currentSubType)
            {
                foreach (previewItem previewItem in shrimpPreviewItemPrefabList)
                {
                    if (previewItem.equipmentType == equipmentType)
                    {
                        return previewItem.previewItemPrefab;
                    }
                }
            }

            return null;
        }
    }
}

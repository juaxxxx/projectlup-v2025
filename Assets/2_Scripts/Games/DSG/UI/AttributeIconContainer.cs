using LUP.DSG.Utils.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    [Serializable]
    public struct AttributeTypeImage
    {
        [SerializeField]
        public EAttributeType attributeType;
        [SerializeField]
        public Sprite typeIcon;
        [SerializeField]
        public UnityEngine.Color typeColor;
        public AttributeTypeImage(EAttributeType type, Sprite icon, UnityEngine.Color color)
        {
            attributeType = type;
            typeIcon = icon;
            typeColor = color;
        }
    }

    public class AttributeIconContainer : MonoBehaviour
    {
        [SerializeField]
        private List<AttributeTypeImage> typeList = new List<AttributeTypeImage>();

        [SerializeField]
        public Dictionary<EAttributeType, AttributeTypeImage> attributeIconDictionary = new Dictionary<EAttributeType, AttributeTypeImage>();

        private void Awake()
        {
            foreach(AttributeTypeImage type in typeList)
            {
                attributeIconDictionary[type.attributeType] = type;
            }
        }

        public AttributeTypeImage GetTypeByAttributeImage(EAttributeType type)
        {
            return attributeIconDictionary[type];
        }
    }
}
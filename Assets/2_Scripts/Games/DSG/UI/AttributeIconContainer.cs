using LUP.DSG.Utils.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class AttributeIconContainer : MonoBehaviour
    {
        [SerializeField]
        private AttributeData attributeData;

        [SerializeField]
        public Dictionary<EAttributeType, AttributeTypeImage> attributeIconDictionary = new Dictionary<EAttributeType, AttributeTypeImage>();

        private void Awake()
        {
            foreach(AttributeTypeImage type in attributeData.attributeList)
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
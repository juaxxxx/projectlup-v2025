using LUP.DSG.Utils.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    [Serializable]
    public struct AttributeTypeImage
    {
        public EAttributeType attributeType;
        public Sprite typeIcon;
        public UnityEngine.Color typeColor;
        public AttributeTypeImage(EAttributeType type, Sprite icon, UnityEngine.Color color)
        {
            attributeType = type;
            typeIcon = icon;
            typeColor = color;
        }
    }

    [CreateAssetMenu(fileName = "Attribute Data", menuName = "DSG/Attribute Data", order = int.MaxValue)]
    public class AttributeData : ScriptableObject
    {
        public List<AttributeTypeImage> attributeList = new List<AttributeTypeImage>();
    }
}
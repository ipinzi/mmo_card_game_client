using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Items
{
    public class Item : ScriptableObject
    {
        [ReadOnly]
        [TableColumnWidth(10, true)]
        public string id = Guid.NewGuid().ToString();
        [TableColumnWidth(100, true), OnValueChanged(nameof(RenameAsset))]
        public string itemName;
        [TextArea]
        [TableColumnWidth(200, true)]
        public string itemText = "";
        [PreviewField]
        [TableColumnWidth(20, true)]
        public Sprite itemImage;
        [ReadOnly]
        [TableColumnWidth(30, true)]
        public string itemType;
        
        private void RenameAsset()
        {
            #if UNITY_EDITOR
                var path = AssetDatabase.GetAssetPath(this);
                AssetDatabase.RenameAsset(path, itemName);
            #endif
        }
    }
    
    [Serializable]
    public class ItemSerializableData
    {
        public string id;
        public string itemName;
        public string itemText = "";
        public string itemType = "";
        
        public ItemSerializableData(Item item)
        {
            id = item.id;
            itemName = item.itemName;
            itemText = item.itemText;
            itemType = item.itemType;
        }
    }
}
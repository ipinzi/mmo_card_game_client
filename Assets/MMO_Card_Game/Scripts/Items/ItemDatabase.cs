using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Items
{
    [CreateAssetMenu(menuName = "MMO Card Game/Item Database", fileName = "Item Database", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        [InfoBox("This is the Item Database, all items available in game should appear here. " +
                 "If for some reason an item is not available in this list then you can add it from the [Items] " +
                 "dropdown in explorer view on the left, otherwise it will not appear in the game or export in JSON.")]
        [Searchable, TableList(DrawScrollView = true, MinScrollViewHeight = 200),OnValueChanged(nameof(CheckDuplicates))]
        public List<Item> items;

        public Item GetItem(string id)
        {
            return items.FirstOrDefault(item => item.id == id);
        }
        public Item GetItemByName(string itemName)
        {
            return items.FirstOrDefault(item => item.itemName == itemName);
        }
        private void CheckDuplicates()
        {
            items = items.Distinct().ToList();
        }

        #if UNITY_EDITOR
            [Button("Export to JSON")]
            private void ExportToJson()
            {
                var path = EditorUtility.SaveFilePanel(
                    "Export Item Database Data as JSON",
                    "",
                    "itemDatabase.json",
                    "json");

                if (path.Length != 0)
                {
                    var serializableData = ConvertToSeralizableData();
                    var json = JsonUtility.ToJson(serializableData, true);
                    File.WriteAllText(path, json);
                    
                    Debug.Log("Item Database Json saved.");
                }
            }
        #endif
        
        private ItemDatabaseSerializableData ConvertToSeralizableData()
        {
            var itemsJsonData = new ItemDatabaseSerializableData();
            foreach (var item in items)
            {
                var newItem = new ItemSerializableData(item);
                itemsJsonData.items.Add(newItem);
            }

            return itemsJsonData;
        }
    }
    [System.Serializable]
    public class ItemDatabaseSerializableData
    {
        public List<ItemSerializableData> items;

        public ItemDatabaseSerializableData()
        {
            items = new List<ItemSerializableData>();
        }
    }
}
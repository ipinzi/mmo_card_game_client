using System.Collections.Generic;
using System.Linq;
using MMO_Card_Game.Editor;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.Items;
using MMO_Card_Game.Scripts.Quests;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MmoCardGameEditor : OdinMenuEditorWindow
    {
        private object _selectedAsset;
        private string _nameString;
        private CardDatabase _cardDatabase;
        private ItemDatabase _itemDatabase;
        private QuestDatabase _questDatabase;
        
        [MenuItem("MMO Card Game/MMO Card Game Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<MmoCardGameEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 1000);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            var style = GetMenuStyle();
            tree.DefaultMenuStyle = style;
            
            tree.AddAssetAtPath("Network Settings", "Assets/MMO_Card_Game/Data/Networking/Network Settings.asset",typeof(ScriptableObject));

            _cardDatabase = (CardDatabase)AssetDatabase.LoadAssetAtPath("Assets/MMO_Card_Game/Data/Cards/Card Database.asset",
                typeof(CardDatabase));
            _itemDatabase = (ItemDatabase)AssetDatabase.LoadAssetAtPath("Assets/MMO_Card_Game/Data/Items/Item Database.asset",
                typeof(ItemDatabase));
            _questDatabase = (QuestDatabase)AssetDatabase.LoadAssetAtPath("Assets/MMO_Card_Game/Data/Quests/Quest Database.asset",
                typeof(QuestDatabase));
            
            tree.AddAssetAtPath("Cards", "Assets/MMO_Card_Game/Data/Cards/Card Database.asset",typeof(ScriptableObject));
            tree.AddAllAssetsAtPath("Cards", "Assets/MMO_Card_Game/Data/Cards/", typeof(Card), true)
                .ForEach(this.AddDragHandles);
            
            tree.EnumerateTree().AddIcons<Card>(x => x.cardSprite);
            
            tree.Add("Cards/Summons", new SummonMmoEditorPane(_cardDatabase.cards));
            tree.Add("Cards/Equipment", new EquipmentMmoEditorPane(_cardDatabase.cards));
            tree.Add("Cards/Items", new ItemMmoEditorPane(_cardDatabase.cards));
            
            
            tree.AddAssetAtPath("Items", "Assets/MMO_Card_Game/Data/Items/Item Database.asset",typeof(ScriptableObject));
            tree.AddAllAssetsAtPath("Items", "Assets/MMO_Card_Game/Data/Items/Items", typeof(Item), true)
                .ForEach(this.AddDragHandles);
            
            tree.AddAssetAtPath("Quests", "Assets/MMO_Card_Game/Data/Quests/Quest Database.asset",typeof(ScriptableObject));
            
            tree.Add("Quests/Tasks", new TasksPane());
            tree.AddAllAssetsAtPath("Quests/Tasks", "Assets/MMO_Card_Game/Data/Quests/Tasks", typeof(QuestTask), true)
                .ForEach(this.AddDragHandles);
            
            tree.AddAssetAtPath("Quests/Quests", "Assets/MMO_Card_Game/Data/Quests/Quest Database.asset",typeof(ScriptableObject));
            tree.AddAllAssetsAtPath("Quests/Quests", "Assets/MMO_Card_Game/Data/Quests/Quests", typeof(Quest), true)
                .ForEach(this.AddDragHandles);
            
            return tree;
        }
        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }


        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();

            DrawCustomEditors();
        }

        private void DrawCustomEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }
                GUILayout.FlexibleSpace();
                
                if(selected.Name.Contains("Cards")){
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("New Card")))
                    {
                        ScriptableObjectCreator.ShowDialog<Card>("Assets/MMO_Card_Game/Data/Cards", obj =>
                        {
                            _cardDatabase.cards.Add(obj);
                            base.TrySelectMenuItemWithObject(obj);
                        });
                    }
                }
                if(selected.Name.Contains("Items")){
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("New Item")))
                    {
                        ScriptableObjectCreator.ShowDialog<Item>("Assets/MMO_Card_Game/Data/Items/Items", obj =>
                        {
                            _itemDatabase.items.Add(obj);
                            base.TrySelectMenuItemWithObject(obj);
                        });
                    }
                }
                if(selected.Name.Contains("Quests")){
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("New Quest")))
                    {
                        ScriptableObjectCreator.ShowDialog<Quest>("Assets/MMO_Card_Game/Data/Quests/Quests", obj =>
                        {
                            _questDatabase.quests.Add(obj);
                            base.TrySelectMenuItemWithObject(obj);
                        });
                    }
                }
                if(selected.Name.Contains("Quests")){
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("New Task")))
                    {
                        ScriptableObjectCreator.ShowDialog<QuestTask>("Assets/MMO_Card_Game/Data/Quests/Tasks", obj =>
                        {
                            base.TrySelectMenuItemWithObject(obj);
                        });
                    }
                }

                if (selected.Value == null) return;

                if (selected.Value.GetType() == typeof(SummonCard) || selected.Value.GetType() == typeof(ItemCard)
                || selected.Value.GetType() == typeof(EquipmentCard))
                {
                    if (SirenixEditorGUI.ToolbarButton("Delete"))
                    {
                        var asset = selected.Value as Object;
                        if (!asset)
                        {
                            Debug.Log("No asset selected.");
                            return;
                        }

                        var path = AssetDatabase.GetAssetPath(asset);

                        if (EditorUtility.DisplayDialog("Delete " + asset.name + "?",
                            "Are you sure you want to delete " + asset.name + "?", "Delete", "Cancel"))
                        {
                            Undo.RecordObject(asset, asset.name + " deleted.");
                            
                            var card = (Card)AssetDatabase.LoadAssetAtPath(path, typeof(Card));
                            _cardDatabase.cards.Remove(card);
                            
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.SaveAssets();
                        }
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private OdinMenuStyle GetMenuStyle()
        {
            return new OdinMenuStyle()
            {
                Height = 30,
                Offset = 28.00f,
                IndentAmount = 10.00f,
                IconSize = 16.00f,
                IconOffset = 0.00f,
                NotSelectedIconAlpha = 0.85f,
                IconPadding = 3.00f,
                TriangleSize = 17.00f,
                TrianglePadding = 8.00f,
                AlignTriangleLeft = false,
                Borders = true,
                BorderPadding = 13.00f,
                BorderAlpha = 0.50f,
                SelectedColorDarkSkin = new Color(.36f, .85f, .29f, 1),
                SelectedColorLightSkin = new Color(.36f, .85f, .29f, 1)
            };
        }
    }
}
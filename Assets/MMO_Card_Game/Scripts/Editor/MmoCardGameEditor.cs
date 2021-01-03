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
        
        [MenuItem("MMO Card Game/MMO Card Game Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<MmoCardGameEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 800);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            var style = GetMenuStyle();
            tree.DefaultMenuStyle = style;
            
            var networkSettingsPane = tree.AddAssetAtPath("Network Settings", "Assets/MMO_Card_Game/Data/Networking/Network Settings.asset",typeof(ScriptableObject));
            
            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();

            //DrawCustomEditors();
        }

        private void DrawCustomEditors()
        {
            var selected = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                if (SirenixEditorGUI.ToolbarButton("Open in Project"))
                {
                    var asset = selected.SelectedValue as Object;
                    var path = AssetDatabase.GetAssetPath(asset);
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
                }

                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("Delete"))
                {
                    var asset = selected.SelectedValue as Object;
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
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
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
                Offset = 16.00f,
                IndentAmount = 15.00f,
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
                SelectedColorDarkSkin = new Color(0.708f, 0.210f, 0.642f, 1.000f),
                SelectedColorLightSkin = new Color(0.755f, 0.231f, 0.682f, 1.000f)
            };
        }
    }
}
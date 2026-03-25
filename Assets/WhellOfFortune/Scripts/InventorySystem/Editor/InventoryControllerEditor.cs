using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WhellOfFortune.Scripts.InventorySystem.Editor
{
    [CustomEditor(typeof(InventoryController))]
    public class InventoryControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            InventoryController controller = (InventoryController)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Fetch All Inventory Items"))
            {
                string[] guids = AssetDatabase.FindAssets("t:BaseInventoryItemData");

                controller.itemDatas = guids
                    .Select(guid => AssetDatabase.LoadAssetAtPath<BaseInventoryItemData>(AssetDatabase.GUIDToAssetPath(guid)))
                    .ToList();

                EditorUtility.SetDirty(controller);

                Debug.Log($"Toplam {controller.itemDatas.Count} item eklendi.");
            }
        }
    }
}
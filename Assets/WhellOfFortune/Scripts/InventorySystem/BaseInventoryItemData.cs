using UnityEngine;

namespace WhellOfFortune.Scripts.InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Game/Inventory/InventoryData", order = 1)]

    public class BaseInventoryItemData : BaseScriptableData<BaseInventorItemSaveDataModel>
    {
        public Sprite icon;
        public InventoryItemTypes itemType;
        public bool isConsumable;
    }
    
    
    [System.Serializable]
    public class BaseInventorItemSaveDataModel : BaseDataModel<BaseInventorItemSaveDataModel>
    {
        public int count;
    }
}
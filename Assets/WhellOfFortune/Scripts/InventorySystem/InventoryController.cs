using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using WhellOfFortune.Scripts.CurrencySystem;
using WhellOfFortune.Scripts.ManagerSystem;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.WheelRewards;

namespace WhellOfFortune.Scripts.InventorySystem
{
    public class InventoryController : BaseManager
    {
        public List<BaseInventoryItemData> itemDatas =  new List<BaseInventoryItemData>();
        private InventoryUIController _inventoryUIController;

        public override void LoadManager(Action onLoadEnd)
        {
            _inventoryUIController =
                _gameManager.GetManager<GeneralUIManager>().GetUIController<InventoryUIController>();
            onLoadEnd?.Invoke();
        }

        public override void InitManager()
        {
            _inventoryUIController.LoadInventory(itemDatas);
        }

    }
}
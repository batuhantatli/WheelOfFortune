using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
            foreach (var data in itemDatas)
            {
                data.Initialize();
            }
            
            _inventoryUIController =
                _gameManager.GetManager<GeneralUIManager>().GetUIController<InventoryUIController>();
            
            _inventoryUIController.LoadInventory(itemDatas);
            
            
            
            onLoadEnd?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                itemDatas[5].UserData.count += 5;
                itemDatas[5].SaveUserData();
            }
        }

        public override void InitManager()
        {

        }

    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WhellOfFortune.Scripts.CurrencySystem;
using WhellOfFortune.Scripts.RewardSystem;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.SpinSystem;
using WhellOfFortune.Scripts.WheelRewards;

namespace WhellOfFortune.Scripts.InventorySystem
{
    public class InventoryUIController : BaseUIController
    {
        [SerializeField] private Transform itemContainer;
        [SerializeField] private InventoryUIItem itemPrefab;
        [SerializeField] private Button spinStartButton;
        public List<InventoryUIItem> currentItems = new List<InventoryUIItem>();
        private SpinUIController  _spinUIController;
        private CurrencyManager  _currencyManager;
        private InventoryController _inventoryController;
        public override void InitializeUIController(GeneralUIManager uiManager)
        {
            _currencyManager = GameManager.Instance.GetManager<CurrencyManager>();
            _inventoryController = GameManager.Instance.GetManager<InventoryController>();
            _spinUIController = uiManager.GetUIController<SpinUIController>();
            spinStartButton.onClick.AddListener((() =>
            {
                _spinUIController.TryOpenPanel(null);
                ClosePanel();
            }));
            base.InitializeUIController(uiManager);
        }

        protected override void OpenPanel()
        {
            base.OpenPanel();
        }
        
        public void AddCollectedRewards(CollectedItemUI collectedItemUI)
        {
            BaseSpinRewardData rewardData = collectedItemUI.RewardData;
            if (rewardData is InventoryItemSpinRewardData spinRewardData1 &&
                IsItemHave(spinRewardData1.inventoryItemData.itemType))
            {
                var inventoryItem = GetItem(spinRewardData1.inventoryItemData.itemType);
                inventoryItem.AddItem(collectedItemUI.ItemCount);
            }
            else if (rewardData is InventoryItemSpinRewardData spinRewardData2)
            {
                InventoryUIItem inventoryItem = Instantiate(itemPrefab, itemContainer.transform);
                inventoryItem.Initialize(_inventoryController, spinRewardData2.inventoryItemData,
                    collectedItemUI.ItemCount);
            }
            else if (rewardData is CurrencySpinRewardData currencyRewardData)
            {
                _currencyManager.AddCurrency(currencyRewardData.currencyData.currencyType,
                    collectedItemUI.ItemCount);
            }
        }

        public void LoadInventory(List<BaseInventoryItemData>  inventoryItems)
        {
            foreach (var inventoryItem in inventoryItems)
            {
                if (inventoryItem.isConsumable)
                {
                    if (inventoryItem.UserData.count <= 0) return;
                    InventoryUIItem spawnedItem = Instantiate(itemPrefab, itemContainer.transform);
                    spawnedItem.Initialize(_inventoryController, inventoryItem, inventoryItem.UserData.count);
                }
                else
                {
                    InventoryUIItem spawnedItem = Instantiate(itemPrefab, itemContainer.transform);
                    spawnedItem.Initialize(_inventoryController, inventoryItem, inventoryItem.UserData.count);
                }
            }
        }


        public bool IsItemHave(InventoryItemTypes targetItemType)
        {
            return currentItems.Any(t => t.InventoryData.itemType == targetItemType);
        }

        public InventoryUIItem GetItem(InventoryItemTypes targetItemType)
        {
            return currentItems.FirstOrDefault(t => t.InventoryData.itemType == targetItemType);
        }
    }
}

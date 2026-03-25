
using TMPro;
using UnityEngine.UI;
using WhellOfFortune.Scripts.CurrencySystem;
using WhellOfFortune.Scripts.InventorySystem;

namespace WhellOfFortune.Scripts.SpinSystem
{
    public class DeathUIController : BaseUIController
    {
        public Button giveUp;
        public Button reviveGold;
        public TMP_Text reviveGoldText;
        public Button reviveAds;

        public int reviveGoldPrice;
        private CurrencyManager _currencyManager;
        private InventoryUIController _inventoryUIController;
        private SpinUIController _spinUIController;
        public override void InitializeUIController(GeneralUIManager uiManager)
        {
            _currencyManager = GameManager.Instance.GetManager<CurrencyManager>();
            _spinUIController = uiManager.GetUIController<SpinUIController>();
            _inventoryUIController = uiManager.GetUIController<InventoryUIController>();
            reviveGold.onClick.AddListener((() => Revive(false)));
            reviveAds.onClick.AddListener((() => Revive(true)));
            giveUp.onClick.AddListener(ReturnInventory);
            base.InitializeUIController(uiManager);
        }

        protected override void OpenPanel()
        {
            UpdateUI();
            base.OpenPanel();
            
        }

        public override void ClosePanel()
        {
            _spinUIController._isCanExit = true;
            _spinUIController._isSpinning = false;
            base.ClosePanel();
        }

        private void UpdateUI()
        {
            reviveAds.interactable = true; //IsAdsLoaded()
            
            SetReviveGoldText();
            
            SetGoldReviveButton();
        }

        private void ReturnInventory()
        {
            _spinUIController.ClosePanel();
            _inventoryUIController.TryOpenPanel(null);
            ClosePanel();
        }

        private void SetReviveGoldText()
        {
            reviveGoldText.text = reviveGoldPrice.ToString();
        }

        private void SetGoldReviveButton()
        {
            reviveGold.interactable = _currencyManager.IsAffordable(CurrencyType.Money,reviveGoldPrice);
        }

        private void Revive(bool isAds)
        {
            if (isAds)
            {
                ClosePanel();
            }

            if (_currencyManager.IsAffordable(CurrencyType.Money,reviveGoldPrice) && !isAds)
            {
                ClosePanel();
                _currencyManager.SpendCurrency(CurrencyType.Gold, reviveGoldPrice);
            }
        }

        private bool IsAdsLoaded()
        {
            return false;
        }
    }
}


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

        private void UpdateUI()
        {
            if (IsAdsLoaded())
            {
                reviveAds.interactable = false;
            }
            
            SetReviveGoldText();
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

        private void Revive(bool isAds)
        {
            if (isAds)
            {
                // close panel
                //revive
            }

            if (_currencyManager.IsAffordable(CurrencyType.Money,reviveGoldPrice))
            {
                //close panel
                //revive
            }
        }

        private bool IsAdsLoaded()
        {
            return false;
        }
    }
}

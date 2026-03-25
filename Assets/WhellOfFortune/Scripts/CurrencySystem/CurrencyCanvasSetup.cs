using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WhellOfFortune.Scripts.CurrencySystem
{
    public class CurrencyCanvasSetup : BaseUIController
    {
        [SerializeField] private List<CurrencyUIControl> currencyUIList;
        
        #region INIT

        public void InitCurrencyUIs(CurrencyManager currencyManager)
        {
            foreach (CurrencyUIControl cUI in currencyUIList)
            {
                cUI.InitCurrencyUI(currencyManager);
            }
        }

        #endregion
        
        #region PRIVATE METHODS

        private CurrencyUIControl GetCurrency(CurrencyType currencyType)
        {
            return currencyUIList.FirstOrDefault(t => t.CurrencyType == currencyType);
        }

        #endregion

        #region PUBLIC METHODS

        public void ActivateMoneyOutOfCurrencyAnimation(bool activate, CurrencyType currencyType)
        {
            CurrencyUIControl currencyPanel = GetCurrency(currencyType);
            if (activate)
            {
                currencyPanel.ActivateOutOfMoneyAnimation();
            }
            else
            {
                currencyPanel.DeActivateOutOfMoneyAnimation();
            }
        }

        #endregion
    }
}

using UnityEngine;
using WhellOfFortune.Scripts.CurrencySystem;
using WhellOfFortune.Scripts.InventorySystem;
using WhellOfFortune.Scripts.SpinSystem;

namespace WhellOfFortune.Scripts.RewardSystem.Rewards
{
    [CreateAssetMenu(fileName = "Currency Reward", menuName = "Game/WheelRewards/Currency Reward",order = 1)]

    public class CurrencySpinRewardData : BaseSpinRewardData
    {
        public CurrencyData currencyData;
        public override void Reward()
        {
            GameManager.Instance.GetManager<GeneralUIManager>().GetUIController<SpinUIController>().CollectedRewardController.CollectReward(this);
        }

        public override Sprite GetRewardImage()
        {
            return currencyData.currencyIcon;
        }

 
    }
}

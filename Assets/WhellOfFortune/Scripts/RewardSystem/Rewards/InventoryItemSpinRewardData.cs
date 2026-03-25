using UnityEngine;
using WhellOfFortune.Scripts.Case;
using WhellOfFortune.Scripts.InventorySystem;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.SpinSystem;

namespace WhellOfFortune.Scripts.WheelRewards
{
    [CreateAssetMenu(fileName = "Inventory Reward", menuName = "Game/WheelRewards/Inventory Reward",order = 1)]
    public class InventoryItemSpinRewardData : BaseSpinRewardData
    {
        public BaseInventoryItemData inventoryItemData;
        public override void Reward()
        {
            GameManager.Instance.GetManager<GeneralUIManager>().GetUIController<SpinUIController>().CollectedRewardController.CollectReward(this);
        }

        public override Sprite GetRewardImage()
        {
            return inventoryItemData.icon;
        }
    }
}

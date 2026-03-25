
using UnityEngine;
using WhellOfFortune.Scripts.InventorySystem;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.SpinSystem;

namespace WhellOfFortune.Scripts.WheelRewards
{
    [CreateAssetMenu(fileName = "Bomb Reward", menuName = "Game/WheelRewards/Bomb Reward",order = 1)]

    public class BombSpinRewardData : BaseSpinRewardData
    {
        public Sprite icon;

        public override void Reward()
        {
            GameManager.Instance.GetManager<GeneralUIManager>().GetUIController<DeathUIController>().TryOpenPanel(null);
        }

        public override Sprite GetRewardImage()
        {
            return icon;
        }

 
    }
}

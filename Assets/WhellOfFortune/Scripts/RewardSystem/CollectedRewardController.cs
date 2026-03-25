using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WhellOfFortune.Scripts.InventorySystem;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.WheelRewards;

namespace WhellOfFortune.Scripts.RewardSystem
{
    public class CollectedRewardController : MonoBehaviour
    {
        public List<CollectedItemUI>  collectedRewards = new List<CollectedItemUI>();
        public Transform collectedRewardContainer;
        public CollectedItemUI collectedItemUI;
        private InventoryUIController _inventoryUIController;
    
        public void Initialize(InventoryUIController inventoryUIController)
        {
            _inventoryUIController = inventoryUIController;
        }

        public void CollectReward(BaseSpinRewardData rewardData)
        {
            if (collectedRewards.Any(t=>t.RewardData == rewardData))
            {
                CollectedItemUI itemUI = collectedRewards.FirstOrDefault(t => t.RewardData == rewardData);
                itemUI.UpdateValue(rewardData.rewardValue);
            }
            else
            {
                CollectedItemUI item = Instantiate(collectedItemUI, collectedRewardContainer);
                item.Initialize(rewardData);
                collectedRewards.Add(item);
            }
        }

        public void ExitZone()
        {
            foreach (var collectedReward in collectedRewards)
            {
                _inventoryUIController.AddCollectedRewards(collectedReward);
            }
            ResetCollectedRewards();
            
        }

        public void ResetCollectedRewards()
        {
            foreach (var collectedReward in collectedRewards)
            {
                Destroy(collectedReward.gameObject);
            }
            collectedRewards.Clear();
        }
    
    }
}

using UnityEngine;

namespace WhellOfFortune.Scripts.RewardSystem.Rewards
{
    public abstract class BaseSpinRewardData : ScriptableObject
    {
        public Sprite RewardImage => GetRewardImage();
        public int rewardValue;
        public abstract void Reward();
        public abstract Sprite GetRewardImage();
    }
}

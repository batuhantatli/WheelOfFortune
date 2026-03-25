using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhellOfFortune.Scripts.RewardSystem.Rewards;

namespace WhellOfFortune.Scripts.RewardSystem
{
    public class CollectedItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text countText;

        private int _currentValue;
        private BaseSpinRewardData _rewardData;
        public BaseSpinRewardData RewardData => _rewardData;
        public int ItemCount => _currentValue;

        public void Initialize(BaseSpinRewardData rewardData)
        {
            _rewardData = rewardData;
            icon.sprite =rewardData.GetRewardImage();
            UpdateValue(rewardData.rewardValue);
        }
        
        public void UpdateValue(int value)
        {
            _currentValue += value;
            countText.AnimateNumber(this,_currentValue,1," ");
        }


    }
}

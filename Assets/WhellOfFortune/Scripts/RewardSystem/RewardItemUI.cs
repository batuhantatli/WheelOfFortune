using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.WheelRewards;

namespace WhellOfFortune.Scripts.RewardSystem
{
    public class RewardItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text rewardCountText;

        public void Initialize(BaseSpinRewardData targetData,Sprite sprite,int count)
        {
            SetRewardCount(count);
            icon.sprite = sprite;
            if (targetData is BombSpinRewardData)
            {
                rewardCountText.gameObject.SetActive(false);
            }
        }

        private void SetRewardCount(int count)
        {
            rewardCountText.text = "x" + count.ToShortString();
        }


        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace ArmasCreator.UI
{
    public class RewardNode : MonoBehaviour
    {
        [SerializeField]
        private Image rewardImage;
        [SerializeField]
        private TMP_Text rewardText;
        
        public void SetReward(Sprite newRewardSprite,string newRewardText)
        {
            rewardImage.sprite = newRewardSprite;
            rewardText.text = newRewardText;
        }
    }
}

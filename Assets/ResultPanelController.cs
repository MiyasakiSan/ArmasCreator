using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArmasCreator.Gameplay.UI
{
    public class ResultPanelController : MonoBehaviour
    {
        [Header("Image")]
        [SerializeField]
        private Image bannerImage;
        [SerializeField]
        private Image bossImage;

        [Header("Text")]
        [SerializeField]
        private TMP_Text bannerText;
        [SerializeField]
        private TMP_Text challengeText;

        [Header("RankText")]
        [SerializeField]
        private TMP_Text dealtRankText;
        [SerializeField]
        private TMP_Text takenRankText;
        [SerializeField]
        private TMP_Text itemRankText;
        [SerializeField]
        private TMP_Text finalRankText;

        [Header("TimeText")]
        [SerializeField]
        private TMP_Text completionTimeText;

        [Header("RewardText")]
        [SerializeField]
        private TMP_Text rewardText;

        private int hour;
        private int minute;
        private int second;

        private int rewardAmount;

        public bool IsUseSimulateTime = true;
        public bool IsSecondSimulateRun = true;
        public bool IsMinuteSimulateRun = true;
        public bool IsHourSimulateRun = true;

        public bool IsUseSimulateReward = true;

        // Start is called before the first frame update
        void Start()
        {
            SetCompletionTime(10, 30, 20);
            StartCoroutine(StopSecondSimulate());
        }

        // Update is called once per frame
        void Update()
        {
            if (IsUseSimulateTime)
            {
                if (IsSecondSimulateRun)
                {
                    completionTimeText.text = (Mathf.CeilToInt(Time.time * 30) % 99).ToString().PadLeft(2, '0') + ":" + (Mathf.CeilToInt(Time.time * 30) % 60).ToString().PadLeft(2, '0') + ":" + (Mathf.CeilToInt(Time.time * 30) % 60).ToString().PadLeft(2, '0') + "'";
                }
                else if (IsMinuteSimulateRun)
                {
                    completionTimeText.text = (Mathf.CeilToInt(Time.time * 30) % 99).ToString().PadLeft(2, '0') + ":" + (Mathf.CeilToInt(Time.time * 30) % 60).ToString().PadLeft(2, '0') + ":" + second + "'";
                }
                else if (IsHourSimulateRun)
                {
                    completionTimeText.text = (Mathf.CeilToInt(Time.time * 30) % 99).ToString().PadLeft(2, '0') + ":" + minute + ":" + second + "'";
                }
                else
                {
                    completionTimeText.text = hour + ":" + minute + ":" + second + "'";
                }
            }

            if(IsUseSimulateReward)
            {
                rewardText.text = (Mathf.CeilToInt(Time.time * 20000) % 9999).ToString().PadLeft(4, '0') + "s";
            }
        }

        IEnumerator StopSecondSimulate()
        {
            yield return new WaitForSeconds(5);
            IsSecondSimulateRun = false;
            StartCoroutine(StopMinuteSimulate());
        }

        IEnumerator StopMinuteSimulate()
        {
            yield return new WaitForSeconds(1);
            IsMinuteSimulateRun = false;
            StartCoroutine(StopHourSimulate());
        }

        IEnumerator StopHourSimulate()
        {
            yield return new WaitForSeconds(1);
            IsHourSimulateRun = false;
            IsUseSimulateTime = false;
        }

        public void SetBossImage(Sprite newBossImage, Sprite newBannerImage)
        {
            bannerImage.sprite = newBannerImage;
            bossImage.sprite = newBossImage;
        }

        public void SetChallengeText(string challengeName)
        {
            challengeText.text = challengeName;
        }

        public void SetRank(string newDealtRankText, string newTakenRankText, string newItemRankText, string newFinalRankText)
        {
            dealtRankText.text = newDealtRankText;
            takenRankText.text = newTakenRankText;
            itemRankText.text = newItemRankText;
            finalRankText.text = newFinalRankText;
        }

        public void SetCompletionTime(int newHour,int newMinute,int newSecond)
        {
            hour = newHour;
            minute = newMinute;
            second = newSecond;
        }

        public void SetRewardAmount(int newReward)
        {
            rewardAmount = newReward;
        }
    }
}

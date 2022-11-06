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
        [SerializeField]
        private TMP_Text damageDealtText;
        [SerializeField]
        private TMP_Text damageTakenText;
        [SerializeField]
        private TMP_Text itemUsedText;

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

        [Header("Button")]
        [SerializeField]
        private Button endMissionButton;

        private int hour;
        private int minute;
        private int second;

        private int rewardAmount;

        private int damageDealtAmount;
        private int damagetakenAmount;
        private int itemUsedAmount;


        private bool IsUseSimulateTime = true;
        private bool IsSecondSimulateRun = true;
        private bool IsMinuteSimulateRun = true;
        private bool IsHourSimulateRun = true;
        private bool IsUseSimulateReward = true;
        private bool IsUseSimulateDamageDealt = true;
        private bool IsUseSimulateDamageTaken = true;
        private bool IsUseSimulateItemUsed = true;

        // Start is called before the first frame update
        void Start()
        {
            endMissionButton.onClick.AddListener(() =>
            {
                OnEndMission();
            });

            //Test
            SetCompletionTime(10, 30, 20);
            StopSimulateTime();
            SetRewardAmount(2500);
            SetResultData(5000, 3000, 10);
            //Test
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
            else
            {
                rewardText.text = rewardAmount.ToString();
            }

            if(IsUseSimulateDamageDealt)
            {
                damageDealtText.text = (Mathf.CeilToInt(Time.time * 20000) % 9999).ToString().PadLeft(4, '0');
            }
            else
            {
                damageDealtText.text = damageDealtAmount.ToString();
            }

            if (IsUseSimulateDamageTaken)
            {
                damageTakenText.text = (Mathf.CeilToInt(Time.time * 20000) % 9999).ToString().PadLeft(4, '0');
            }
            else
            {
                damageTakenText.text = damagetakenAmount.ToString();
            }

            if(IsUseSimulateItemUsed)
            {
                itemUsedText.text = (Mathf.CeilToInt(Time.time * 200) % 99).ToString().PadLeft(2, '0');
            }
            else
            {
                itemUsedText.text = itemUsedAmount.ToString();
            }
        }

        #region Enumerator Stop Time

        IEnumerator StopSecondSimulate()
        {
            yield return new WaitForSeconds(1);
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

        #endregion

        #region Stop Function

        public void StopSimulateTime()
        {
            StartCoroutine(StopSecondSimulate());
        }

        public void StopSimulateReward()
        {
            IsUseSimulateReward = false;
        }

        public void StopSimulateDamageDealt()
        {
            IsUseSimulateDamageDealt = false;
        }

        public void StopSimulateDamageTaken()
        {
            IsUseSimulateDamageTaken = false;
        }

        public void StopSimulateItemUsed()
        {
            IsUseSimulateItemUsed = false;
        }

        #endregion

        public void SetBossImage(Sprite newBossImage, Sprite newBannerImage)
        {
            bannerImage.sprite = newBannerImage;
            bossImage.sprite = newBossImage;
        }

        public void SetChallengeText(string challengeName)
        {
            challengeText.text = challengeName;
        }

        public void SetBannerText(string bannerName)
        {
            bannerText.text = bannerName;
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

        public void SetResultData(int newDamageDealt, int newDamageTaken, int newItemUsed)
        {
            damageDealtAmount = newDamageDealt;
            damagetakenAmount = newDamageTaken;
            itemUsedAmount = newItemUsed;
        }

        public void OnEndMission()
        {

        }
    }
}

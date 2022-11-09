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

        private int minute;
        private int second;
        private int milliSecond;

        private int rewardAmount;

        private int damageDealtAmount;
        private int damagetakenAmount;
        private int itemUsedAmount;


        // Start is called before the first frame update
        void Start()
        {
            endMissionButton.onClick.AddListener(() =>
            {
                OnEndMission();
            });

            //Test
            SetCompletionTime(10, 30, 99);
            SetRewardAmount(2500);
            SetResultData(5000, 3000, 10);
            //Test
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(CountDamageDealt());
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(CountDamageTaken());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(CountItemUsed());
            }
            if(Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(CountReward());
            }
        }
        
        IEnumerator CountMilliSecondTime()
        {
            completionTimeText.text = "00" + ":" + "00" + ":" + "00" + "'";
            int countMilliSecondTime = 0;
            while (milliSecond > countMilliSecondTime)
            {
                countMilliSecondTime += 1;
                completionTimeText.text = "00" + ":" + "00" + ":" + countMilliSecondTime.ToString().PadRight(2,'0') + "'";
                yield return new WaitForSeconds(0.01f);
            }
            completionTimeText.text = "00" + ":" + "00" + ":" + milliSecond.ToString().PadLeft(2,'0') + "'";
        }

        IEnumerator CountSecondTime()
        {
            completionTimeText.text = "00" + ":" + "00" + ":" + milliSecond + "'";
            int countSecondTime = 0;
            while (second > countSecondTime)
            {
                countSecondTime += 1;
                completionTimeText.text = "00" + ":" + countSecondTime.ToString().PadRight(2, '0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
                yield return new WaitForSeconds(0.01f);
            }
            completionTimeText.text = "00" + ":" + second.ToString().PadLeft(2,'0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
        }

        IEnumerator CountMinuteTime()
        {
            completionTimeText.text = "00" + ":" + second + ":" + milliSecond + "'";
            int countMinuteTime = 0;
            while (minute > countMinuteTime)
            {
                countMinuteTime += 1;
                completionTimeText.text = countMinuteTime.ToString().PadLeft(2,'0') + ":" + second.ToString().PadRight(2, '0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
                yield return new WaitForSeconds(0.01f);
            }
            completionTimeText.text = minute.ToString().PadLeft(2, '0') + ":" + second.ToString().PadLeft(2, '0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
        }

        IEnumerator CountReward()
        {
            rewardText.text = "0 s";
            int countReward = 0;
            int increaseAmount = Mathf.CeilToInt(Mathf.Pow(10, Mathf.CeilToInt(Mathf.Log10(rewardAmount/25))));
            while (rewardAmount > countReward)
            {
                countReward += increaseAmount;
                rewardText.text = countReward.ToString() + " s";
                yield return new WaitForSeconds(0.01f);
            }
            rewardText.text = rewardAmount.ToString() + " s";
        }

        IEnumerator CountDamageDealt()
        {
            damageDealtText.text = "0";
            int countDamageDealt = 0;
            int increaseAmount = Mathf.CeilToInt(Mathf.Pow(10, Mathf.CeilToInt(Mathf.Log10(damageDealtAmount / 50))));
            while (damageDealtAmount > countDamageDealt)
            {
                countDamageDealt += increaseAmount;
                damageDealtText.text = countDamageDealt.ToString();
                yield return new WaitForSeconds(0.01f);
            }
            damageDealtText.text = damageDealtAmount.ToString();
        }

        IEnumerator CountDamageTaken()
        {
            damageTakenText.text = "0";
            int countDamageTaken = 0;
            int increaseAmount = Mathf.CeilToInt(Mathf.Pow(10, Mathf.CeilToInt(Mathf.Log10(damagetakenAmount / 50))));
            while (damagetakenAmount > countDamageTaken)
            {
                countDamageTaken += increaseAmount;
                damageTakenText.text = countDamageTaken.ToString();
                yield return new WaitForSeconds(0.01f);
            }
            damageTakenText.text = damagetakenAmount.ToString();
        }

        IEnumerator CountItemUsed()
        {
            itemUsedText.text = "0";
            int countItemUsed = 0;
            int increaseAmount = 1;
            while (itemUsedAmount > countItemUsed)
            {
                countItemUsed += increaseAmount;
                itemUsedText.text = countItemUsed.ToString();
                yield return new WaitForSeconds(0.01f);
            }
            damageTakenText.text = damagetakenAmount.ToString();
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

        public void SetCompletionTime(int newMinute,int newSecond,int newMilliSecond)
        {
            minute = newMinute;
            second = newSecond;
            milliSecond = newMilliSecond;
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

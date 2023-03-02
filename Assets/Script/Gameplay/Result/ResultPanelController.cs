using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ArmasCreator.Utilities;

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

        [SerializeField]
        private GameObject content;

        [SerializeField]
        private Animator resultPanelAnimator;

        private int minute;
        private int second;
        private int milliSecond;

        private int rewardAmount;

        private int damageDealtAmount;
        private int damagetakenAmount;
        private int itemUsedAmount;

        public static bool IsResultSequenceFinished { get; private set; }

        private bool IsMilliSequenceFinished;
        private bool IsSecSequenceFinished;
        private bool IsMinuteSequenceFinished;
        private bool IsDamageDeltSequenceFinished;
        private bool IsDamageTakenSequenceFinished;
        private bool IsItemUsedSequenceFinished;
        private bool IsRewardSequenceFinished;

        private GameplayController gameplayController;

        private const float runTime = 0.025f;


        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
        }

        void Start()
        {
            endMissionButton.onClick.AddListener(() =>
            {
                OnEndMission();
            });

            gameplayController.OnStateResult += StartResultSequence;
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public void StartResultSequence()
        {
            IsResultSequenceFinished = false;
            IsMilliSequenceFinished = false;
            IsSecSequenceFinished = false;
            IsDamageDeltSequenceFinished = false;
            IsDamageTakenSequenceFinished = false;
            IsItemUsedSequenceFinished = false;
            IsRewardSequenceFinished = false;

            SetCompletionTime(10, 30, 99);
            SetRewardAmount(2500);
            SetResultData(5000, 3000, 10);
            SetRank("A", "A", "A", "A");

            resultPanelAnimator.SetTrigger("show");
        }

        public void Skip()
        {
            completionTimeText.text = minute.ToString().PadLeft(2, '0') + ":" + second.ToString().PadLeft(2, '0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
            rewardText.text = rewardAmount.ToString() + " s";
            damageDealtText.text = damageDealtAmount.ToString();
            damageTakenText.text = damagetakenAmount.ToString();
            damageTakenText.text = damagetakenAmount.ToString();
            SetRank("A","A","A","A");
            resultPanelAnimator.SetTrigger("Skip");
        }

        public void PlayTimeSequence()
        {
            StartCoroutine(TimeSequence());
        }

        public void PlayRewardSequence()
        {
            StartCoroutine(RewardSequence());
        }

        public void PlayDealtSequence()
        {
            StartCoroutine(DamageDealtSequence());
        }

        public void PlayTakenSequence()
        {
            StartCoroutine(DamageTakenSequence());
        }

        public void PlayItemSequence()
        {
            StartCoroutine(ItemSequence());
        }

        public IEnumerator TimeSequence()
        {
            StartCoroutine(CountMilliSecondTime());

            yield return new WaitUntil(() => IsMilliSequenceFinished);

            StartCoroutine(CountSecondTime());

            yield return new WaitUntil(() => IsSecSequenceFinished);

            StartCoroutine(CountMinuteTime());

            yield return new WaitUntil(() => IsMinuteSequenceFinished);

            resultPanelAnimator.SetTrigger("timeComplete");
        }

        public IEnumerator RewardSequence()
        {
            StartCoroutine(CountReward());

            yield return new WaitUntil(() => IsRewardSequenceFinished);

            resultPanelAnimator.SetTrigger("rewardComplete");
        }

        public IEnumerator DamageDealtSequence()
        {
            StartCoroutine(CountDamageDealt());

            yield return new WaitUntil(() => IsDamageDeltSequenceFinished);

            resultPanelAnimator.SetTrigger("dealtComplete");
        }

        public IEnumerator DamageTakenSequence()
        {
            StartCoroutine(CountDamageTaken());

            yield return new WaitUntil(() => IsDamageTakenSequenceFinished);

            resultPanelAnimator.SetTrigger("takenComplete");
        }

        public IEnumerator ItemSequence()
        {
            StartCoroutine(CountItemUsed());

            yield return new WaitUntil(() => IsItemUsedSequenceFinished);

            resultPanelAnimator.SetTrigger("itemComplete");

        }

        public void ResultSequenceFinished()
        {
            IsResultSequenceFinished = true;
        }

        IEnumerator CountMilliSecondTime()
        {
            completionTimeText.text = "00" + ":" + "00" + ":" + "00" + "'";
            int countMilliSecondTime = 0;
            while (milliSecond > countMilliSecondTime)
            {
                countMilliSecondTime += 1;
                completionTimeText.text = "00" + ":" + "00" + ":" + countMilliSecondTime.ToString().PadRight(2,'0') + "'";
                yield return new WaitForSeconds(runTime / 3);
            }
            completionTimeText.text = "00" + ":" + "00" + ":" + milliSecond.ToString().PadLeft(2,'0') + "'";
            IsMilliSequenceFinished = true;
        }

        IEnumerator CountSecondTime()
        {
            completionTimeText.text = "00" + ":" + "00" + ":" + milliSecond + "'";
            int countSecondTime = 0;
            while (second > countSecondTime)
            {
                countSecondTime += 1;
                completionTimeText.text = "00" + ":" + countSecondTime.ToString().PadRight(2, '0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
                yield return new WaitForSeconds(runTime / 3);
            }
            completionTimeText.text = "00" + ":" + second.ToString().PadLeft(2,'0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
            IsSecSequenceFinished = true;
        }

        IEnumerator CountMinuteTime()
        {
            completionTimeText.text = "00" + ":" + second + ":" + milliSecond + "'";
            int countMinuteTime = 0;
            while (minute > countMinuteTime)
            {
                countMinuteTime += 1;
                completionTimeText.text = countMinuteTime.ToString().PadLeft(2,'0') + ":" + second.ToString().PadRight(2, '0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
                yield return new WaitForSeconds(runTime / 3);
            }
            completionTimeText.text = minute.ToString().PadLeft(2, '0') + ":" + second.ToString().PadLeft(2, '0') + ":" + milliSecond.ToString().PadLeft(2, '0') + "'";
            IsMinuteSequenceFinished = true;
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
                yield return new WaitForSeconds(runTime);
            }
            rewardText.text = rewardAmount.ToString() + " s";
            IsRewardSequenceFinished = true;
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
                yield return new WaitForSeconds(runTime);
            }
            damageDealtText.text = damageDealtAmount.ToString();
            IsDamageDeltSequenceFinished = true;
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
                yield return new WaitForSeconds(runTime);
            }
            damageTakenText.text = damagetakenAmount.ToString();
            IsDamageTakenSequenceFinished = true;
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
                yield return new WaitForSeconds(runTime);
            }
            damageTakenText.text = damagetakenAmount.ToString();
            IsItemUsedSequenceFinished = true;
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
            IsResultSequenceFinished = true;
        }

        private void OnDestroy()
        {
            if(gameplayController != null)
            {
                gameplayController.OnStateResult -= StartResultSequence;
            }
        }
    }
}

using ArmasCreator.UI;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private TMP_Text enemyAmountText;

    [SerializeField]
    private Animator anim;

    private LoadingPopup loadingPopup;

    private bool isInit;
    private bool isFinishTutorial;

    public Transform[] spawnPos;

    private int maxEnemy = 0;

    private void Awake()
    {
        loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
    }

    void Start()
    {
        foreach(var transform in spawnPos)
        {
            var enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
            maxEnemy++;
        }

        isInit = true;

        StartCoroutine(waitForFadeBlack());
    }

    void Update()
    {
        if (!isInit) { return; }

        var enemy = GameObject.FindGameObjectsWithTag("Enemy");

        enemyAmountText.text = "Kill Slime " + (maxEnemy - enemy.Length).ToString() + " / " + maxEnemy.ToString();

        if (enemy.Length == 0 && !isFinishTutorial)
        {
            StartCoroutine(waitUntilShowFinishEnd());
        }
    }

    IEnumerator waitForFadeBlack()
    {
        yield return new WaitUntil(() => loadingPopup.IsFadingBlack);
        yield return new WaitForSeconds(1.8f);
        anim.SetTrigger("ShowStart");
    }
    IEnumerator waitUntilShowFinishEnd()
    {
        anim.SetBool("ShowFinish", true);
        isFinishTutorial = true;
        yield return new WaitForSeconds(1.2f);
        FinishTutorial();
    }

    public void FinishTutorial()
    {
        loadingPopup.LoadSceneAsync("Town");
    }


    public void StartTutorial()
    {
        anim.SetTrigger("ShowStart");
    }
}

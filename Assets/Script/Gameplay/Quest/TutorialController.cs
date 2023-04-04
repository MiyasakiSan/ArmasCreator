using ArmasCreator.UI;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;

    private LoadingPopup loadingPopup;

    private bool isInit;
    private bool isFinishTutorial;

    public Transform[] spawnPos;

    private void Awake()
    {
        loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
    }

    void Start()
    {
        foreach(var transform in spawnPos)
        {
            var enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        }

        isInit = true;
    }

    void Update()
    {
        if (!isInit) { return; }

        var enemy = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemy.Length == 0 && !isFinishTutorial)
        {
            FinishTutorial();
        }
    }

    public void FinishTutorial()
    {
        isFinishTutorial = true;
        loadingPopup.LoadSceneAsync("Town");
    }
}

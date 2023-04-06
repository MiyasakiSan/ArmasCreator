using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay;
using ArmasCreator.GameData;

public class TestAnalytics : MonoBehaviour
{
    private void Awake()
    {
        SharedContext.Instance.Add(this);
        DontDestroyOnLoad(this.gameObject);
    }

    private async void Start()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("dev");
            
            await UnityServices.InitializeAsync(options);
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }

        for(int i = 0; i< 10; i++)
        {
            //testSendCustomEvent(i);
            //testSendCustomMapEvent(i);
        }
    }

    private void testSendCustomEvent(int i)
    {
        var customParams = new Dictionary<string, object>();
        customParams.Add("mapId", $"testMap{i}");
        customParams.Add("score", Random.Range(5000,10000));
        customParams.Add("time", Time.time);

        AnalyticsService.Instance.CustomData("ChallengeModeEnd", customParams);

        AnalyticsService.Instance.Flush();
        Debug.Log("Do");
    }

    private void testSendCustomMapEvent(int i)
    {
        var customParams = new Dictionary<string, object>();
        float time = 600 - (i * Random.Range(6,10));
        customParams.Add("Clear", i);
        customParams.Add("ClearTime", time);

        AnalyticsService.Instance.CustomData("Map1", customParams);

        AnalyticsService.Instance.Flush();
        Debug.Log("Do");
    }

    public void SendResultToAnalyze(SubType enemyType, string challengId , ResultContainer resultContainer)
    {
        var customParams = new Dictionary<string, object>();
        customParams.Add("challengeId", challengId);
        customParams.Add("userName", PlayerPrefs.GetString("PName"));
        customParams.Add("winNo", PlayerPrefs.GetInt(challengId).ToString());
        customParams.Add("timeAmount", resultContainer.timeAmount);
        customParams.Add("damageDelt", resultContainer.damageDelt);
        customParams.Add("damageTaken", resultContainer.damageTaken);
        customParams.Add("itemUsed", resultContainer.itemUsed);

        if (enemyType == SubType.Lion)
        {
            AnalyticsService.Instance.CustomData("CiladaeMap", customParams);
        }
        else
        {
            AnalyticsService.Instance.CustomData("GotenaMap", customParams);
        }

        AnalyticsService.Instance.Flush();
        Debug.Log($"========== Send {challengId} result to analyze ==============");
    }

    public void OnDestroy()
    {
        SharedContext.Instance.Remove(this);
    }

}

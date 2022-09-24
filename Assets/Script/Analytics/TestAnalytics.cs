using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class TestAnalytics : MonoBehaviour
{
    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }

        for(int i = 0; i< 10; i++)
        {
            //testSendCustomEvent(i);
            testSendCustomMapEvent(i);
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

}

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

        testSendCustomEvent();
    }

    private void testSendCustomEvent()
    {
        var customParams = new Dictionary<string, object>();
        customParams.Add("mapId", "testMapId");
        customParams.Add("score", 10000);
        customParams.Add("time", Time.time);

        AnalyticsService.Instance.CustomData("ChallengeModeEnd", customParams);

        AnalyticsService.Instance.Flush();
        Debug.Log("Do");
    }

}

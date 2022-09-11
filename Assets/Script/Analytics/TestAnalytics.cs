using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class TestAnalytics : MonoBehaviour
{
    private void Start()
    {
        //testSendAnalyticEvent();
        testSendCustomEvent();
    }

    private void testSendAnalyticEvent()
    {
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("MapId", "testMapId");
        customParams.Add("TestScore", 10000);
        customParams.Add("TestTime", Time.time);

        var result = AnalyticsEvent.LevelComplete("test",customParams);

        Debug.Log(result);

    }

    private void testSendCustomEvent()
    {
        var customParams = new Dictionary<string, object>();
        customParams.Add("MapId", "testMapId");
        customParams.Add("TestScore", 10000);
        customParams.Add("TestTime", Time.time);

        var result = Analytics.CustomEvent("ChallengeModeEnd", customParams);

        Debug.Log(result);
    }

}

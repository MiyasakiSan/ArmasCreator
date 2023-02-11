using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowITweenPath : MonoBehaviour
{
    public string PathName;
    public float Time;

    void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(PathName), "time", Time));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNameFadeController : MonoBehaviour
{
    public enum BossType
    {
        CiladaeRoca,
        GotenaDunga
    }

    [SerializeField]
    private BossType bossType;

    [SerializeField]
    private Animator anim;

    public void OnStartFade()
    {
        if(bossType == BossType.CiladaeRoca)
        {
            anim.SetTrigger("ShowCiladae");
        }
        else
        {
            anim.SetTrigger("ShowGotena");
        }
    }
}

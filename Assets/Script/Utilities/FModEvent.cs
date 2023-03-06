using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FModEvent : MonoBehaviour
{
    [field : Header("SFX")]
    [field : Header("Player")]
    [field : SerializeField] public EventReference PlayerWalkSFX {get; private set;}
    [field : SerializeField] public EventReference PlayerRunSFX {get; private set;}

    
    [field : Header("UI")]
    [field : SerializeField] public EventReference ButtonSFX {get; private set;}
}

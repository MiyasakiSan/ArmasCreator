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
    [field: SerializeField] public EventReference PlayerRollFX { get; private set; }
    [field: SerializeField] public EventReference PlayerFirstComboSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerSecondComboSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerThirdComboSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerGunComboSFX { get; private set; }


    [field : Header("UI")]
    [field : SerializeField] public EventReference ButtonSFX {get; private set;}
    [field: SerializeField] public EventReference MouseHoverSFX { get; private set; }

    [field: Header("BGM")]
    [field: SerializeField] public EventReference TownBGM { get; private set; }
    [field: SerializeField] public EventReference MainmenuBGM { get; private set; }
    [field: SerializeField] public EventReference ConstructionSiteBGM { get; private set; }
    [field: SerializeField] public EventReference ParkBGM { get; private set; }
}

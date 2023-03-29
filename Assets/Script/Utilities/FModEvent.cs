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
    [field: SerializeField] public EventReference PlayerRollSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerFirstComboSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerSecondComboSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerThirdComboSFX { get; private set; }
    [field: SerializeField] public EventReference PlayerGunComboSFX { get; private set; }

    [field: Header("Shrimp")]
    [field: SerializeField] public EventReference LegSFX { get; private set; }
    [field: SerializeField] public EventReference MachineSFX { get; private set; }
    [field: SerializeField] public EventReference ShrimpRoar { get; private set; }
    [field: SerializeField] public EventReference ShrimpLightning { get; private set; }
    [field: SerializeField] public EventReference ShrimpSmash { get; private set; }
    [field: SerializeField] public EventReference ShrimpThrash { get; private set; }
    [field: SerializeField] public EventReference ShrimpPierce { get; private set; }
    [field: SerializeField] public EventReference ShrimpDead { get; private set; }
    [field: SerializeField] public EventReference BubbleShoot { get; private set; }
    [field: SerializeField] public EventReference WaterBeam { get; private set; }
    [field: SerializeField] public EventReference PlasmaBall { get; private set; }
    [field: SerializeField] public EventReference WaterGround { get; private set; }
    [field: SerializeField] public EventReference ThunderGround { get; private set; }
    [field: SerializeField] public EventReference UltimateGround { get; private set; }
    [field: SerializeField] public EventReference ShrimpCrash { get; private set; }
    [field: SerializeField] public EventReference ShrimpGetHit { get; private set; }

    [field: Header("Tiger")]


    [field : Header("UI")]
    [field : SerializeField] public EventReference ButtonSFX {get; private set;}
    [field: SerializeField] public EventReference MouseHoverSFX { get; private set; }

    [field: Header("BGM")]
    [field: SerializeField] public EventReference TownBGM { get; private set; }
    [field: SerializeField] public EventReference MainmenuBGM { get; private set; }
    [field: SerializeField] public EventReference ConstructionSiteBGM { get; private set; }
    [field: SerializeField] public EventReference ParkBGM { get; private set; }
    [field: SerializeField] public EventReference WinBGM { get; private set; }

    [field: Header("AMBIENCE")]
    [field: SerializeField] public EventReference TownAMBIENCE { get; private set; }
    [field: SerializeField] public EventReference MainmenuAMBIENCE { get; private set; }
    [field: SerializeField] public EventReference ConstructionSiteAMBIENCE { get; private set; }
    [field: SerializeField] public EventReference ParkAMBIENCE { get; private set; }
}

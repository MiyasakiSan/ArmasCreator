using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBoxcolliderControl : MonoBehaviour
{
    [SerializeField] private WeaponShealthDraw weaponCollector;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void showHitbox()
    {
        weaponCollector.currentWeaponInHand?.GetComponent<IMeleeWeapon>().showHitbox_OnAnim();
    }
    public void hideHitbox()
    {
        weaponCollector.currentWeaponInHand?.GetComponent<IMeleeWeapon>().hideHitbox_OnAnim();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

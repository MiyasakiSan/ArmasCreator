using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.UserData;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;

public class LongSword : IMeleeWeapon
{
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _staggerDamage;
    [SerializeField] private statusEffect _weaponStatus;

    [SerializeField] private GameObject GotenaWeapon;
    [SerializeField] private GameObject CiladaeWeapon;

    private UserDataManager userDataManager = SharedContext.Instance.Get<UserDataManager>();
    private GameDataManager gameDataManager = SharedContext.Instance.Get<GameDataManager>();

    public Transform gunTransform;
    public float attackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }
    public float staggerDamage
    {
        get { return _staggerDamage; }
        set { _staggerDamage = value; }
    }

    public statusEffect weaponStatus
    {
        get { return _weaponStatus; }
        set { _weaponStatus = value; }
    }

    void Awake()
    {
        
    }

    void Start()
    {
        setUpgradeWeapon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setUpgradeWeapon()
    {
        Instantiate(CiladaeWeapon, this.gameObject.transform);
        Instantiate(GotenaWeapon, this.gameObject.transform);
        var weaponId = userDataManager.UserData.UserDataInventory.GetEquipItemInfo(SubType.Weapon);

        gameDataManager.GetEquipInfoSubType(weaponId.ID, out string asset);

        if (asset == "ciladae")
        {
            GotenaWeapon.SetActive(false);
        }
        else if (asset == "gotena")
        {
            CiladaeWeapon.SetActive(false);
        }
        else
        {
            GotenaWeapon.SetActive(false);
            CiladaeWeapon.SetActive(false);
        }
    }


    public override void Attack(AttackTarget target, float damage)
    {
        //base.Attack(target,damage);
        Debug.Log("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AttackTarget>() && !other.CompareTag("Player"))
        {
            Attack(other.GetComponent<AttackTarget>(), _weaponDamage);
            //other.GetComponent<UseVFX>().OnUseVFX(0);
        }
    }
}

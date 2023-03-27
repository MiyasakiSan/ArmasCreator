using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGroundController : MonoBehaviour
{
    [SerializeField] private List<GameObject> OuterWaterGroundList;
    [SerializeField] private List<GameObject> MiddleWaterGroundList;
    [SerializeField] private List<GameObject> InnerWaterGroundList;
    [Space(10)]

    [SerializeField] private List<GameObject> OuterThunderGroundList;
    [SerializeField] private List<GameObject> InnerThunderGroundList;
    [Space(10)]

    [SerializeField] private GameObject WaterGroundPrefab;
    [SerializeField] private GameObject thunderGroundPrefab;

    public delegate void OnGroundWaterFinished();
    public OnGroundWaterFinished onGroundWaterFinishedCallback;

    void Start()
    {
        
    }

    public void InitGroundWater(float damage)
    {
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                {
                    StartCoroutine(FirstGroundWater(damage));
                    break;
                }
            case 1:
                {
                    StartCoroutine(SecondGroundWater(damage));
                    break;
                }
            case 2:
                {
                    StartCoroutine(ThirdGroundWater(damage));
                    break;
                }
        }
    }

    public void InitGroundThunder(float damage)
    {
        StartCoroutine(ThunderGround(damage));
    }

    public void InitUltimateGround(float damage)
    {
        StartCoroutine(UltimateThunderGround(damage));
    }

    IEnumerator FirstGroundWater(float damage)
    {
        foreach(var groundWater in OuterWaterGroundList)
        {
            groundWater.SetActive(true);
            groundWater.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        foreach (var groundWater in MiddleWaterGroundList)
        {
            groundWater.SetActive(true);
            groundWater.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        foreach (var groundWater in InnerWaterGroundList)
        {
            groundWater.SetActive(true);
            groundWater.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        onGroundWaterFinishedCallback?.Invoke();
        CloseAll();
    }

    IEnumerator SecondGroundWater(float damage)
    {
        foreach (var groundWater in InnerWaterGroundList)
        {
            groundWater.SetActive(true);
            groundWater.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        foreach (var groundWater in MiddleWaterGroundList)
        {
            groundWater.SetActive(true);
            groundWater.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        foreach (var groundWater in OuterWaterGroundList)
        {
            groundWater.SetActive(true);
            groundWater.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        onGroundWaterFinishedCallback?.Invoke();
        CloseAll();
    }

    IEnumerator ThirdGroundWater(float damage)
    {
        for(int i =0; i< 3; i++)
        {
            var player = GameObject.FindGameObjectWithTag("Player").transform.position;

            var groundWater = Instantiate(WaterGroundPrefab, new Vector3(player.x, player.y - 1f, player.z), Quaternion.Euler(0, 90 * i, 0));
            groundWater.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);

            yield return new WaitForSeconds(2f);

            Destroy(groundWater);
        }

        onGroundWaterFinishedCallback?.Invoke();
    }

    IEnumerator ThunderGround(float damage)
    {
        for (int i = 0; i < 3; i++)
        {
            var player = GameObject.FindGameObjectWithTag("Player").transform.position;

            var groundThunder = Instantiate(thunderGroundPrefab, new Vector3(player.x, player.y - 1f, player.z), Quaternion.Euler(0, 45 * i, 0));
            groundThunder.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);

            yield return new WaitForSeconds(3f);

            Destroy(groundThunder);
        }
    }

    IEnumerator UltimateThunderGround(float damage)
    {
        foreach (var groundThunder in OuterThunderGroundList)
        {
            groundThunder.SetActive(true);
            groundThunder.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        foreach (var groundThunder in InnerThunderGroundList)
        {
            groundThunder.SetActive(true);
            groundThunder.GetComponentInChildren<ColliderDamage>().SetupAttackPattern(damage);
        }

        yield return new WaitForSeconds(2f);

        onGroundWaterFinishedCallback?.Invoke();
        CloseAllThunder();
    }

    void CloseAllThunder()
    {
        foreach (var groundThunder in OuterThunderGroundList)
        {
            groundThunder.SetActive(false);
        }

        foreach (var groundThunder in InnerThunderGroundList)
        {
            groundThunder.SetActive(false);
        }
    }

    void CloseAll()
    {
        foreach (var groundWater in OuterWaterGroundList)
        {
            groundWater.SetActive(false);
        }

        foreach (var groundWater in MiddleWaterGroundList)
        {
            groundWater.SetActive(false);
        }

        foreach (var groundWater in InnerWaterGroundList)
        {
            groundWater.SetActive(false);
        }
    }
}

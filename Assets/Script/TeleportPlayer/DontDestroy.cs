using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] gameObjects;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("PlayerCanvas");
        if (gameObjects.Length > 1)
        {
            Destroy(gameObjects[1]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveAround : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private Transform center;

    [SerializeField]
    private Transform playerPos;

    private float playerSlope;

    public float radius;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateProjection();
    }

    private void CalculateProjection()
    {
        if ( playerPos.position.z != center.position.z)
        {
            playerSlope = (playerPos.position.x - center.position.x) / (playerPos.position.z - center.position.z);
        }
        else
        {
            playerSlope = 0;
        }
        

        float c = playerPos.position.z - (playerSlope * playerPos.position.x);

        float A, B, C;

        float x1 = 0, x2 = 0, disc, deno, xPos, yPos;

        A = Mathf.Pow(playerSlope, 2) + 1;
        B = 2*playerSlope * c;
        C = Mathf.Pow(c, 2) - Mathf.Pow(radius, 2);

        if (A == 0)
        {
            x1 = -C / B;
        }
        else
        {
            disc = (B * B) - (4 * A * C);
            deno = 2 * A;
            if (disc > 0)
            {
                x1 = (-B / deno) + (Mathf.Sqrt(disc) / deno);
                x2 = (-B / deno) - (Mathf.Sqrt(disc) / deno);
            }
            else if (disc == 0)
            {
                x1 = -B / deno;
            }
            else
            {
                x1 = -B / deno;
                x2 = ((Mathf.Sqrt((4 * A * C) - (B * B))) / deno);
            }
        }

        if (playerPos.position.x >= 0)
        {
            xPos = x1;
        }
        else
        {
            xPos = x2;
        }

        yPos = playerSlope * xPos + c;

        //transform.position = new Vector3(xPos, transform.position.y, yPos);

        transform.position = Vector3.Lerp(transform.position, new Vector3(xPos, transform.position.y, yPos), Time.deltaTime * speed );

        var lookPos = playerPos.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);
    }
}


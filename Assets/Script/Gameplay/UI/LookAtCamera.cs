using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.Environment
{
    public class LookAtCamera : MonoBehaviour
    {
        Camera cam;
        void Start()
        {
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.forward = cam.transform.position - transform.position;
        }
    }
}

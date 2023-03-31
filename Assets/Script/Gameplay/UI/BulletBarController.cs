using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ArmasCreator.UI
{
    public class BulletBarController : MonoBehaviour
    {
        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        private Transform Content;

        private List<GameObject> bulletNodes = new List<GameObject>();

        public void OnShoot()
        {
            if(bulletNodes.Count > 0)
            {
                GameObject currentBullet = bulletNodes[bulletNodes.Count - 1];
                bulletNodes.RemoveAt(bulletNodes.Count - 1);
                Destroy(currentBullet);
            }
        }

        public void OnReload()
        {
            if (bulletNodes.Count < 4)
            {
                bulletNodes.Add(Instantiate(bulletPrefab, Content));
            }
        }
    }
}

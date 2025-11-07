using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    public class BulletObjectPool : MonoBehaviour
    {
        private GameObject bullletPrefab;
        public int initialSize = 20;
        Queue<GameObject> pool = new Queue<GameObject>();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Init(GameObject bullletPrefab)
        {
            this.bullletPrefab = bullletPrefab;
            for (int i = 0; i < initialSize; i++)
            {
                GameObject bullet = Instantiate(bullletPrefab);
                bullet.SetActive(false);
                pool.Enqueue(bullet);
            }
        }

        public GameObject Get()
        {
            if (pool.Count == 0)
            {
                GameObject newBullet = Instantiate(bullletPrefab);
                newBullet.SetActive(false);
                pool.Enqueue(newBullet);
            }

            GameObject bullet = pool.Dequeue();
            bullet.SetActive(true);
            return bullet;

        }

        public void Return(GameObject bullet)
        {
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
    }
}

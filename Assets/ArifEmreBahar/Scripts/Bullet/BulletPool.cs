using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    Bullet bulletPrefab;
    int poolSize;

    Queue<Bullet> bulletQueue; 

    public BulletPool(Bullet prefab, int size)
    {
        bulletPrefab = prefab;
        poolSize = size;

        bulletQueue = new Queue<Bullet>();

        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            bulletQueue.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if (bulletQueue.Count > 0)
        {
            Bullet bullet = bulletQueue.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            return bullet;
        }
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletQueue.Enqueue(bullet);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    normal,
    bomb,
}

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    public BulletType bulletType;

    [SerializeField] private GameObject bulletPrefab;

    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }

    public void Shoot(BulletType bulletType)
    {
        switch (bulletType)
        {
            case BulletType.normal:
                Destroy(gameObject, 5.0f);
                break;
            case BulletType.bomb:
                StartCoroutine(BombShoot(1.5f));
                break;
        }
    }

    IEnumerator BombShoot(float second)
    {
        int bulletCount = 8;
        float time = 3;

        yield return new WaitForSeconds(time);

        for (int i = 0; i < 360; i += 360 / bulletCount)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, i));
            bullet.GetComponent<Bullet>().Shoot(BulletType.normal);
        }

        Destroy(gameObject);
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.CallCoroutine();
            PlayerController.instance._playerHp -= damage;
            Destroy(gameObject);
        }
    }
}

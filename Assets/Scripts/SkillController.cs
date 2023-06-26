using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public GameObject _explosionEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.CompareTag("Harpoon"))
        {
            // 게임오브젝트가 바라보는 방향으로 이동합니다.
            this.transform.Translate(Vector2.up * 10f * Time.deltaTime);
        }
        if (this.CompareTag("Jangpung"))
        {
            // 게임오브젝트가 바라보는 방향으로 velocity를 적용하여 이동합니다.
            this.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.up * 10f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CompareTag("Mine") && other.CompareTag("Enemy"))
        {
            GameObject effect = Instantiate(_explosionEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}

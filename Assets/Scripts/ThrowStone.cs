using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStone : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int power;

    private void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !PlayerController.instance._isDash &&
        !PlayerController.instance._isDying && !PlayerController.instance._isFinishing
        && !PlayerController.instance._isInvincible)
        {
            PlayerController.instance._playerHp -= power;
            PlayerController.instance.CallCoroutine();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

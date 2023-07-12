using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour
{
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float attackCurrentTime;

    private void Start()
    {
        attackCurrentTime = attackCoolTime;
    }

    private void Update()
    {
        attackCurrentTime += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && attackCurrentTime >= attackCoolTime)
        {
            attackCurrentTime = 0;
            PlayerController.instance._playerHp -= 5;
            PlayerController.instance.CallCoroutine();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxolotlPoison : MonoBehaviour
{
    private float currentTime;
    [SerializeField] private float coolTime;
    [SerializeField] private int damage;
    [SerializeField] private float destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        currentTime += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentTime >= coolTime)
        {
            Debug.Log("독데미지");
            other.GetComponent<PlayerController>()._playerHp -= damage;
            currentTime = 0;
        }
    }
}
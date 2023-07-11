using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AtkEffectFinished()
    {
        this.gameObject.SetActive(false);
        PlayerController.instance._isAttacking = false;
        PlayerController.instance._playerAgent.isStopped = false;
    }

    public void DestroyOnEffectFinished()
    {
        Destroy(gameObject);
    }
}

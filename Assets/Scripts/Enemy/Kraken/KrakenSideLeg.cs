using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenSideLeg : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Vector2 dir;

    public void SideDir(Vector2 dir)
    {
        this.dir = dir;
    }

    private void FixedUpdate()
    {
        Debug.Log(dir);
        //transform.position += dir *=
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("멈춰라 얍!");
        }
    }
}

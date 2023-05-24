using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 1f);
        transform.position = Vector2.Lerp(transform.position, transform.position + new Vector3(0, 1, 0), Time.deltaTime * 1f);
        this.GetComponent<TextMeshPro>().color = new Color(255, 0, 0, Mathf.Lerp(this.GetComponent<TextMeshPro>().color.a, 0, Time.deltaTime * 2f));
    }
}

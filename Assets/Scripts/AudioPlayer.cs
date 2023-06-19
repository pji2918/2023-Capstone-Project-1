using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    bool _isSoundOn = false;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AudioSource>().Play();
        _isSoundOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<AudioSource>().isPlaying && _isSoundOn)
        {
            Destroy(this);
        }
    }
}

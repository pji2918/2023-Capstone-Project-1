using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeAudioScript : MonoBehaviour
{
    void Start()
    {
        SoundManager.instance.PlayMusic(SoundManager.instance.GetAudioClip("Now,here/You're my air"));
    }
}

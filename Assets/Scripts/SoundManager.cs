using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    void Awake()
    {
        instance = this;
    }

    public AudioClip GetAudioClip(string name)
    {
        return Resources.Load<AudioClip>(Path.Combine("Audio", name));
    }

    [SerializeField] private AudioSource _musicPlayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMusic(AudioClip clip)
    {
        _musicPlayer.clip = clip;
        _musicPlayer.Play();
        Debug.Log("음악 재생");
    }

    public void StopMusic()
    {
        if (_musicPlayer.isPlaying)
        {
            _musicPlayer.Stop();
        }
        else
        {
            Debug.LogWarning("음악이 재생 중이 아닙니다.");
        }
    }

    public void PauseOrResumeMusic()
    {
        if (_musicPlayer.isPlaying)
        {
            _musicPlayer.Pause();
        }
        else
        {
            _musicPlayer.UnPause();
        }
    }
}

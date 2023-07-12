using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    void Awake()
    {
        instance = this;
    }

    private AudioSource _musicPlayer;
    [SerializeField] private AudioSource _playerAudio;
    private int _musicVolume = 100;
    private int _effectVolume = 100;

    /// <summary>
    /// 음악의 음량입니다. 0 ~ 100 사이의 값을 가집니다.
    /// </summary>
    public int MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            if (value < 0 || value > 100)
            {
                Debug.LogError("음악 음량은 0 ~ 100 사이의 값을 가져야 합니다.");
            }
            else
            {
                _musicVolume = value;
            }
        }
    }

    /// <summary>
    /// 효과음의 음량입니다. 0 ~ 100 사이의 값을 가집니다.
    /// </summary>
    public int EffectVolume
    {
        get
        {
            return _effectVolume;
        }
        set
        {
            if (value < 0 || value > 100)
            {
                Debug.LogError("효과음 음량은 0 ~ 100 사이의 값을 가져야 합니다.");
            }
            else
            {
                _effectVolume = value;
            }
        }
    }


    #region GetAudioClip의 오버로드

    /// <summary>
    /// 오디오 클립을 Resources 폴더에서 가져와서 반환합니다.
    /// </summary>
    /// <param name="name">가져올 오디오 클립의 이름을 여기에 지정하십시오.</param>
    /// <returns>지정한 오디오 클립이 AudioClip 형식으로 반환됩니다.</returns>
    public AudioClip GetAudioClip(string name)
    {
        return Resources.Load<AudioClip>(Path.Combine("Audio", name));
    }

    /// <summary>
    /// 오디오 클립을 Resources 폴더에서 가져와서 반환합니다.
    /// </summary>
    /// <param name="clip">AudioClips 열거형에서 가져올 오디오 클립을 여기에 지정하십시오.</param>
    /// <returns>지정한 오디오 클립이 AudioClip 형식으로 반환됩니다.</returns>
    public AudioClip GetAudioClip(AudioClips clip)
    {
        switch (clip)
        {
            case AudioClips.Axolotl:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Aholotle_CDA_Oriental_Fever_FULL_Loop"));
                }
            case AudioClips.Ending1:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Ending1_MP_Enlightenment_FULL_Loop"));
                }
            case AudioClips.Ending2:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Ending2_Burning water"));
                }
            case AudioClips.Fight:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Fight_Be yourself"));
                }
            case AudioClips.Kraken:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Kraken_CDA_Ready_Steady_GO_FULL_Loop"));
                }
            case AudioClips.Lobby:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Lobby_You're my air"));
                }
            case AudioClips.Title:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Title_Midnight"));
                }
            case AudioClips.Whale:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "BGM", "Whale_MP_SecretLabyrinth_FULL_Loop"));
                }
            case AudioClips.Click:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Click"));
                }
            case AudioClips.Dead:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Dead"));
                }
            case AudioClips.E_Skill:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "E_Skill"));
                }
            case AudioClips.Get_Food:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Get_Food"));
                }
            case AudioClips.Get_Item:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Get_Item"));
                }
            case AudioClips.Q_Skill:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Q_Skill"));
                }
            case AudioClips.R_Skill:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "R_Skill"));
                }
            case AudioClips.R_Skill2:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "R_Skill2"));
                }
            case AudioClips.Slow:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Slow"));
                }
            case AudioClips.Upgrade:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "UP_SkillShelter"));
                }
            case AudioClips.W_Skill:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "W_Skill"));
                }
            case AudioClips.Axolotl_Damage:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Aholotle_Damage"));
                }
            case AudioClips.Axolotl_Dash:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Aholotle_Dash"));
                }
            case AudioClips.Kraken_Attack:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Kraken_Attack"));
                }
            case AudioClips.Kraken_BFlash:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Kraken_BackFlash"));
                }
            case AudioClips.Loop_BFlash:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Loop_BackFlash"));
                }
            case AudioClips.Loop_BFlash2:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Loop_BackFlash2"));
                }
            case AudioClips.Loop_BFlash3:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Loop_BackFlash3"));
                }
            case AudioClips.Loop_Background:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Loop_Background"));
                }
            case AudioClips.Loop_Background2:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Loop_Background2"));
                }
            case AudioClips.Monster_Damage:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Monster_Damage"));
                }
            case AudioClips.Monster_Hit:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Monster_Hit"));
                }
            case AudioClips.Whale_Attack:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Whale_Attack"));
                }
            case AudioClips.Whale_Damage:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Whale_Damage"));
                }
            case AudioClips.Whale_StartAttack:
                {
                    return Resources.Load<AudioClip>(Path.Combine("Sounds", "SFX", "Monster", "Whale_StartAtk"));
                }
            default:
                {
                    Debug.LogError("Audio Not Found.");
                    return null;
                }
        }
    }

    #endregion

    /// <summary>
    /// 다음 효과음들은 자주 사용되므로, 여기에 열거되어 있습니다.
    /// GetAudioClip()에서 사용할 수 있습니다.
    /// </summary>
    public enum AudioClips
    {
        BeYourSelf,
        Ocean_Boom,
        Axolotl,
        Ending1,
        Ending2,
        Fight,
        Kraken,
        Lobby,
        Title,
        Whale,
        Click,
        Dead,
        E_Skill,
        Get_Food,
        Get_Item,
        Q_Skill,
        R_Skill,
        R_Skill2,
        Slow,
        Upgrade,
        W_Skill,
        Axolotl_Damage,
        Axolotl_Dash,
        Kraken_Attack,
        Kraken_BFlash,
        Loop_BFlash,
        Loop_BFlash2,
        Loop_BFlash3,
        Loop_Background,
        Loop_Background2,
        Monster_Damage,
        Monster_Hit,
        Whale_Attack,
        Whale_Damage,
        Whale_StartAttack
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _musicVolume = DataManager.instance._data.musicVolume;
        _effectVolume = DataManager.instance._data.effectVolume;
        if (_musicPlayer is not null)
        {
            _musicPlayer.volume = _musicVolume / 100f; // 음악 음량 설정
        }
    }

    /// <summary>
    /// 음악을 재생합니다. 
    /// </summary>
    /// <param name = "clip">재생할 음악을 여기에 지정하십시오.</param>
    public void PlayMusic(AudioClip clip)
    {
        GetAudioSourceComponent();
        _musicPlayer.clip = clip;
        _musicPlayer.Play();
        Debug.Log("음악 재생");
    }

    private void GetAudioSourceComponent()
    {
        if (_musicPlayer is null)
        {
            _musicPlayer = this.gameObject.GetComponent<AudioSource>(); // 오디오 소스 컴포넌트를 가져옴
        }
    }

    /// <summary>
    /// 음악 재생을 정지합니다.
    /// </summary>
    public void StopMusic()
    {
        GetAudioSourceComponent();
        if (_musicPlayer.isPlaying)
        {
            _musicPlayer.Stop();
        }
        else
        {
            Debug.LogWarning("음악이 재생 중이 아닙니다.");
        }
    }

    /// <summary>
    /// 음악을 일시정지하거나, 다시 재생합니다.
    /// </summary>
    public void PauseOrResumeMusic()
    {
        GetAudioSourceComponent();
        if (_musicPlayer.isPlaying)
        {
            _musicPlayer.Pause();
        }
        else
        {
            _musicPlayer.UnPause();
        }
    }

    /// <summary>
    /// 효과음을 특정 게임오브젝트에 넣어 재생합니다.
    /// </summary>
    /// <param name="obj">효과음을 삽입할 게임오브젝트를 여기에 지정하십시오.</param>
    /// <param name="clip">재생할 효과음을 여기에 지정하십시오.</param>
    /// <returns></returns>
    public GameObject AddAndPlayEffects(GameObject obj, AudioClip clip)
    {
        obj.AddComponent<AudioSource>();
        obj.GetComponent<AudioSource>().volume = _effectVolume / 100f;
        obj.GetComponent<AudioSource>().loop = false;
        if (DataManager.instance._data.is3dAudio)
        {
            obj.GetComponent<AudioSource>().spatialBlend = 1f;
        }
        else
        {
            obj.GetComponent<AudioSource>().spatialBlend = 0f;
        }
        obj.AddComponent<AudioPlayer>();
        obj.GetComponent<AudioSource>().clip = clip;
        return obj;
    }

    /// <summary>
    /// 플레이어에서 효과음을 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 효과음을 여기에 지정하십시오.</param>
    public void PlayPlayerEffects(AudioClip clip)
    {
        if (SceneManager.GetActiveScene().name.Equals("Fighting"))
        {
            _playerAudio.volume = _effectVolume / 100f;
            _playerAudio.clip = clip;
            _playerAudio.Play();
        }
        else
        {
            Debug.LogError("오류: 해당 기능은 전투 Scene에서만 사용할 수 있습니다.");
        }
    }

    /// <summary>
    /// 몬스터에서 효과음을 재생합니다.
    /// </summary>
    /// <param name="obj">어디서 재생할지 여기에 지정하십시오.</param>
    /// <param name="clip">재생할 효과음을 여기에 지정하십시오.</param>
    public void PlayMonsterEffects(GameObject obj, AudioClip clip)
    {
        obj.GetComponent<AudioSource>().volume = _effectVolume / 100f;
        if (DataManager.instance._data.is3dAudio)
        {
            obj.GetComponent<AudioSource>().spatialBlend = 1f;
        }
        else
        {
            obj.GetComponent<AudioSource>().spatialBlend = 0f;
        }
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<AudioSource>().Play();
    }
}
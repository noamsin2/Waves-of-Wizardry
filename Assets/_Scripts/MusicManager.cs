using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> backgroundMusic_1;

    private const string PLAYER_PREFS_TOTAL_VOLUME = "TotalVolume";
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    private float totalVolume = 0.5f;
    private float musicVolume = 1f;
    int wave = 0;
    int soundIndex = 0;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        totalVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_TOTAL_VOLUME, .50f);
        musicVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .50f);
        UpdateVolume();
    }

    void Start()
    {
       GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }


    public float GetMusicVolume()
    {
        return musicVolume;
    }

    //changes the background music according to the wave
    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsWaveStarted())
        {
            wave++;
            //Debug.Log("wave, soundIndex" + wave + ", " + soundIndex);
            if (wave / 4 != soundIndex)
            {
                Debug.Log(wave / 4);
                soundIndex++;
                audioSource.clip = backgroundMusic_1[soundIndex];
                audioSource.Play();
            }
        }
    }

    private void UpdateVolume()
    {
        audioSource.volume = totalVolume * musicVolume;
    }
    
    public void SetTotalVolume(float volume)
    {
        totalVolume = volume;
        //NO NEED TO SETFLOAT FOR PLAYER PREFS BECAUSE WE ALREADY DO THAT IN SoundManager
        UpdateVolume();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
        UpdateVolume();
    }
    public void PauseMusic()
    {
        audioSource.Pause();
    }
    public void ResumeMusic()
    {
        audioSource.Play();
    }
}

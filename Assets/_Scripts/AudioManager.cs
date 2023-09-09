using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource audioSource;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip resumeSound;
    [SerializeField] private AudioClip equipPendantSound;
    [SerializeField] private AudioClip unequipPendantSound;
    [SerializeField] private AudioClip[] CutSounds;

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    private const string PLAYER_PREFS_TOTAL_VOLUME = "TotalVolume";

    private float totalVolume = 0.5f;
    private float effectsVolume = 1f;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        totalVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_TOTAL_VOLUME, .50f);
        effectsVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, .50f);
        UpdateVolume();
    }
    private void Start()
    {
        GameManager.Instance.OnOpenMenu += GameManager_OnOpenMenu;
        GameManager.Instance.OnCloseMenu += GameManager_OnCloseMenu;
    }

    private void GameManager_OnCloseMenu(object sender, EventArgs e)
    {
        audioSource.clip = resumeSound;
        audioSource.Play();
        MusicManager.Instance.ResumeMusic();
    }

    private void GameManager_OnOpenMenu(object sender, System.EventArgs e)
    {
        audioSource.clip = pauseSound;
        MusicManager.Instance.PauseMusic();
        audioSource.Play();
        
    }

    public void PlayEquip()
    {
        PlayClip(equipPendantSound);
    }
    public void PlayUnequip()
    {
        PlayClip(unequipPendantSound);
    }
    public void PlayRandomCutSound(Vector2 position)
    {
        PlaySound(CutSounds[UnityEngine.Random.Range(0, CutSounds.Length)],position);
    }
    public void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void UpdateVolume()
    {
        audioSource.volume = totalVolume * effectsVolume;
    }
    public void SetTotalVolume(float volume)
    {
        totalVolume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_TOTAL_VOLUME, volume);
        PlayerPrefs.Save();
        UpdateVolume();
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
        UpdateVolume();
    }
    public float GetTotalVolume()
    {
        return totalVolume;
    }
    public float GetEffectsVolume()
    {
        return effectsVolume;
    }

    public float GetFinalVolume()
    {
        return totalVolume * effectsVolume;
    }
    //---Play clip at point functions---
    private void PlaySound(AudioClip clip, Vector2 position)
    {
        Vector3 newPosition = new Vector3(position.x, position.y, 0);
        PlaySound(clip, newPosition, audioSource.volume);
    }
    private void PlaySound(AudioClip clip, Vector2 position, float volume)
    {
        Vector3 newPosition = new Vector3(position.x, position.y, 0);
        PlaySound(clip, newPosition, volume);
    }
    private void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
    //----------------------------------

}

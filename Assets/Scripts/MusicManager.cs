using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;

    //Components
    private AudioSource audioSource;

    //Volume 
    [Range(0f, 1.0f)]
    public static float musicVolume = 1f;
    [Range(0f, 1.0f)]
    public static float soundEffectsVolume = 1f;

    private float fadeStep = 0.05f;

    public bool onMenu = false;

    [Header("Music Tracks")]
    //Music Tracks
    public AudioClip menuTheme;
    public AudioClip fieldTheme;
    public AudioClip battleTheme;
    public AudioClip bossTheme;

    [Header("Sounds Effects")]
    //Sound Effects
    public AudioClip gunShot;

    // Use this for initialization
    void Start ()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();

        if(onMenu)
            audioSource.clip = menuTheme;
        else
            audioSource.clip = fieldTheme;

        audioSource.Play();
    }

    public void playBossMusic()
    {
        audioSource.clip = bossTheme;
        audioSource.Play();
    }

    public void stopBossMusic()
    {
        audioSource.clip = fieldTheme;
        audioSource.Play();
    }

    public void playBattleMusic()
    {
        audioSource.clip = battleTheme;
        audioSource.Play();
    }

    public IEnumerator fadeMusic()
    {
        //Fade in screen
        while (audioSource.volume > 0)
        {
            audioSource.volume -= fadeStep;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void setMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public void setSoundVolume(float volume)
    {
        soundEffectsVolume = volume;
    }

    private void Update()
    {
        audioSource.volume = musicVolume;
    }

}

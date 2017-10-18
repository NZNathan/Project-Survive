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

    //Enemy Variable to control battle music
    private int enemiesOnScreen = 0;

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

    public void stopTrack()
    {
        audioSource.Stop();
    }

    public void playBossMusic()
    {
        audioSource.clip = bossTheme;
        audioSource.Play();
    }

    public void playFieldMusic()
    {
        if (enemiesOnScreen != 0)
            playBattleMusic();
        else
        {
            audioSource.clip = fieldTheme;
            audioSource.Play();
        }
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
        audioSource.volume = musicVolume;
    }

    public void setSoundVolume(float volume)
    {
        soundEffectsVolume = volume;
    }

    public void addEnemy()
    {
        if (enemiesOnScreen == 0)
        {
            playBattleMusic();
        }

        enemiesOnScreen++;
    }

    public void removeEnemy()
    {
        if (enemiesOnScreen >= 0)
        {
            enemiesOnScreen--;

            if (enemiesOnScreen == 0)
            {
                playFieldMusic();
            }
        }
    }

}

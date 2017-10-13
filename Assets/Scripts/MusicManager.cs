using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;

    //Components
    private AudioSource audioSource;

    [Header("Music Tracks")]
    //Music Tracks
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

    // Update is called once per frame
    void Update () {
		
	}
}

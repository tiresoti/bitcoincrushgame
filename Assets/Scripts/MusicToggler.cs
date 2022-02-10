using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggler : MonoBehaviour
{
    public AudioSource playerMusic;
    // private GameObject[] musicPlayers;

    // // Prevents music from being started from the beginning on every scene reload
    // void Awake()
    // {
    //     DontDestroyOnLoad(this.gameObject);
    // }

    // // Destroys new music sources if there are more than one currently
    // void Start()
    // {
    //     musicPlayers = GameObject.FindGameObjectsWithTag("Music");
    //     if(musicPlayers.Length > 1) Destroy(musicPlayers[1]);
    // }

    // The method to toggle between muting and unmuting
    public void ToggleMusic()
        {
            if(!playerMusic.mute) playerMusic.mute = true;
            else playerMusic.mute = false;
        }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggler : MonoBehaviour
{
    public AudioSource playerMusic;

    public void ToggleMusic()
        {
            if(!playerMusic.mute) playerMusic.mute = true;
            else playerMusic.mute = false;
        }
}

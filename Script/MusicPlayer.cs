using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public AudioSource intoroSource, loopSource;

 
    void Start()
    {
        intoroSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + intoroSource.clip.length);
    }
}

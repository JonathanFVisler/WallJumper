using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    GameObject gameManager;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = Memory.MusicVolume;
    }
}

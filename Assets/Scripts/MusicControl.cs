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
        gameManager = GameObject.Find("GameManager");
        audioSource.volume = gameManager.GetComponent<GameManager>().musicVolumne;
    }
}

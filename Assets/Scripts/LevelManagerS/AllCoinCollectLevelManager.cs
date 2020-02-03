using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCoinCollectLevelManager : MonoBehaviour, CoinCollectLevelManagerStrategy
{
    int score = 0;
    float Timer = 0;
    [SerializeField]
    float StartTimer = 60;

    public BeatDegree BeatLevel(){
        return BeatDegree.FAIL;
    }

    public float GetTimeToBeat(){
        return 0;
    }

    public float GetScore(){
        return 0;
    }

    public Dictionary<string, float> GetHighScores(){
        return new Dictionary<string, float>();
    }

    public void PlayLevel(){

    }

    public void CoinPickup(){

    }

    public int GetCoinCount(){
        return 0;
    }
}
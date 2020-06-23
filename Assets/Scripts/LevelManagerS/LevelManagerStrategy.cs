using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LevelManagerStrategy
{
    BeatDegree BeatLevel();

    float GetTimeToBeat();

    float GetScore();

    float GetHighScore();

    void SaveScore();

    void PlayLevel();

    void CoinPickup();

    void EndLevel();

    void FailLevel();

    void DisplayLevelEndMenu();
}

public enum BeatDegree {
    FAIL,
    ONEStar,
    TWOStar,
    THREEStar
};
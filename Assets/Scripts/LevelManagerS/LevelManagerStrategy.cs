using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LevelManagerStrategy
{
    BeatDegree BeatLevel();

    float GetTimeToBeat();

    float GetScore();

    Dictionary<string,float> GetHighScores();

    void PlayLevel();
}

public enum BeatDegree {
    FAIL,
    ONEStar,
    TWOStar,
    THREEStar
};
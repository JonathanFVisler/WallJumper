using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Memory
{
    private static bool inited = false;

    public static float[] GoalMemory
    {
        get;
        private set;
    }

    public static int TotalStarMemory
    {
        get;
        private set;
    }

    public static int AdCounter
    {
        get;
        set;
    }

    public static float MusicVolume
    {
        get;
        set;
    }

    public static float EffectVolume
    {
        get;
        set;
    }

    public static void increaseStars(int amount)
    {
        TotalStarMemory = TotalStarMemory + amount;
        PlayerPrefs.SetInt("TotalStars", TotalStarMemory);
    }
    public static void init()
    {
        if (inited)
        {
            return;
        }

        //Constructor here
        GoalMemory = new float[3];
        AdCounter = 0;
        TotalStarMemory = PlayerPrefs.GetInt("TotalStars");
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        EffectVolume = PlayerPrefs.GetFloat("EffectVolume");

        Debug.Log("Volume: " + MusicVolume);

        if(MusicVolume == 0)
        {
            MusicVolume = 1;
        }

        if(EffectVolume == 0)
        {
            EffectVolume = 0.5f;
        }
        Debug.Log("Volume: " + MusicVolume);

        inited = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour
{

    string storeID = "3480453";

    string videoAd = "video";

    void Start()
    {
        //Advertisement.Initialize(storeID);
    }

    void showSkipableAd()
    { 
        /*if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }*/
    }

    public void IncreaseCounter()
    {
        Memory.AdCounter += 1;
        if(Memory.AdCounter >= 5)
        {
            Memory.AdCounter = 0;
            showSkipableAd();
        }
    }
}

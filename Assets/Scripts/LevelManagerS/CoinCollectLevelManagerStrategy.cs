using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CoinCollectLevelManagerStrategy : LevelManagerStrategy
{
    void CoinPickup();

    int GetCoinCount();
}

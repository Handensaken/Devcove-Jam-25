using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    //Event invoked when the player is forced to switch to brawler
    public event Action OnForcedSwitch;

    public void ForcedSwitch()
    {
        if (OnForcedSwitch != null)
        {
            OnForcedSwitch();
        }
    }
}

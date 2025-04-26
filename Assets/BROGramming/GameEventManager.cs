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

    public event Action OnForcedSwitch;

    public void ForcedSwitch()
    {
        if (OnForcedSwitch != null)
        {
           // Debug.Log("initiating switch event");
            OnForcedSwitch();
        }
    }
    // Update is called once per frame
}

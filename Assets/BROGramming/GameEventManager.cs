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


    //Hitstop 
    public void Stop(float duration)
    {
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
    }

    //Pause for animations. If we wanna play animations, during this time, set their "Update mode" to "Unscaled Time". 
    public void SetTimeScale(float newTimeScale)
    {
        //Stops if there are any current hitstops from starting the game again
        StopAllCoroutines();
        Time.timeScale = newTimeScale;
    }
}

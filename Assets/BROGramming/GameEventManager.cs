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

    public event Action OnResumeCamControl;

    public void ResumeCamControl()
    {
        if (OnResumeCamControl != null)
        {
            OnResumeCamControl();
        }
    }

    public event Action OnStopPlayerInput;

    public void StopPlayerInput()
    {
        if (OnStopPlayerInput != null)
        {
            OnStopPlayerInput();
        }
    }

    public event Action OnActivatePlayerInput;

    public void ActivatePlayerInput()
    {
        if (OnActivatePlayerInput != null)
        {
            OnActivatePlayerInput();
        }
    }

    public event Action OnFightStart;

    public void FightStart()
    {
        if (OnFightStart != null)
        {
            OnFightStart();
        }
    }

    public event Action OnFightEnd;

    public void FightEnd()
    {
        if (OnFightEnd != null)
        {
            OnFightEnd();
        }
    }

    public event Action<float> OnScreenShake;

    public void ScreenShake(float duration)
    {
        if (OnScreenShake != null)
        {
            OnScreenShake(duration);
        }
    }

    public event Action<float> OnPlayerHurt;

    public void PlayerHurt(float f)
    {
        if (OnPlayerHurt != null)
        {
            OnPlayerHurt(f);
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

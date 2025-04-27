using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        GameEventManager.instance.OnScreenShake += Cunt;
        cam = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    void OnDisable()
    {
        GameEventManager.instance.OnScreenShake -= Cunt;
    }

    public void Cunt(float duration)
    {
        //Debug.Log("it do");
        StartCoroutine(shaking(duration));
    }

    private IEnumerator shaking(float fuckiwuki)
    {
        var v = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        v.m_AmplitudeGain = 0.3f;
        v.m_FrequencyGain = 10f;
        yield return new WaitForSeconds(fuckiwuki);
        v.m_AmplitudeGain = 0f;
        v.m_FrequencyGain = 0f;
    }

    // Update is called once per frame
    void Update() { }
}

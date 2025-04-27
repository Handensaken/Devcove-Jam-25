using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField]
    private RenderTexture BaseTexture;

    [SerializeField]
    private Volume pp;

    private ChromaticAberration cA;
    public bool lowRes { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        lowRes = false;
        BaseTexture.useDynamicScale = true;
        pp.profile.TryGet(out cA);
    }

    void OnDisable()
    {
        cA.active = false;
        //Debug.Log("High Res");
        ScalableBufferManager.ResizeBuffers(1f, 1f);
        lowRes = false;

    }

    // Update is called once per frame
    void Update() { }

    //changes the resolution of the render texture after an input
    public void GetChangeResolutionInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.inProgress)
        {
            // ChangeResolution();
        }
    }

    public void ChangeResolution()
    {
        if (!lowRes)
        {
            //lowers resolution and activates chromatic aberration
            ScalableBufferManager.ResizeBuffers(0.15f, 0.15f);
            lowRes = true;
            cA.active = true;
        }
        else
        {
            //deactivates chromatic aberration and resets texture resolution
            cA.active = false;
            ScalableBufferManager.ResizeBuffers(1f, 1f);
            lowRes = false;
        }
    }
}

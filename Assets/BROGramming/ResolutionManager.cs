using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField]
    private RenderTexture BaseTexture;

    [SerializeField]
    private Volume pp;

    private ChromaticAberration cA;
    private bool lowRes;

    // Start is called before the first frame update
    void Start()
    {
        lowRes = false;
        BaseTexture.useDynamicScale = true;
        pp.profile.TryGet(out cA);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!lowRes)
            {
                ScalableBufferManager.ResizeBuffers(0.1f, 0.1f);
                Debug.Log(
                    $"Width {ScalableBufferManager.widthScaleFactor} | Height {ScalableBufferManager.heightScaleFactor}"
                );
                lowRes = true;
                cA.active = true;
            }
            else
            {
                cA.active = false;
                Debug.Log("High Res");
                ScalableBufferManager.ResizeBuffers(1f, 1f);
                lowRes = false;
            }
        }
    }
}

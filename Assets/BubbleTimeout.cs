using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTimeout : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    private float fuck = 0;

    // Update is called once per frame
    void Update()
    {
        fuck += Time.deltaTime;

        if (fuck > 5)
        {
            gameObject.SetActive(false);
        }
    }
}

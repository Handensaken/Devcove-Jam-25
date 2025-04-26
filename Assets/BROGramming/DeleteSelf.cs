using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{
    public float timeBeforeDelete;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeBeforeDelete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

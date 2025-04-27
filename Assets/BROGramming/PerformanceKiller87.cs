using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceKiller87 : MonoBehaviour
{
    private int fireballcount = 0;
    public void FireballSpawned(GameObject newfireball)
    {
        
        if (fireballcount > 0)
        {
            Destroy(newfireball);
        } else fireballcount++;
    }
    public void FireballDied()
    {
        fireballcount++;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

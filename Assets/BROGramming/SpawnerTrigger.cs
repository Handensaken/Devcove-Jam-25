using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
   public GameObject player;
    public GameObject spawner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        else
            if (collision.gameObject == player)
            spawner.GetComponent<EnemySpawner>().Activate();
       
    }

    
}

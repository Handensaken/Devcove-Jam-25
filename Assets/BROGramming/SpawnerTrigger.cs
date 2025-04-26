using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public GameObject spawner;
    public GameObject spelare;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTERED");
        if (collision.gameObject == spelare)
        {
            Debug.Log("is player");
            spawner.GetComponent<EnemySpawner>().Activate();
        }
    }
}

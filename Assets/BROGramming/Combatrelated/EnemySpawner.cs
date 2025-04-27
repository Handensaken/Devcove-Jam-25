using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<int> EnemyWaveCount = new List<int>();
    [SerializeField] GameObject enemy;
    [SerializeField] List<Transform> spawnlocations = new List<Transform>();

    private bool shouldSpawn = false;
   
    private int currentwave = 0;
    private int enemiesAlive = 0;
    
    // Start is called before the first frame update
    void Start()
    {
    //    Activate();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldSpawn)
        {
            for (int i = 0; i < EnemyWaveCount[currentwave]; i++)
            {
                SpawnEnemy();
            }
            shouldSpawn = false;
        }
    }

    private void SpawnEnemy()
    {
        int spawnplace = Random.Range(0, spawnlocations.Count);
        Debug.Log(spawnlocations.Count);
        Debug.Log(spawnplace);
        GameObject enemyyy = Instantiate(enemy, spawnlocations[spawnplace].position, Quaternion.identity);
        enemyyy.GetComponent<EnemyHealth>().AddConnectedRoom(gameObject);
        enemiesAlive++;
        
    }

    public void RemoveEnemy()
    {
        enemiesAlive--;
        if (enemiesAlive == 0 && !shouldSpawn)
        {
            if (currentwave < EnemyWaveCount.Count)
            {
                NewWave();
            } else
            {
                GameEventManager.instance.FightEnd();
                Destroy(gameObject);
            }
        }
    }

    private void NewWave()
    {
        currentwave++;
        shouldSpawn = true;
       
    }

    //Denna m�ste kallas f�r att starta ig�ng rummet
    public void Activate()
    {
        Debug.Log("ACTIVATE");
        shouldSpawn = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<int> EnemyWaveCount = new List<int>();
    [SerializeField] GameObject enemy;
    [SerializeField] List<Transform> spawnlocations = new List<Transform>();
    [SerializeField] float timeBetweenWaves = 5f;

    private bool shouldSpawn = false;
    private float timePassed = 0;
    private int currentwave = 0;
    private int enemiesAlive = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Wait();
    //    Activate();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed < timeBetweenWaves)
        {

        } else if (shouldSpawn)
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
        enemiesAlive++;
        Debug.Log("me when enemy live" + enemiesAlive);
        int spawnplace = Random.Range(0, spawnlocations.Count);
        GameObject enemyyy = Instantiate(enemy, spawnlocations[spawnplace].position, Quaternion.identity);
        enemyyy.GetComponent<EnemyHealth>().AddConnectedRoom(gameObject);
        
        
    }

    public void RemoveEnemy()
    {

        enemiesAlive--;
        Debug.Log("me when enemy ded" + enemiesAlive);
        if (enemiesAlive == 0 && !shouldSpawn)
        {
            Debug.Log("currentwave: " + currentwave + " enemywave.count: " + EnemyWaveCount.Count);
            currentwave++;
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
        Wait();
        shouldSpawn = true;
       
    }

    //Denna m�ste kallas f�r att starta ig�ng rummet
    public void Activate()
    {
        shouldSpawn = true;
    }

    private void Wait()
    {
        timePassed = 0;
    }
}

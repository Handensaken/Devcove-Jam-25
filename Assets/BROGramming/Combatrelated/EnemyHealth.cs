using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    
    [SerializeField] float maxHealth = 100f;
    [SerializeField] GameObject deathVFX;
    private float currentHealth;

    private GameObject connectedRoom;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void Damage(float damageamount)
    {
        currentHealth -= damageamount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (deathVFX != null)
        {
            Instantiate(deathVFX);
        }
        if (connectedRoom != null) connectedRoom.GetComponent<EnemySpawner>().RemoveEnemy();
        Destroy(gameObject);
    }

    public void AddConnectedRoom(GameObject spawner)
    {
        connectedRoom = spawner;
    }
}

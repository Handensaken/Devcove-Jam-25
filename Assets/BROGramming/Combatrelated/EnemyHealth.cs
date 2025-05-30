using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    
    [SerializeField] float maxHealth = 100f;
    [SerializeField] GameObject deathVFX;
    private float currentHealth;

    private DamageFlash _damageFlash;

    private GameObject connectedRoom;
    public Action onDamageTaken;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        _damageFlash = GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void Damage(float damageamount)
    {
        onDamageTaken?.Invoke();
        currentHealth -= damageamount;

        _damageFlash.CallDamageFlash();

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
        EnemyAI.enemyAttacking.Remove(gameObject);
        Destroy(gameObject);
    }

    public void AddConnectedRoom(GameObject spawner)
    {
        connectedRoom = spawner;
    }
}

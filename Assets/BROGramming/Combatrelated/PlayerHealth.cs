using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    private float currentHealth;
    public UnityEvent onDeath;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        GameEventManager.instance.OnPlayerHurt += Damage;
    }
    void OnDisable()
    {
        GameEventManager.instance.OnPlayerHurt -= Damage;
        
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
        Debug.Log("Player ded");
        onDeath?.Invoke();
    }
}

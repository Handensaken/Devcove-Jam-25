using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHurtbox : MonoBehaviour
{
    public float damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //I want to invoke event so we can add a listener to a UI script without referencing shit constantly
            //collision.GetComponent<PlayerHealth>().Damage(damage);
            GameEventManager.instance.PlayerHurt(damage);
        }
        //Check if it is player and do damage if it is type beat
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHurtbox : MonoBehaviour
{
    public float damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if it is player and do damage if it is type beat
    }
}

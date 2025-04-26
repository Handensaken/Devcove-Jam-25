using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fistBehaviour : MonoBehaviour
{
    [SerializeField] float minDamage;
    [SerializeField] float maxDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out _))
        {
            float random = Random.Range(minDamage, maxDamage);
            collision.gameObject.GetComponent<IDamageable>().Damage(random);
            Destroy(gameObject);
        }
    }
}

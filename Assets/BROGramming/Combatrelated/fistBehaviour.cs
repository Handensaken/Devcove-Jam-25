using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fistBehaviour : MonoBehaviour
{
    [SerializeField] float minDamage =20;
    [SerializeField] float maxDamage =30;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out _))
        {
            Debug.Log("Found enemy hit");
            float random = Random.Range(minDamage, maxDamage);
            collision.gameObject.GetComponent<IDamageable>().Damage(random);
            GameEventManager.instance.Stop(0.1f + random / 100);
            //Destroy(gameObject);
        }
    }
}

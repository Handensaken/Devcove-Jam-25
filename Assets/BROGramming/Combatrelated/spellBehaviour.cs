using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private List<GameObject> damagedobjects = new List<GameObject>();

    [SerializeField] float movespeed;
    [SerializeField] float damage;
    // Start is called before the first frame update
    void Start()
    {
        damagedobjects.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
        bool facingright = transform.Find("Player").gameObject.GetComponent<PlayerController>().isFacingRight;
        if (facingright)
        {
            rb.velocity = transform.right * movespeed;
        }
        else rb.velocity = -transform.right * movespeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TryGetComponent<IDamageable>(out IDamageable idmg))
        {
            bool shouldDamage = true;
            foreach (GameObject gameObject in damagedobjects)
            {
                if (gameObject == collision.gameObject)
                {
                    shouldDamage = false;
                }
            }

            if (shouldDamage){
                idmg.Damage(damage);
                damagedobjects.Add(collision.gameObject);
            }
        }
    }
}

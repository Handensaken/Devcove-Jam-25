using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    private List<GameObject> damagedobjects = new List<GameObject>();
    [SerializeField] GameObject player;
    [SerializeField] float movespeed = 5;
    [SerializeField] float damage = 10f;
    private bool hasNerfed = false;
    // Start is called before the first frame update
    void Start()
    {
        
        damagedobjects.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
        bool facingright = player.GetComponent<PlayerController>().getfacingright();
      
      /*  if (facingright)
        {
            rb.velocity = transform.right * movespeed;
        } */
        
    }

    // Update is called once per frame
    void Update()
    {



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<spellBehaviour>(out _))
        {
            hasNerfed = true;
            damage = damage / 2;
            Debug.Log("Shits myself");
        }

        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable idmg))
        {
          
            bool shouldDamage = true;
            foreach (GameObject gameObject in damagedobjects)
            {
                if (gameObject == collision.gameObject)
                {
                    shouldDamage = false;
                }
            }

            if (shouldDamage)
            {
                idmg.Damage(damage);
                damagedobjects.Add(collision.gameObject);
                Debug.Log("I did ouchie to meanie " + collision.gameObject.name);
            }
        }
    }
    public void ShootLeft()
    {

        rb.velocity = -transform.right * movespeed;
    }

    public void ShootRight()
    {
       
        rb.velocity = transform.right * movespeed;
    }
}

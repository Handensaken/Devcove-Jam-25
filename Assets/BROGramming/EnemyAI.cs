using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 1;

    [Header("References")]
    public Animator anim;
    public Rigidbody2D rb;

    private GameObject player;
    private float distanceToPlayer = 0;
    private float sideOfPlayerSign;
    private enemyState state = enemyState.angry;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var meMinusPlayer = transform.position - player.transform.position;
        distanceToPlayer = Vector2.SqrMagnitude(meMinusPlayer);
        sideOfPlayerSign = Mathf.Sign(meMinusPlayer.x);

        if(state == enemyState.idle)
        {

        }

        else if (state == enemyState.angry)
        {
            //Set animation
            var targetPosition = player.transform.position + new Vector3(sideOfPlayerSign * attackRange - 0.1f, 0);
            targetPosition.z = 0;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if(distanceToPlayer <= attackRange && Mathf.Abs(meMinusPlayer.y) < Mathf.Abs(meMinusPlayer.x))
            {
                anim.SetTrigger("Attack");
            }
        }

        else if(state == enemyState.attacking)
        {
            //trollol
        }

        else if (state == enemyState.fleeing)
        {
            Debug.Log("Jag flyr nu reeee");
        }
    }

    public void AttackEnded()
    {
        state = enemyState.fleeing;
    }
}

public enum enemyState
{
    idle,
    angry,
    attacking,
    fleeing
}
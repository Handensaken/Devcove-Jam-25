using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 1;
    private float sqrAttackRange;

    [Header("References")]
    public Animator anim;
    public Rigidbody2D rb;

    private GameObject player;
    private float distanceToPlayer = 0;
    private float negativeSideOfPlayerSign;
    private enemyState state = enemyState.angry;

    private void OnValidate()
    {
        sqrAttackRange = attackRange * attackRange;
    }

    private void Awake()
    {
        sqrAttackRange = attackRange * attackRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    //TODO: Allt. 
    void Update()
    {
        var meMinusPlayer = transform.position - player.transform.position;
        distanceToPlayer = Vector2.SqrMagnitude(meMinusPlayer);
        negativeSideOfPlayerSign = Mathf.Sign(meMinusPlayer.x);

        if(state == enemyState.idle)
        {
            anim.SetBool("Walking", false);
        }

        else if (state == enemyState.angry)
        {
            anim.SetBool("Walking", true);

            var targetPosition = player.transform.position + new Vector3(negativeSideOfPlayerSign * attackRange - 0.1f, 0);
            targetPosition.z = 0;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if(distanceToPlayer <= sqrAttackRange && Mathf.Abs(meMinusPlayer.y) < Mathf.Abs(meMinusPlayer.x))
            {
                anim.SetTrigger("Attack");
                anim.SetBool("Walking", false);
                state = enemyState.attacking;
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
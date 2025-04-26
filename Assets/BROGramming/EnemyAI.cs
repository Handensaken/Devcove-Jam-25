using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static bool enemyAttacking = false;
    public float speed = 3f;
    public float attackRange = 1;
    private float sqrAttackRange;

    [SerializeField]
    float fleeDistance = 1f;

    [Header("References")]
    public Animator anim;
    public Rigidbody2D rb;
    public BoxCollider2D boxcollider;

    private GameObject player;
    private float distanceToPlayer = 0;
    private float negativeSideOfPlayerSign;
    private enemyState state = enemyState.idle;

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

        if (state == enemyState.idle)
        {
            anim.SetBool("Walking", false);
            if (!EnemyAI.enemyAttacking && canAttack)
            {
                state = enemyState.angry;
            }
        }
        else if (state == enemyState.angry)
        {
            EnemyAI.enemyAttacking = true;
            anim.SetBool("Walking", true);

            var targetPosition =
                player.transform.position
                + new Vector3(negativeSideOfPlayerSign * attackRange - 0.1f, 0);
            targetPosition.z = 0;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            if (
                distanceToPlayer <= sqrAttackRange
                && Mathf.Abs(meMinusPlayer.y) < Mathf.Abs(meMinusPlayer.x)
            )
            {
                anim.SetTrigger("Attack");
                anim.SetBool("Walking", false);
                state = enemyState.attacking;
            }
        }
        else if (state == enemyState.attacking)
        {
            //here there should probably be some kind of wind up, could wait 0.25s before activating this, but for now just activating it is good enough
            // boxcollider.gameObject.SetActive(true);
        }
        else if (state == enemyState.fleeing)
        {
            EnemyAI.enemyAttacking = false;
            anim.SetBool("Walking", true);
            //  Vector3 randomCircle = -dir.normalized * fleeDistance;
            //boxcollider.gameObject.SetActive(false);
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetFleePosition,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetFleePosition) < 0.1f)
            {
                state = enemyState.idle;
                transform.localScale = new Vector3(1, 1, 1);
                StartCoroutine(wait());
            }
        }
    }

    bool canAttack = true;

    private IEnumerator wait()
    {
        canAttack = false;
        yield return new WaitForSeconds(2);
        {
            canAttack = true;
        }
    }

    public void ActivateHurtBox(int i)
    {
        if (i == 1)
        {
            boxcollider.gameObject.SetActive(true);
        }
        else
        {
            boxcollider.gameObject.SetActive(false);
        }
    }

    Vector3 targetFleePosition;

    public void AttackEnded()
    {
        Vector3 dir = transform.position - player.transform.position;
        transform.localScale = new Vector3(-1, 1, 1);
        state = enemyState.fleeing;
        targetFleePosition = player.transform.position + dir * fleeDistance;
    }
}

public enum enemyState
{
    idle,
    angry,
    attacking,
    fleeing
}

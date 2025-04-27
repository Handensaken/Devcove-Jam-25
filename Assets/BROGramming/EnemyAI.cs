using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 1;
    private float sqrAttackRange;
    public float idleDetectionRange = 1;
    private float sqrIdleDetectionRange;

    [SerializeField]
    float fleeDistance = 1f;
    public Vector2 yBounds;

    [Header("References")]
    public Animator anim;
    public Rigidbody2D rb;
    public BoxCollider2D boxcollider;
    public EnemyHealth enemyHealth;

    private GameObject player;
    private float sqrDistanceToPlayer = 0;
    private float negativeSideOfPlayerSign;
    private enemyState state = enemyState.idle;
    private float fleeingTimer;

    Vector3 targetFleePosition;
    public static HashSet<GameObject> enemyAttacking = new HashSet<GameObject>();


    private void OnValidate()
    {
        sqrAttackRange = attackRange * attackRange;
        sqrIdleDetectionRange = idleDetectionRange * idleDetectionRange;
    }

    private void Awake()
    {
        sqrAttackRange = attackRange * attackRange;
        sqrIdleDetectionRange = idleDetectionRange * idleDetectionRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    private void OnEnable()
    {
        enemyHealth.onDamageTaken += DamageTaken;
    }

    private void OnDisable()
    {
        enemyHealth.onDamageTaken += DamageTaken;
    }

    //TODO: Allt.
    void Update()
    {
        var meMinusPlayer = transform.position - player.transform.position;
        sqrDistanceToPlayer = Vector2.SqrMagnitude(meMinusPlayer);
        negativeSideOfPlayerSign = Mathf.Sign(meMinusPlayer.x);

        if (state == enemyState.idle)
        {
            anim.SetBool("Walking", false);
            if (EnemyAI.enemyAttacking.Count < 1 || sqrDistanceToPlayer < sqrIdleDetectionRange)
            {
                state = enemyState.angry;
                EnemyAI.enemyAttacking.Add(gameObject);
            }
        }
        else if (state == enemyState.angry)
        {
            transform.localScale = new Vector3(negativeSideOfPlayerSign, 1, 1);
            anim.SetBool("Walking", true);

            var targetPosition =
                player.transform.position
                + new Vector3(negativeSideOfPlayerSign * (attackRange - 0.1f), 0);
            targetPosition.z = 0;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            if (
                sqrDistanceToPlayer <= sqrAttackRange
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
            transform.localScale = new Vector3(Mathf.Sign(transform.position.x - targetFleePosition.x), 1, 1);
            fleeingTimer += Time.deltaTime;
            anim.SetBool("Walking", true);
            transform.position = Vector3.MoveTowards(transform.position, targetFleePosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetFleePosition) < 0.1f || fleeingTimer >= 1.4f)
            {
                state = enemyState.idle;
            }
        }
    }

    private void DamageTaken()
    {
        Debug.Log("Hej");
        if(state == enemyState.attacking || state == enemyState.fleeing)
        {
            state = enemyState.angry;
            anim.SetTrigger("ForceAngry");
            anim.SetBool("Walking", true);
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
    

    public void AttackEnded()
    {
        Vector3 dir = transform.position - player.transform.position;
        state = enemyState.fleeing;
        targetFleePosition = player.transform.position + dir * fleeDistance;
        if (targetFleePosition.y > yBounds.x) targetFleePosition.y = yBounds.x;
        if (targetFleePosition.y < yBounds.y) targetFleePosition.y = yBounds.y;
        EnemyAI.enemyAttacking.Remove(gameObject);
        fleeingTimer = 0;

        //TODO: if last one, go into angry state instead. 
    }
}

public enum enemyState
{
    idle,
    angry,
    attacking,
    fleeing
}

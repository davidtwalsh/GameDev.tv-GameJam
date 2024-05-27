using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private UnitState state = UnitState.Wandering;
    [SerializeField]
    private float moveSpeed = .5f;
    [SerializeField]
    private float wanderDistance = 3f;

    private bool isMoving = false;
    private Vector2 moveTarget;

    private Rigidbody2D rb;

    private float checkMonsterDistTimer = 0f;
    private float newMoveTargetTimer = 0f;

    [SerializeField]
    private float attackRange = 3f;

    private GameObject attackTarget;

    private IAttacker myAttacker;
    private bool startedAttack = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAttacker = GetComponent<IAttacker>();
    }

    void Update()
    {
        switch (state)
        {
            case UnitState.Wandering:

                if (isMoving == false) //need to find a new point to wander to
                {
                    float x = Random.Range(transform.position.x - wanderDistance, transform.position.x + wanderDistance);
                    float y = Random.Range(transform.position.y - wanderDistance, transform.position.y + wanderDistance);
                    moveTarget = new Vector3(x, y);
                    isMoving = true;
                }
                if (moveTarget != null && CalculateDistance(transform.position.x,transform.position.y,moveTarget.x,moveTarget.y) < .2f) // pick a new wander target
                {
                    isMoving = false;
                }

                newMoveTargetTimer += Time.deltaTime;
                if (newMoveTargetTimer > 3f)
                {
                    isMoving = false;
                    newMoveTargetTimer = 0f;
                }

                //Periodically check monsters to see if one comes in range (pick closest one)
                checkMonsterDistTimer += Time.deltaTime;
                if (checkMonsterDistTimer > .5f)
                {
                    GameObject closestMonsterInRange = null;
                    float minDst = 99999f;
                    foreach (GameObject monster in SpawnController.Instance.GetMonsters())
                    {
                        float dstToMonster = CalculateDistance(transform.position.x,transform.position.y,monster.transform.position.x,monster.transform.position.y);
                        if (dstToMonster < attackRange && dstToMonster < minDst)
                        {
                            closestMonsterInRange = monster;
                            minDst = dstToMonster;
                        }
                    }
                    checkMonsterDistTimer = 0f;
                    if (closestMonsterInRange != null)
                    {
                        isMoving = false;
                        attackTarget = closestMonsterInRange;
                        state = UnitState.Attacking;
                        break;
                    }
                }
                break;

            case UnitState.Attacking:

                if (startedAttack == false)
                {
                    myAttacker.BeginAttacking(attackTarget);
                    startedAttack = true;
                }
                if (attackTarget != null)
                {
                    if (transform.position.x >= attackTarget.transform.position.x)
                    {
                        transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    }
                }
                if (myAttacker.IsDoneAttacking() == true || attackTarget == null)
                {
                    startedAttack = false;
                    myAttacker.CleanUpAttack();
                    state = UnitState.Wandering;
                    break;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (isMoving == true && moveTarget != null)
        {
            Vector2 direction = (moveTarget - rb.position).normalized;

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            }

            // Move towards the target point
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            
        }
    }

    private float CalculateDistance(float x1, float y1, float x2, float y2)
    {
        // Calculate the squared differences
        float dx = x2 - x1;
        float dy = y2 - y1;
        float squaredDistance = dx * dx + dy * dy;

        // Return the square root of the squared distance
        return Mathf.Sqrt(squaredDistance);
    }
}

public enum UnitState
{
    Wandering,
    Attacking
}

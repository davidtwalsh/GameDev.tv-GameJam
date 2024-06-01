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

    [SerializeField]
    private float maxWanderDistance = 5f;

    private Vector3 originalPosition;

    private bool isInTower = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAttacker = GetComponent<IAttacker>();
        originalPosition = transform.position;
    }

    void Update()
    {
        switch (state)
        {
            case UnitState.Wandering:

                if (isMoving == false && isInTower == false) //need to find a new point to wander to
                {
                    int maxAttempts = 25;
                    for (int i = 0; i < maxAttempts; i++)
                    {
                        float x = Random.Range(transform.position.x - wanderDistance, transform.position.x + wanderDistance);
                        float y = Random.Range(transform.position.y - wanderDistance, transform.position.y + wanderDistance);
                        if (MathHelper.CalculateDistance(x,y,originalPosition.x,originalPosition.y) < maxWanderDistance)
                        {
                            moveTarget = new Vector3(x, y);
                            isMoving = true;
                            break;
                        }
                    }
                }
                if (moveTarget != null && MathHelper.CalculateDistance(transform.position.x,transform.position.y,moveTarget.x,moveTarget.y) < .2f) // pick a new wander target
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
                    float curAttackRange = attackRange;
                    if (isInTower == true)
                    {
                        curAttackRange += UpgradeController.Instance.GetTowerRangeBonus();
                    }
                    GameObject monster = myAttacker.GetAttackTarget(curAttackRange);
                    checkMonsterDistTimer = 0f;
                    if (monster != null)
                    {
                        isMoving = false;
                        attackTarget = monster;
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

    private void LateUpdate()
    {
        if (isInTower == true)
        {
            rb.velocity = Vector2.zero;
        }
    }
    public void SetIsInTower(bool inTower)
    {
        isInTower = inTower;

    }
}

public enum UnitState
{
    Wandering,
    Attacking
}

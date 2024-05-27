using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class Enemy : MonoBehaviour
{
    private EnemyState state = EnemyState.FindingTarget;
    private GameObject target = null;

    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float attackDistance = 2f;

    [SerializeField]
    private Animator weaponAnimator;

    void Update()
    {
        float dstToTarget;
        //Switch State
        switch (state)
        {
            case EnemyState.FindingTarget:
                bool foundTarget = FindClosestTarget();
                if (foundTarget)
                {
                    state = EnemyState.MovingToTarget;
                }
                break;
            case EnemyState.MovingToTarget:
                //move in fixed update
                if (target != null)
                {
                    dstToTarget = CalculateDistance(transform.position.x, transform.position.y, target.transform.position.x, target.transform.position.y);
                    if (dstToTarget < attackDistance)
                    {
                        rb.velocity = Vector3.zero;
                        state = EnemyState.Attacking;
                        break;
                    }
                }
                else //lost target so get a new one
                {
                    state = EnemyState.FindingTarget;
                }

                break;
            case EnemyState.Attacking:
                if (target == null)
                {
                    weaponAnimator.SetBool("isAttacking", false);
                    state = EnemyState.FindingTarget;
                    break;
                }
                if (weaponAnimator != null)
                {
                    weaponAnimator.SetBool("isAttacking", true);
                }
                dstToTarget = CalculateDistance(transform.position.x, transform.position.y, target.transform.position.x, target.transform.position.y);
                if (dstToTarget > attackDistance + .2f)
                {
                    weaponAnimator.SetBool("isAttacking", false);
                    state = EnemyState.FindingTarget;
                    break;
                }
                break;
        }
    }

    private bool FindClosestTarget()
    {
        GameObject closestPlayerAttackable = null;
        float curClosestDst = 999999f;
        foreach (GameObject attackable in ObjectPlacer.Instance.GetPlayerAttackables())
        {
            float xx = attackable.transform.position.x;
            float yy = attackable.transform.position.y;

            float myX = transform.position.x;
            float myY = transform.position.y;

            float dstToAttackable = CalculateDistance(xx, yy, myX, myY);
            if (dstToAttackable < curClosestDst)
            {
                curClosestDst = dstToAttackable;
                closestPlayerAttackable = attackable;
            }
        }
        if (closestPlayerAttackable != null)
        {
            target = closestPlayerAttackable;
            return true;
        }
        else
        {
            return false;
        }

    }

    void FixedUpdate()
    {
        if (state == EnemyState.MovingToTarget && target != null)
        {
            Transform targetPoint = target.transform;
            if (targetPoint != null)
            {
                // Calculate the direction vector towards the target point
                Vector2 targetPos = new Vector2(targetPoint.position.x, targetPoint.position.y);
                Vector2 direction = (targetPos - rb.position).normalized;

                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1f,transform.localScale.y,transform.localScale.z);
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                }

                // Move towards the target point
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
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

    public GameObject getTarget()
    {
        return target;
    }
}

public enum EnemyState
{
    FindingTarget,
    MovingToTarget,
    Attacking
}
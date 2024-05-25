using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyState state = EnemyState.Waiting;
    private GameObject target = null;

    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float attackDistance = 2f;

    void Update()
    {
        //Switch State
        switch (state)
        {
            case EnemyState.Waiting:
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
                    float dstToTarget = CalculateDistance(transform.position.x, transform.position.y, target.transform.position.x, target.transform.position.y);
                    if (dstToTarget < attackDistance)
                    {
                        state = EnemyState.Attacking;
                    }
                }

                break;
            case EnemyState.Attacking:

                break;
        }
    }

    private bool FindClosestTarget()
    {
        Wall closestWall = null;
        float curClosestDst = 999999f;
        foreach ((int,int) item in MapMaker.Instance.walls.Keys)
        {
            float wallX = item.Item1;
            float wallY = item.Item2;

            float myX = transform.position.x;
            float myY = transform.position.y;

            float dstToWall = CalculateDistance(wallX, wallY, myX, myY);
            if (dstToWall < curClosestDst)
            {
                curClosestDst = dstToWall;
                closestWall = MapMaker.Instance.walls[item];
            }
        }
        if (closestWall != null)
        {
            target = closestWall.gameObject;
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
}

public enum EnemyState
{
    Waiting,
    MovingToTarget,
    Attacking
}

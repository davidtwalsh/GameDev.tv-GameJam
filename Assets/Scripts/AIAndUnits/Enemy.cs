using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

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

    private Vector3 spawnPosition;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    private bool isWandering = false;
    Vector2 wanderTarget;
    private float newWanderTargetTimer = 0f;

    [SerializeField]
    private int coinDropMin;
    [SerializeField]
    private int coinDropMax;

    [SerializeField]
    private AudioClip deathSound;

    AudioSource audioSource;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        spawnPosition = transform.position;
    }

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
                    dstToTarget = MathHelper.CalculateDistance(transform.position.x, transform.position.y, target.transform.position.x, target.transform.position.y);
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
                dstToTarget = MathHelper.CalculateDistance(transform.position.x, transform.position.y, target.transform.position.x, target.transform.position.y);
                if (dstToTarget > attackDistance + .2f)
                {
                    weaponAnimator.SetBool("isAttacking", false);
                    state = EnemyState.FindingTarget;
                    break;
                }
                break;
            case EnemyState.Fleeing:
                weaponAnimator.SetBool("isAttacking", false);
                break;
            case EnemyState.Polymorphed:

                if (isWandering == false) //need to find a new point to wander to
                {
                    float x = Random.Range(transform.position.x - 3, transform.position.x + 3);
                    float y = Random.Range(transform.position.y - 3, transform.position.y + 3);
                    wanderTarget = new Vector3(x, y);
                    isWandering = true;
                }
                if (wanderTarget != null && MathHelper.CalculateDistance(transform.position.x, transform.position.y, wanderTarget.x, wanderTarget.y) < .2f) // pick a new wander target
                {
                    isWandering = false;
                }

                newWanderTargetTimer += Time.deltaTime;
                if (newWanderTargetTimer > 3f)
                {
                    isWandering = false;
                    newWanderTargetTimer = 0f;
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

            float dstToAttackable = MathHelper.CalculateDistance(xx, yy, myX, myY);
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
        else if (state == EnemyState.Fleeing)
        {
            Vector2 targetPos = new Vector2(spawnPosition.x, spawnPosition.y);
            Vector2 direction = (targetPos - rb.position).normalized;

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
        else if (state == EnemyState.Polymorphed)
        {
            if (isWandering == true && wanderTarget != null)
            {
                Vector2 direction = (wanderTarget - rb.position).normalized;

                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                }

                // Move towards the target point
                rb.MovePosition(rb.position + direction * .3f * Time.fixedDeltaTime);

            }
        }
    }

    public GameObject getTarget()
    {
        return target;
    }

    public void SetState(EnemyState state)
    {
        this.state = state;
    }

    public void SetPolymorphed()
    {
        if (audioSource != null && state != EnemyState.Polymorphed)
        {
            audioSource.clip = SpriteController.Instance.sheepClip;
            audioSource.Play();
        }
        weaponAnimator.gameObject.SetActive(false);
        spriteRenderer.sprite = SpriteController.Instance.GetSheepSprite();
        SetState(EnemyState.Polymorphed);
    }

    public void EndPolymorph()
    {
        weaponAnimator.gameObject.SetActive(true);
        spriteRenderer.sprite = originalSprite;
        SetState(EnemyState.FindingTarget);
    }

    public void SetOriginalSprite()
    {
        spriteRenderer.sprite = originalSprite;
    }

    public void DropCoins()
    {
        int coinsToDrop = Random.Range(coinDropMin, coinDropMax+1);
        for (int i = 0; i < coinsToDrop; i++)
        {
            GameObject coinObj = Instantiate(ResourceController.Instance.GetCoinPrefab(),transform.position,Quaternion.identity);
            Coin coin = coinObj.GetComponent<Coin>();
            if (coin != null)
            {
                ResourceController.Instance.GetGroundCoins().Add(coin);
            }
        }
    }

    public AudioClip GetDeathSound()
    {
        return deathSound;
    }
}

public enum EnemyState
{
    FindingTarget,
    MovingToTarget,
    Attacking,
    Fleeing,
    Polymorphed
}

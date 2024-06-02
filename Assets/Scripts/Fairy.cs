using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    private FairyState state;
    private bool hasWanderTarget = false;
    private Vector2 target;

    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed;

    private float coinCheckTimer = 0f;

    private Coin seekingCoin;

    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        state = FairyState.Wandering;
    }

    private void Update()
    {
        switch (state) {

            case FairyState.Wandering:

                if (hasWanderTarget == false)//find a new wander point
                {
                    float x = Random.Range(1f, 23f);
                    float y = Random.Range(1f, 15f);
                    target = new Vector2(x, y);
                    hasWanderTarget = true;
                }
                if (hasWanderTarget && MathHelper.CalculateDistance(transform.position.x, transform.position.y, target.x, target.y) < .2f) // pick a new wander target
                {
                    hasWanderTarget = false;
                }

                //periodically check for coins
                coinCheckTimer += Time.deltaTime;
                if (coinCheckTimer > .3f)
                {
                    coinCheckTimer = 0f;
                    Coin coin = CheckCoins();
                    if (coin != null)
                    {
                        target = coin.transform.position;
                        state = FairyState.Collecting;
                        hasWanderTarget = false;
                        seekingCoin = coin;
                        break;
                    }
                }

                break;

            case FairyState.Collecting:
                if (seekingCoin == null || target == null)
                {
                    state = FairyState.Wandering;
                    break;
                }
                if ( MathHelper.CalculateDistance(transform.position.x, transform.position.y, target.x, target.y) < .2f && seekingCoin != null) // pick up coin
                {
                    PickUpCoin();
                    state = FairyState.Wandering;
                    break;
                }
                break;

        }
    }

    private void PickUpCoin()
    {
        ResourceController.Instance.AddCoin();
        ResourceController.Instance.GetGroundCoins().Remove(seekingCoin);
        Destroy(seekingCoin.gameObject);
        audioSource.Play();

    }

    private Coin CheckCoins()
    {
        Coin closestCoin = null;

        float minDst = 99999f;
        foreach (Coin coin in ResourceController.Instance.GetGroundCoins())
        {
            if (coin.HasSeeker() == false)
            {
                float dstToCoin = MathHelper.CalculateDistance(transform.position.x, transform.position.y, coin.transform.position.x, coin.transform.position.y);
                if (dstToCoin < minDst)
                {
                    closestCoin = coin;
                    minDst = dstToCoin;
                }
            }
        }

        if (closestCoin != null)
        {
            closestCoin.SetSeeker();
        }
        return closestCoin;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate the direction vector towards the target point
            Vector2 targetPos = new Vector2(target.x, target.y);
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

    public void SetUpgradedSpeed(float speedBoost)
    {
        moveSpeed += speedBoost;
    }


    private enum FairyState
    {
        Wandering,
        Collecting
    }
}

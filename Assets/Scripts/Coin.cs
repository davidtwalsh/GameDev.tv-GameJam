using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool beginJump = false;
    float timer = 0f;
    Vector3 startPos;
    private float yClamper;
    [SerializeField]
    private float timeAmplifier;
    private float xSpeed;
    private float xDirection;
    private float yEnd;

    private bool hasSeeker = false;


    private void Awake()
    {
        startPos = transform.position;

        // Generate a random number between 0 and 1
        int randomSign = Random.Range(0, 2);

        // Use conditional logic to determine the sign (-1 or 1)
        xDirection = randomSign == 0 ? -1 : 1;

        yClamper = Random.Range(20f, 30f);
        xSpeed = Random.Range(.5f, 2f);
        yEnd = Random.Range(.15f, .6f);
    }

    private void Start()
    {
        beginJump = true;
    }
    // Update is called once per frame
    void Update()
    {

        if (beginJump == true && transform.position.y >= startPos.y - yEnd)
        {
            timer += Time.deltaTime;

            float x = timer * timeAmplifier;
            float y = GetYFromEquation(-1,10,-1,x)/yClamper + startPos.y;

            float xx = transform.position.x + (xSpeed * Time.deltaTime * xDirection);

            Vector3 newPos = new Vector3(xx, y, 0);
            transform.position = newPos;
        }
    }

    private float GetYFromEquation(float a, float b, float c, float x)
    {
        float first = a * (x * x);
        float second = b * x;
        float third = c;
        return first + second + third;
    }

    public bool HasSeeker()
    {
        return hasSeeker;
    }

    public void SetSeeker()
    {
        hasSeeker = true;
    }
}

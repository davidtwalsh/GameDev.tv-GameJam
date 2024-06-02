using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRemains : MonoBehaviour
{
    private float activeTimer = 0f;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        activeTimer += Time.deltaTime;
        if (activeTimer > 10f)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound()
    {
        audioSource.Play();
    }



}

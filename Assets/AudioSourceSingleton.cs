using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSingleton : MonoBehaviour
{

    public static AudioSourceSingleton Instance;

    private AudioSource audioSource;

    [SerializeField]
    AudioClip structureDestroyedClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If an instance already exists and it's not this one, destroy this instance
            Destroy(this.gameObject);
        }
        else
        {
            // Set this instance as the singleton instance if it's the first one
            Instance = this;
        }
        audioSource = GetComponent<AudioSource>();  
    }

    public void PlayStructureDestroyed()
    {
        audioSource.clip = structureDestroyedClip;
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField]
    private Sprite sheepSprite;

    public static SpriteController Instance;

    public AudioClip sheepClip;

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
    }

    public Sprite GetSheepSprite()
    {
        return sheepSprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    private EntityStatus status;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite statueNoDamageSprite;
    [SerializeField]
    private Sprite statueMinorDamageSprite;
    [SerializeField]
    private Sprite statueMajorDamageSprite;

    private void Awake()
    {
        status = GetComponent<EntityStatus>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ObjectPlacer.Instance.GetPlayerAttackables().Add(gameObject);
    }
    public void StatueAttacked()
    {
        CheckUpdateSprite();
    }

    public void StatueDestroyed()
    {
        AudioSourceSingleton.Instance.PlayStructureDestroyed();
        WinConditionController.Instance.LostGame();
    }

    private void CheckUpdateSprite()
    {
        float maxHP = status.GetMaxHP();
        float curHP = status.GetCurrentHP();
        if (curHP / maxHP <= .33f)
        {
            spriteRenderer.sprite = statueMajorDamageSprite;
        }
        else if (curHP / maxHP <= .66f)
        {
            spriteRenderer.sprite = statueMinorDamageSprite;
        }
        else
        {
            spriteRenderer.sprite = statueNoDamageSprite;
        }
    }
}

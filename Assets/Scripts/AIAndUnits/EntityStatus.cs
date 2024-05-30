using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStatus : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private Color normalSpriteColor;
    [SerializeField]
    private Color damageSpriteColor;

    private float hp;
    
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject remainsPrefab;

    [SerializeField]
    private UnityEvent onAttackEvent;

    [SerializeField]
    private UnityEvent onDeathEvent;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHP;
    }

    public void DealDamageToEntity(float damage)
    {
        hp -= damage;
        onAttackEvent.Invoke();
        CheckHP();
        StartCoroutine(FlashSprite());
    }

    private void CheckHP()
    {
        if (hp <= 0)
        {
            onDeathEvent.Invoke();

            if (SpawnController.Instance.GetMonsters().Contains(gameObject))
            {
                SpawnController.Instance.GetMonsters().Remove(gameObject);
            }
            if (ObjectPlacer.Instance.GetPlayerAttackables().Contains(gameObject))
            {
                ObjectPlacer.Instance.GetPlayerAttackables().Remove(gameObject);
            }

            if (remainsPrefab != null)
            {
                GameObject remains = Instantiate(remainsPrefab, transform.position, Quaternion.identity);
                remains.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                remains.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            }

            Destroy(gameObject);
        }
    }

    IEnumerator FlashSprite()
    {
        spriteRenderer.color = damageSpriteColor;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = normalSpriteColor;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }

    public float GetCurrentHP()
    {
        return hp;
    }

    public void RestoreHP()
    {
        hp = maxHP;
    }

    public bool IsFullHP()
    {
        if (hp == maxHP)
        {
            return true;
        }
        return false;
    }

    public void AddHP(int hpToAdd)
    {
        hp += hpToAdd;
    }
}

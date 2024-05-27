using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHP;
    }

    public void DealDamageToEntity(float damage)
    {
        hp -= damage;
        CheckHP();
        StartCoroutine(FlashSprite());
    }

    private void CheckHP()
    {
        if (hp <= 0)
        {
            if (SpawnController.Instance.GetMonsters().Contains(gameObject))
            {
                SpawnController.Instance.GetMonsters().Remove(gameObject);
            }
            ObjectPlacer.Instance.GetPlayerAttackables().Remove(gameObject);

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
}

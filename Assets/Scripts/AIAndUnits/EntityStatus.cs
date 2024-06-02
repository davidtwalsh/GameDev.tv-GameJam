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

    private float polymorphTime = 0f;
    private bool isPolymorphed = false;

    [SerializeField]
    private bool isArmoured = false;

    private List<SpriteRenderer> childrenSpriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) //use children sprite renderers then
        {
            // Loop through each child of the parent transform
            for (int i = 0; i < transform.childCount; i++)
            {
                // Get the child transform at index i
                Transform childTransform = transform.GetChild(i);

                SpriteRenderer spr = childTransform.GetComponent<SpriteRenderer>();
                if (spr != null)
                {
                    childrenSpriteRenderers.Add(spr);
                }
            }
        }
        hp = maxHP;
    }

    private void Update()
    {
        if (isPolymorphed)
        {
            polymorphTime -= Time.deltaTime;
            if (polymorphTime <= 0f)
            {
                EndPolymorph();
            }
        }
    }

    public void DealDamageToEntity(float damage,bool isPercingAttack)
    {
        if (isArmoured == true && isPercingAttack == false)
        {
            damage = damage / 4;
        }
        hp -= damage;
        onAttackEvent.Invoke();
        CheckHP();
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashSprite());
        }
        else if (childrenSpriteRenderers.Count > 0)
        {
            StartCoroutine(FlashChildrenSprites());
        }
    }

    public void PolymorphEntity(float polymorphTime)
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetPolymorphed();
            isPolymorphed = true;
            this.polymorphTime = polymorphTime;
        }
    }

    public void EndPolymorph()
    {
        isPolymorphed = false;
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.EndPolymorph(); 
        }
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
                AudioSource audioSource = remains.GetComponent<AudioSource>();
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null && audioSource != null)
                {
                    audioSource.clip = enemy.GetDeathSound();
                    ObjectRemains objectRemains = remains.GetComponent<ObjectRemains>();
                    if  (objectRemains != null)
                    {
                        objectRemains.PlaySound();
                    }
                }
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

    IEnumerator FlashChildrenSprites()
    {
        foreach (SpriteRenderer spriteRenderer in childrenSpriteRenderers)
        {
            spriteRenderer.color = damageSpriteColor;
        }

        yield return new WaitForSeconds(.1f);

        foreach (SpriteRenderer spriteRenderer in childrenSpriteRenderers)
        {
            spriteRenderer.color = normalSpriteColor;
        }
        
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

    public bool IsPolymorphed()
    {
        return isPolymorphed;
    }

    public bool IsArmoured()
    {
        return isArmoured;
    }
}

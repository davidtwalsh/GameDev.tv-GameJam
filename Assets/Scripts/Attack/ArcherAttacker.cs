using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttacker : MonoBehaviour, IAttacker
{
    [SerializeField]
    GameObject bow;

    [SerializeField]
    GameObject arrowPrefab;

    [SerializeField]
    float attackTime = 2f;

    private bool isAttacking = false;
    private GameObject target;

    private float attackTimer = 0f;

    [SerializeField]
    private DamageAttack attack;

    void Update()
    {
        if (isAttacking && target != null)
        {
            RotateBow();
            attackTimer += Time.deltaTime;
            if (attackTimer > attackTime)
            {
                ShootArrow();
                attackTimer = 0f;
            }
        }
    }

    public void BeginAttacking(GameObject target)
    {
        isAttacking = true;
        this.target = target;
        
    }

    public bool IsDoneAttacking()
    {
        return false;
    }

    public void CleanUpAttack()
    {

    }

    private void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = bow.transform.position;
        arrow.transform.rotation = bow.transform.rotation;
        arrow.transform.localScale = transform.localScale;
        Projectile projectile = arrow.GetComponent<Projectile>();
        projectile.Init(target,attack);
    }

    private void RotateBow()
    {
        //Rotate the bow to target
        // Get the direction vector from this object to the targe
        Vector3 dir = target.transform.position - bow.transform.position;
        // Calculate the angle from the direction vector (in radians)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (transform.localScale.x < 0f)
        {
            //its flipped so add an extra 180 
            angle += 180;
        }
        // Rotate the object to face the target
        bow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public DamageAttack GetDamageAttack()
    {
        return attack;
    }

    public void SetUpgradedAttackTime(float newAttackTime)
    {
        attackTime = newAttackTime;
    }

    public GameObject GetAttackTarget(float attackRange)
    {
        /*
        GameObject closestMonsterInRange = null;
        float minDst = 99999f;
        foreach (GameObject monster in SpawnController.Instance.GetMonsters())
        {
            float dstToMonster = MathHelper.CalculateDistance(transform.position.x, transform.position.y, monster.transform.position.x, monster.transform.position.y);
            if (dstToMonster < attackRange && dstToMonster < minDst)
            {
                closestMonsterInRange = monster;
                minDst = dstToMonster;
            }
        }
        */
        GameObject newTarget = null;
        List<GameObject> monstersInRangeWithArmour = new List<GameObject>();
        List<GameObject> monstersInRangeNoArmour = new List<GameObject>();

        foreach (GameObject monster in SpawnController.Instance.GetMonsters())
        {
            float dstToMonster = MathHelper.CalculateDistance(transform.position.x, transform.position.y, monster.transform.position.x, monster.transform.position.y);
            if (dstToMonster < attackRange)
            {
                EntityStatus status = monster.GetComponent<EntityStatus>();
                if (status != null)
                {
                    if (status.IsArmoured() == true)
                    {
                        monstersInRangeWithArmour.Add(monster);
                    }
                    else
                    {
                        monstersInRangeNoArmour.Add(monster);
                    }
                }
            }
        }
        if (monstersInRangeNoArmour.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, monstersInRangeNoArmour.Count);
            newTarget = monstersInRangeNoArmour[randomIndex];
        }
       else if (monstersInRangeWithArmour.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, monstersInRangeWithArmour.Count);
            newTarget = monstersInRangeWithArmour[randomIndex];
        }
        return newTarget;
    }

}

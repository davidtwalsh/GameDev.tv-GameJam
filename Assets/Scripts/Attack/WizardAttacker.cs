using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttacker : MonoBehaviour, IAttacker
{
    [SerializeField]
    GameObject wand;

    [SerializeField]
    GameObject spellPrefab;

    [SerializeField]
    float attackTime = 2f;

    private bool isAttacking = false;
    private GameObject target;

    private float attackTimer = 0f;

    [SerializeField]
    private PolymorphAttack attack;

    private bool isDoneCasting = false;

    void Update()
    {
        if (isAttacking && target != null && isDoneCasting == false)
        {
            RotateWand();
            attackTimer += Time.deltaTime;
            if (attackTimer > attackTime)
            {
                CastSpell();
                attackTimer = 0f;
            }
        }
    }

    public void BeginAttacking(GameObject target)
    {
        isAttacking = true;
        this.target = target;
        isDoneCasting = false;
    }

    public bool IsDoneAttacking()
    {
        return isDoneCasting;
    }

    public void CleanUpAttack()
    {

    }

    private void CastSpell()
    {
        GameObject spell = Instantiate(spellPrefab);
        spell.transform.position = wand.transform.position;
        spell.transform.rotation = wand.transform.rotation;
        spell.transform.localScale = transform.localScale;
        Projectile projectile = spell.GetComponent<Projectile>();
        projectile.Init(target, attack);
        isDoneCasting = true;
    }

    private void RotateWand()
    {
        //Rotate the bow to target
        // Get the direction vector from this object to the targe
        Vector3 dir = target.transform.position - wand.transform.position;
        // Calculate the angle from the direction vector (in radians)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (transform.localScale.x < 0f)
        {
            //its flipped so add an extra 180 
            angle += 180;
        }
        // Rotate the object to face the target
        wand.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public PolymorphAttack GetDamageAttack()
    {
        return attack;
    }

    public void SetUpgradedPolymorphTime(float upgradedWizardPolymorphTime)
    {
        attack.UpgradePolymorphTime(upgradedWizardPolymorphTime);
    }

    public GameObject GetAttackTarget(float attackRange)
    {
        List<EntityStatus> statusesOfMonstersInRange = new List<EntityStatus>();

        foreach (GameObject monster in SpawnController.Instance.GetMonsters())
        {
            float dstToMonster = MathHelper.CalculateDistance(transform.position.x, transform.position.y, monster.transform.position.x, monster.transform.position.y);
            if (dstToMonster < attackRange)
            {
                EntityStatus status = monster.GetComponent<EntityStatus>();
                if (status != null)
                {
                    statusesOfMonstersInRange.Add(status);
                }
            }
        }

        List<EntityStatus> nonPolymorphed = new List<EntityStatus>();
        foreach (EntityStatus status in statusesOfMonstersInRange)
        {
            if (status.IsPolymorphed() == false)
            {
                nonPolymorphed.Add(status);
            }
        }

        if (nonPolymorphed.Count > 0)
        {
            //Get Closest one
            /*
            GameObject closestMonsterInRange = null;
            float minDst = 99999f;
            foreach (EntityStatus status in nonPolymorphed)
            {
                float dstToMonster = MathHelper.CalculateDistance(transform.position.x, transform.position.y, status.gameObject.transform.position.x, status.gameObject.transform.position.y);
                if (dstToMonster < attackRange && dstToMonster < minDst)
                {
                    closestMonsterInRange = status.gameObject;
                    minDst = dstToMonster;
                }
            }
            if (closestMonsterInRange != null)
            {
                return closestMonsterInRange;
            }
            */
            int randomIndex = UnityEngine.Random.Range(0, nonPolymorphed.Count);
            return (nonPolymorphed[randomIndex].gameObject);
        }

        return null;
    }

}

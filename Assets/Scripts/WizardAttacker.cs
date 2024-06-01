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
    private DamageAttack attack;

    void Update()
    {
        if (isAttacking && target != null)
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

    }

    public bool IsDoneAttacking()
    {
        return false;
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

    public DamageAttack GetDamageAttack()
    {
        return attack;
    }

    public void SetUpgradedAttackTime(float newAttackTime)
    {
        attackTime = newAttackTime;
    }
}

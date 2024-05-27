using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private Enemy weaponUser;
    [SerializeField]
    private DamageAttack damageAttack;


    public void AttackEvent()
    {
        GameObject attackTarget = weaponUser.getTarget();
        if (attackTarget != null)
        {
            EntityStatus status = attackTarget.GetComponent<EntityStatus>();
            if (status != null)
            {
                damageAttack.AffectTarget(status);
            }
        }

    }
}

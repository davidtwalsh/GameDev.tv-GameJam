using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageAttack : IAttack
{
    [SerializeField]
    private int damage;
    public void AffectTarget(EntityStatus target)
    {
        target.DealDamageToEntity(damage);
    }

    public int GetDamage()
    {
        return damage; 
    }

    public void UpgradeDamage(int extraDamage)
    {
        damage += extraDamage;
    }
}

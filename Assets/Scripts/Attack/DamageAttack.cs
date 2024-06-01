using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DamageAttack : IAttack
{
    [SerializeField]
    private int damage;
    [SerializeField]
    bool isPiercing = false;
    public void AffectTarget(EntityStatus target)
    {
        target.DealDamageToEntity(damage,isPiercing);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PolymorphAttack : IAttack
{
    [SerializeField]
    private float polymorphTime;
    public void AffectTarget(EntityStatus target)
    {
        target.PolymorphEntity(polymorphTime);
    }

    public float GetPolymorphTime()
    {
        return polymorphTime;
    }

    public void UpgradePolymorphTime(float extraTime)
    {
        polymorphTime += extraTime;
    }
}

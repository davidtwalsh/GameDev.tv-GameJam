using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private Enemy weaponUser;

    public void AttackEvent()
    {
        GameObject attackTarget = weaponUser.getTarget();
        if (attackTarget != null)
        {
            Wall wall = attackTarget.GetComponent<Wall>();
            if (wall != null)
            {
                wall.Attacked(damage);
            }
        }

    }

}

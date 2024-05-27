using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus : MonoBehaviour
{
    [SerializeField]
    private float hp;

    public void DealDamageToEntity(float damage)
    {
        hp -= damage;
        CheckHP();
    }

    private void CheckHP()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}

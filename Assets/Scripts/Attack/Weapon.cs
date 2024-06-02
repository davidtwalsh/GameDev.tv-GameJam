using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Enemy weaponUser;
    [SerializeField]
    private DamageAttack damageAttack;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AttackEvent()
    {
        GameObject attackTarget = weaponUser.getTarget();
        if (attackTarget != null)
        {
            EntityStatus status = attackTarget.GetComponent<EntityStatus>();
            if (status != null)
            {
                damageAttack.AffectTarget(status);
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.Play();
                }
            }
        }

    }
}

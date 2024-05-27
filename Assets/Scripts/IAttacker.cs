using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    public bool IsDoneAttacking();

    public void BeginAttacking(GameObject target);

    public void CleanUpAttack();
}

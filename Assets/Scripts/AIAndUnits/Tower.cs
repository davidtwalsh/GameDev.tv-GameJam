using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float fallDistance = 2f;

    private List<PlayerUnit> containedUnits = new List<PlayerUnit>();

    public void AddUnit(PlayerUnit unit)
    {
        containedUnits.Add(unit);
    }

    public void TowerDestroyed()
    {
        foreach (PlayerUnit unit in containedUnits)
        {
            Vector3 newPos = unit.gameObject.transform.position;
            newPos.y += fallDistance;
            unit.gameObject.transform.position = newPos;

            unit.SetIsInTower(false);

            ObjectPlacer.Instance.GetPlayerAttackables().Add(unit.gameObject);
            if (ObjectPlacer.Instance.GetToweredUnits().Contains(unit.gameObject))
            {
                ObjectPlacer.Instance.GetToweredUnits().Remove(unit.gameObject);
            }
        }
    }
}

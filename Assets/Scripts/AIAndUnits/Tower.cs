using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float fallDistance = 2f;

    private List<PlayerUnit> containedUnits = new List<PlayerUnit>();

    [SerializeField]
    private SpriteRenderer frontSprite;
    [SerializeField]
    private SpriteRenderer backSprite;
    [SerializeField]
    private SpriteRenderer floorSprite;

    [SerializeField]
    private Sprite stoneFrontSprite;
    [SerializeField]
    private Sprite stoneBackSprite;
    [SerializeField]
    private Sprite stoneFloorSprite;

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

    public void UpgradeTower()
    {
        frontSprite.sprite = stoneFrontSprite;
        backSprite.sprite = stoneBackSprite;
        floorSprite.sprite = stoneFloorSprite;
    }
}

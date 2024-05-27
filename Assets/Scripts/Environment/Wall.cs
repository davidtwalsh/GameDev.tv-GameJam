using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private int xPosition;
    private int yPosition;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private EntityStatus status;

    private WallType wallType;

    [SerializeField]
    private Sprite woodenWallTopSprite;
    [SerializeField]
    private Sprite woodenWallFrontSprite;
    [SerializeField]
    private Sprite woodenWallFrontMinorDamageSprite;
    [SerializeField]
    private Sprite woodenWallFrontMajorDamageSprite;
    [SerializeField]
    private Sprite woodenWallTopMinorDamageSprite;
    [SerializeField]
    private Sprite woodenWallTopMajorDamageSprite;


    private void Start()
    {
        status = GetComponent<EntityStatus>();
    }
    public void SetPosition(int x, int y)
    {
        xPosition = x;
        yPosition = y;
    }

    public void SetSprite()
    {
        TileType tileBelow = MapMaker.Instance.GetTileTypeForCell(xPosition, yPosition-1);
        if (tileBelow == TileType.Wall)
        {
            wallType = WallType.OnlyTop;
            CheckIfNeedToUpdateWallSprite();
        }
        else
        {
            wallType= WallType.FrontAndTop;
            CheckIfNeedToUpdateWallSprite();
        }
    }

    public void UpdateOtherSprites()
    {
        //Get Upper
        if (MapMaker.Instance.walls.ContainsKey((xPosition, yPosition + 1)))
        {
            Wall aboveWall = MapMaker.Instance.walls[(xPosition, yPosition + 1)];
            aboveWall.SetSprite();
        }
        //GetLower
        if (MapMaker.Instance.walls.ContainsKey((xPosition, yPosition - 1)))
        {
            Wall belowWall = MapMaker.Instance.walls[(xPosition, yPosition - 1)];
            belowWall.SetSprite();
        }
    }

    private void OnDestroy()
    {
        MapMaker.Instance.walls.Remove((xPosition, yPosition));
        MapMaker.Instance.SetTileTypeForCell(xPosition, yPosition, TileType.Grass);
        UpdateOtherSprites();
        Destroy(gameObject);
    }

    public void CheckIfNeedToUpdateWallSprite()
    {
        if (status == null)
        {
            if (wallType == WallType.FrontAndTop)
            {
                spriteRenderer.sprite = woodenWallFrontSprite;
            }
            else if (wallType == WallType.OnlyTop)
            {
                spriteRenderer.sprite = woodenWallTopSprite;
            }
        }
        else
        {
            float maxHP = status.GetMaxHP();
            float curHP = status.GetCurrentHP();

            if (curHP / maxHP <= .33f)
            {
                if (wallType == WallType.FrontAndTop)
                {
                    spriteRenderer.sprite = woodenWallFrontMajorDamageSprite;
                }
                else if (wallType == WallType.OnlyTop)
                {
                    spriteRenderer.sprite = woodenWallTopMajorDamageSprite;
                }
            }
            else if (curHP / maxHP <= .66f)
            {
                if (wallType == WallType.FrontAndTop)
                {
                    spriteRenderer.sprite = woodenWallFrontMinorDamageSprite;
                }
                else if (wallType == WallType.OnlyTop)
                {
                    spriteRenderer.sprite = woodenWallTopMinorDamageSprite;
                }
            }
            else
            {
                if (wallType == WallType.FrontAndTop)
                {
                    spriteRenderer.sprite = woodenWallFrontSprite;
                }
                else if (wallType == WallType.OnlyTop)
                {
                    spriteRenderer.sprite = woodenWallTopSprite;
                }
            }
        }
    }
    private enum WallType
    {
        OnlyTop,
        FrontAndTop
    }
}

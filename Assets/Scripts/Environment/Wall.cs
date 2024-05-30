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


    [SerializeField]
    private Sprite stoneWallTopSprite;
    [SerializeField]
    private Sprite stoneWallFrontSprite;
    [SerializeField]
    private Sprite stoneWallFrontMinorDamageSprite;
    [SerializeField]
    private Sprite stoneWallFrontMajorDamageSprite;
    [SerializeField]
    private Sprite stoneWallTopMinorDamageSprite;
    [SerializeField]
    private Sprite stoneWallTopMajorDamageSprite;

    private bool isStoneWall = false;

    private void Awake()
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
                if (isStoneWall == false)
                {
                    spriteRenderer.sprite = woodenWallFrontSprite;
                }
                else
                {
                    spriteRenderer.sprite = stoneWallFrontSprite;
                }
            }
            else if (wallType == WallType.OnlyTop)
            {
                if (isStoneWall == false)
                {
                    spriteRenderer.sprite = woodenWallTopSprite;
                }
                else
                {
                    spriteRenderer.sprite = stoneWallTopSprite;
                }
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
                    if (isStoneWall == false)
                    {
                        spriteRenderer.sprite = woodenWallFrontMajorDamageSprite;
                    }
                    else
                    {
                        spriteRenderer.sprite = stoneWallFrontMajorDamageSprite;
                    }
                }
                else if (wallType == WallType.OnlyTop)
                {
                    if (isStoneWall == false)
                    {
                        spriteRenderer.sprite = woodenWallTopMajorDamageSprite;
                    }
                    else
                    {
                        spriteRenderer.sprite = stoneWallTopMajorDamageSprite;
                    }
                }
            }
            else if (curHP / maxHP <= .66f)
            {
                if (wallType == WallType.FrontAndTop)
                {
                    if (isStoneWall == false)
                    {
                        spriteRenderer.sprite = woodenWallFrontMinorDamageSprite;
                    }
                    else
                    {
                        spriteRenderer.sprite = stoneWallFrontMinorDamageSprite;
                    }
                }
                else if (wallType == WallType.OnlyTop)
                {
                    if (isStoneWall == false)
                    {
                        spriteRenderer.sprite = woodenWallTopMinorDamageSprite;
                    }
                    else
                    {
                        spriteRenderer.sprite = stoneWallTopMinorDamageSprite;
                    }
                }
            }
            else
            {
                if (wallType == WallType.FrontAndTop)
                {
                    if (isStoneWall == false)
                    {
                        spriteRenderer.sprite = woodenWallFrontSprite;
                    }
                    else
                    {
                        spriteRenderer.sprite = stoneWallFrontSprite;
                    }
                }
                else if (wallType == WallType.OnlyTop)
                {
                    if (isStoneWall == false)
                    {
                        spriteRenderer.sprite = woodenWallTopSprite;
                    }
                    else
                    {
                        spriteRenderer.sprite= stoneWallTopSprite;
                    }
                }
            }
        }
    }

    public SpriteRenderer GetSpriteRenderer() 
    { 
        return spriteRenderer; 
    }
    public EntityStatus GetEntityStatus()
    {
        return status;
    }

    public void SetStoneWall()
    {
        isStoneWall = true;
    }
    private enum WallType
    {
        OnlyTop,
        FrontAndTop
    }
}

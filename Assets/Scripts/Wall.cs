using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private int xPosition;
    private int yPosition;

    [SerializeField]
    private Sprite hasBelowSprite;
    [SerializeField]
    private Sprite noBelowSprite;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float hp = 100;

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
            spriteRenderer.sprite = hasBelowSprite;
        }
        else
        {
            spriteRenderer.sprite = noBelowSprite;
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

    public void Attacked(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            DestroyWall();
        }
    }

    private void DestroyWall()
    {
        MapMaker.Instance.walls.Remove((xPosition, yPosition));
        MapMaker.Instance.SetTileTypeForCell(xPosition, yPosition,TileType.Grass);
        UpdateOtherSprites();
        Destroy(gameObject);
    }
}

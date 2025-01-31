using UnityEngine;
public class Tile
{
    public int row;
    public int column;
    public bool isPath;
    public bool isGoal;
    public Piece piece;
    public GameObject tileObject;
    public ClickTile clickTile;

    public Trap trap;

    public SpriteType spriteType;
    public GameObject key;


    public Tile(bool IsPath, int f, int c)
    {
        row = f;
        column = c;
        isPath = IsPath;
        piece = null;
        trap = null;
    }

    public enum SpriteType
    {
        none, wallLimitDown, wallLimitLeft, wallLimitUp, wallLimitRight, wallCornerDowLeft, wallCornerDowRight, wallCornerUpLeft, wallCornerUpRight, tMinus90, tPlus90, wallHorizontal, wallVertical, wallT, wallX, L, LReves, PointUp, PointDown, LMinus90, LMinus180, PointRight, PointLeft,
    }




}


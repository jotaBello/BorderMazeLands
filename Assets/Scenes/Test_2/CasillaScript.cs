using UnityEngine;
public class Casilla
{
    public int fila;
    public int columna;
    public bool EsCamino;
    public bool isGoal;
    public Ficha ficha;
    public GameObject casillaObject;

    public ClickCasilla clickCasilla;

    public Trampa trampa;

    public SpriteType spriteType;

    public int pathIndex;

    public GameObject key;


    public Casilla(bool esCamino, int f, int c)
    {
        fila = f;
        columna = c;
        EsCamino = esCamino;
        ficha = null;
        trampa = null;
    }

    public enum SpriteType
    {
        none, wallLimitDown, wallLimitLeft, wallLimitUp, wallLimitRight, wallCornerDowLeft, wallCornerDowRight, wallCornerUpLeft, wallCornerUpRight, tMinus90, tPlus90, wallHorizontal, wallVertical, wallT, wallX, L, LReves, PointUp, PointDown, LMinus90, LMinus180, PointRight, PointLeft,
    }




}


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

    public Casilla(bool esCamino, int f, int c)
    {
        fila = f;
        columna = c;
        EsCamino = esCamino;
        ficha = null;
        trampa = null;
    }


}


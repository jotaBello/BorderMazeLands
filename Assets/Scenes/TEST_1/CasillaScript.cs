using UnityEngine;

public class Casilla
{
    public int fila;
    public int columna;
    public bool EsCamino { get; set; }
    public Ficha ficha { get; set; } // Si hay un jugador en la casilla

    public GameObject casillaObject;

    public Trampa trampa;

    public Casilla(bool esCamino, int f, int c)
    {
        fila = f;
        columna = c;
        EsCamino = esCamino;
        ficha = null; // No hay jugador al principio
        trampa = null;
    }
}


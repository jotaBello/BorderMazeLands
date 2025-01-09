using UnityEngine;

public abstract class Trampa
{
    public Casilla casillaAsociada;

    public Trampa(Casilla casilla)
    {
        casillaAsociada = casilla;
    }


    public abstract void Activar(Ficha ficha);
}

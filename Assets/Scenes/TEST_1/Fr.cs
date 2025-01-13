using UnityEngine;
using URandom = UnityEngine.Random;

public class FreezeTrampa : Trampa
{
    private int time;

    public FreezeTrampa(Casilla casilla, int time) : base(casilla)
    {
        this.time = time;
    }

    public override void Activar(Ficha ficha)
    {
        if (!ficha.shield)
            ficha.freeze = time;
    }
}
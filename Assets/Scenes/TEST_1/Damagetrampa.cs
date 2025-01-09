using UnityEngine;

public class TrampaDaño : Trampa
{
    private int daño;

    public TrampaDaño(Casilla casilla, int daño) : base(casilla)
    {
        this.daño = daño;
    }

    public override void Activar(Ficha ficha)
    {
        // Reduce puntos de vida de la ficha
        ficha.vida -= daño;
        Debug.Log($"Ficha {ficha.team.teamName} sufrió {daño} puntos de daño");
    }
}

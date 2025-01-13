using UnityEngine;

public class Ficha
{
    public int Velocidad { get; set; }
    public int HabilidadEnfriamiento { get; set; } // Tiempo de enfriamiento para la habilidad
    public Casilla Posicion { get; set; }
    public Teams team;

    public int freeze;
    public int vida;
    public bool shield;

    public GameObject fichaObj;




    public Ficha(Teams team)
    {
        this.team = team;
        vida = team.vida;
        Velocidad = team.velocidad;
        freeze = 0;
        shield = false;
    }


}

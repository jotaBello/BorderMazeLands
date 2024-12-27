using UnityEngine;

public class Ficha
{
    public int Velocidad { get; set; }
    public int HabilidadEnfriamiento { get; set; } // Tiempo de enfriamiento para la habilidad
    public Casilla Posicion { get; set; }
    public Teams team;
   

    public Ficha(Teams tea)
    {
        team = tea;
    }
}

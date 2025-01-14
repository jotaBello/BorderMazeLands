using UnityEngine;
public class Ficha
{
    public int Velocidad;
    public Casilla Posicion;
    public Casilla PosicionInicialTurno;
    public Casilla Spawn;
    public Teams team;
    public ClickFicha clickFicha;

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

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
    public int slowness;
    public int lighttime;
    public int vida;
    public bool shield;
    public int shieldTime;

    public GameObject fichaObj;

    public bool Moved;

    public bool HadKey;
    public KeyScript key;
    public int cooldown;




    public Ficha(Teams team)
    {
        this.team = team;
        vida = team.vida;
        Velocidad = team.velocidad;
        freeze = 0;
        slowness = 0;
        shield = false;
        cooldown = team.habilidadEnfriamiento;
    }





}

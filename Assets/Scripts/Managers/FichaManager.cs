using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine.Rendering.Universal;

public class FichaManager : MonoBehaviour
{
    public Ficha fichaSelecc = null;
    public MazeManager mazeManager;
    public Turn_Manager turnManager;


    GameObject key;

    public List<Ficha> fichaList = new List<Ficha>();

    private void Start()
    {

    }
    public void SeleccionarFicha(Ficha fichaSel)
    {
        if (fichaSel.freeze <= 0)
        {
            if (!fichaSel.Moved && fichaSel.freeze <= 0) mazeManager.PonerVerdeCasillasValidas(fichaSel);
            fichaSelecc = fichaSel;
        }
    }

    public void MoverFicha(Ficha ficha, Casilla destino)
    {
        if (ficha.Posicion.fila > destino.fila)
            ficha.fichaObj.transform.rotation = quaternion.RotateY(math.PI);
        if (ficha.Posicion.fila < destino.fila)
            ficha.fichaObj.transform.rotation = quaternion.RotateY(0.0f);
        ficha.Posicion = destino;
        destino.ficha = ficha;

        ficha.fichaObj.transform.position = destino.casillaObject.transform.position;


        mazeManager.PrintMaze();
    }

    public void CheckTraps()
    {
        foreach (Ficha fich in fichaList)
        {
            if (fich.team == turnManager.equipos[turnManager.turnoActual])
            {
                if (fich.Posicion.trampa != null)
                {
                    fich.Posicion.trampa.Actived = true;
                    fich.Posicion.trampa.Activar(fich);


                    mazeManager.PrintMaze();
                }
            }

        }
    }
    public void CheckLife()
    {
        foreach (Ficha ficha in fichaList)
        {
            if (ficha.vida <= 0)
            {
                if (ficha.HadKey)
                {
                    ficha.HadKey = false;
                    ficha.key.CaerEnELPiso(ficha.Posicion);
                    ficha.key = null;
                }

                Casilla spawn = ficha.Spawn;
                MoverFicha(ficha, spawn);
                ficha.vida = ficha.team.vida;


            }
        }

    }

    public void CheckFreeze()
    {
        foreach (Ficha fich in fichaList)
        {
            if (fich.team == turnManager.equipos[turnManager.turnoActual])
            {
                if (fich.freeze > 0) fich.freeze--;
            }

        }
    }

    public void UpdateInitialPosotion()
    {
        foreach (Ficha ficha in fichaList)
        {
            ficha.PosicionInicialTurno = ficha.Posicion;
        }

    }

    public void CheckMovement()
    {
        foreach (Ficha ficha in fichaList)
        {
            ficha.Moved = false;
        }
    }

    public void CheckCooldown()
    {
        foreach (Ficha ficha in fichaList)
        {
            if (ficha.team == turnManager.equipos[turnManager.turnoActual])
            {
                if (ficha.cooldown > 0) ficha.cooldown--;
            }

        }
    }
    public void CheckSlowness()
    {
        foreach (Ficha ficha in fichaList)
        {
            if (ficha.team == turnManager.equipos[turnManager.turnoActual])
            {
                if (ficha.slowness > 0) ficha.slowness--;
                else
                {
                    ficha.Velocidad = ficha.team.velocidad;
                }
            }

        }
    }
    public void CheckLight()
    {
        foreach (Ficha ficha in fichaList)
        {
            if (ficha.team == turnManager.equipos[turnManager.turnoActual])
            {
                if (ficha.lighttime > 1) ficha.lighttime--;
                else
                {
                    ficha.fichaObj.GetComponent<Light2D>().pointLightOuterRadius = 6.0f;
                    ficha.fichaObj.GetComponent<Light2D>().pointLightInnerRadius = 2.0f;
                }
            }

        }
    }
    public void CheckShield()
    {
        foreach (Ficha ficha in fichaList)
        {
            if (ficha.team == turnManager.equipos[turnManager.turnoActual])
            {
                if (ficha.shieldTime > 0) ficha.shieldTime--;
                else
                {
                    ficha.shield = false;
                }
            }

        }
    }

    public void CheckWithoutPassTurn()
    {
        CheckTraps();
        CheckLife();
        CheckFreeze();
        turnManager.CheckKeys();
        turnManager.CheckWin();
    }

}

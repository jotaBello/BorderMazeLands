using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

public class FichaManager : MonoBehaviour
{
    public Ficha fichaSelecc = null;
    public MazeManager mazeManager;
    public Turn_Manager turnManager;

    public List<Ficha> fichaList = new List<Ficha>();

    private void Start()
    {

    }
    public void SeleccionarFicha(Ficha fichaSel)
    {
        if (fichaSel.freeze <= 0)
        {
            mazeManager.PonerVerdeCasillasValidas(fichaSel);
            fichaSelecc = fichaSel;
        }
    }

    public void MoverFicha(Ficha ficha, Casilla destino)
    {
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
                    fich.Posicion.trampa.Activar(fich);
                    fich.Posicion.trampa = null;
                    mazeManager.PrintMaze();

                    Debug.Log($"Vida restante  : {fich.vida}");
                    Debug.Log($"Velocidad restante  : {fich.Velocidad}");
                    Debug.Log($"Freeze time  : {fich.freeze}");
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


}

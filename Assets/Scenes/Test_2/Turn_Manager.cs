using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class Turn_Manager : MonoBehaviour
{
    public MazeManager mazeManager;
    private GameManager gameManager;
    public FichaManager fichaManager;
    public List<Teams> equipos;
    public int turnoActual;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();



        equipos = gameManager.users;
        turnoActual = 0;
        IniciarTurno();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FinalizarTurno();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (fichaManager.fichaSelecc != null)
            {
                fichaManager.fichaSelecc.team.Habilidad(fichaManager.fichaSelecc);
                fichaManager.CheckLife();
            }
        }
    }

    void IniciarTurno()
    {


    }

    public void FinalizarTurno()
    {

        fichaManager.CheckTraps();
        fichaManager.CheckLife();
        fichaManager.CheckFreeze();
        fichaManager.UpdateInitialPosotion();
        mazeManager.PrintMaze();


        turnoActual = (turnoActual + 1) % equipos.Count;
        IniciarTurno();
    }

}

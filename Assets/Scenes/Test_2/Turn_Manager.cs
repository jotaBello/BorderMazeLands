using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


public class Turn_Manager : MonoBehaviour
{
    public MazeManager mazeManager;
    private GameManager gameManager;
    public FichaManager fichaManager;
    private HudManager hudManager;
    public List<Teams> equipos;
    public int turnoActual;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();



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
                if (fichaManager.fichaSelecc.cooldown <= 0)
                {
                    fichaManager.fichaSelecc.team.Habilidad(fichaManager.fichaSelecc);
                    fichaManager.CheckLife();
                    fichaManager.fichaSelecc.cooldown = fichaManager.fichaSelecc.team.habilidadEnfriamiento;
                }
            }
        }
    }

    void IniciarTurno()
    {

        UpdateCamera();
    }

    void UpdateCamera()
    {
        GameObject target = null;
        foreach (Ficha ficha in fichaManager.fichaList)
        {
            if (ficha.team == equipos[turnoActual])
            {
                target = ficha.fichaObj;
            }
        }
        mazeManager.MainCamera.GetComponent<Camera_Script>().target = target;
    }

    void UpdateLight()
    {
        foreach (Ficha ficha in fichaManager.fichaList)
        {
            if (ficha.team == equipos[turnoActual])
            {
                ficha.fichaObj.GetComponent<Light2D>().enabled = false;
            }
            if (ficha.team == equipos[(turnoActual + 1) % equipos.Count])
            {
                ficha.fichaObj.GetComponent<Light2D>().enabled = true;
            }
        }
    }

    public void CheckWin()
    {
        foreach (Ficha ficha in fichaManager.fichaList)
        {
            if (ficha.team == equipos[turnoActual])
            {
                if (ficha.Posicion.isGoal == true && ficha.HadKey)
                {
                    Win(ficha);
                }
            }
        }
    }

    void Win(Ficha ficha)
    {
        Debug.Log($"{ficha.team.teamName} WINS ");
        gameManager.winner = ficha;
        hudManager.Win();
    }

    public void CheckKeys()
    {
        foreach (Ficha ficha in fichaManager.fichaList)
        {
            if (ficha.team == equipos[turnoActual])
            {
                if (ficha.Posicion.key != null && !ficha.HadKey && ficha.Posicion.key.GetComponent<KeyScript>().target == null)
                {
                    ficha.Posicion.key.GetComponent<KeyScript>().target = ficha.fichaObj;
                    ficha.HadKey = true;
                    ficha.key = ficha.Posicion.key.GetComponent<KeyScript>();
                }
            }
        }
    }




    public void FinalizarTurno()
    {
        fichaManager.fichaSelecc = null;


        fichaManager.CheckFreeze();
        fichaManager.UpdateInitialPosotion();
        mazeManager.PrintMaze();
        fichaManager.CheckMovement();
        fichaManager.CheckCooldown();
        fichaManager.CheckSlowness();
        fichaManager.CheckShield();


        fichaManager.CheckLight();
        UpdateLight();

        CheckWin();

        CheckKeys();



        turnoActual = (turnoActual + 1) % equipos.Count;
        IniciarTurno();
    }

}

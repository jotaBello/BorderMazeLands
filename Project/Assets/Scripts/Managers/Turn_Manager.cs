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
    public PieceManager pieceManager;
    private HudManager hudManager;
    public List<Teams> teams;
    public int currentTurn;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();



        teams = gameManager.users;
        currentTurn = 0;
        StartTurn();
    }

    void Update()
    {
        //Revisa si el jugador toca la tecla A o E
        if (Input.GetKeyDown(KeyCode.A))
        {
            FinishTurn();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pieceManager.pieceSelect != null)
            {
                if (pieceManager.pieceSelect.cooldown <= 0)
                {
                    pieceManager.pieceSelect.team.Ability(pieceManager.pieceSelect);
                    pieceManager.CheckLife();
                    pieceManager.pieceSelect.cooldown = pieceManager.pieceSelect.team.cooldown;
                }
            }
        }
    }

    //Comienza un turno
    void StartTurn()
    {
        hudManager.PutMessage($"Turno del jugador {currentTurn + 1}");

        UpdateCamera();
    }

    //Actualiza la camara al jugador que le corresponde el turno
    void UpdateCamera()
    {
        GameObject target = null;
        foreach (Piece piece in pieceManager.pieceList)
        {
            if (piece.team == teams[currentTurn])
            {
                target = piece.pieceObject;
            }
        }
        mazeManager.MainCamera.GetComponent<Camera_Script>().target = target;
    }

    //Actualiza la luz al jugador que le corresponde el turno
    void UpdateLight()
    {
        foreach (Piece piece in pieceManager.pieceList)
        {
            if (piece.team == teams[currentTurn])
            {
                piece.pieceObject.GetComponent<Light2D>().enabled = false;
            }
            if (piece.team == teams[(currentTurn + 1) % teams.Count])
            {
                piece.pieceObject.GetComponent<Light2D>().enabled = true;
            }
        }
    }

    //Revisa la condicion de victoria
    public void CheckWin()
    {
        foreach (Piece piece in pieceManager.pieceList)
        {
            if (piece.team == teams[currentTurn])
            {
                if (piece.Position.isGoal == true && piece.HadKey)
                {
                    Win(piece);
                }
            }
        }
    }

    //Metodo de Victoria
    void Win(Piece piece)
    {
        gameManager.winner = piece;
        hudManager.Win();
    }

    //Revisa si una ficha se encuentra con una llave
    public void CheckKeys()
    {
        foreach (Piece piece in pieceManager.pieceList)
        {
            if (piece.team == teams[currentTurn])
            {
                if (piece.Position.key != null && !piece.HadKey && piece.Position.key.GetComponent<KeyScript>().target == null)
                {
                    piece.Position.key.GetComponent<KeyScript>().target = piece.pieceObject;
                    piece.HadKey = true;
                    piece.key = piece.Position.key.GetComponent<KeyScript>();
                }
            }
        }
    }



    //Termina el turno y revisa las estadisticas y condicion de victoria
    public void FinishTurn()
    {
        pieceManager.pieceSelect = null;


        pieceManager.CheckFreeze();
        pieceManager.UpdateInitialPositions();
        mazeManager.PrintMaze();
        pieceManager.CheckMovement();
        pieceManager.CheckCooldown();
        pieceManager.CheckSlowness();
        pieceManager.CheckShield();


        pieceManager.CheckLight();
        UpdateLight();

        CheckWin();

        CheckKeys();



        currentTurn = (currentTurn + 1) % teams.Count;
        StartTurn();
    }

}

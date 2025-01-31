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

    void StartTurn()
    {
        hudManager.PutMessage($"Turno del jugador {currentTurn + 1}");

        UpdateCamera();
    }

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

    void Win(Piece piece)
    {
        gameManager.winner = piece;
        hudManager.Win();
    }

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

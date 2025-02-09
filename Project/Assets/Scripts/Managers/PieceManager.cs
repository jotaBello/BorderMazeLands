using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine.Rendering.Universal;

public class PieceManager : MonoBehaviour
{
    public Piece pieceSelect = null;
    public MazeManager mazeManager;
    public Turn_Manager turnManager;


    GameObject key;

    public List<Piece> pieceList = new List<Piece>();

    private void Start()
    {

    }

    //Method to select a piece to move or use an ability
    public void SelectPiece(Piece pieceSel)
    {
        if (pieceSel.freeze <= 0)
        {
            if (!pieceSel.Moved && pieceSel.freeze <= 0) mazeManager.Show_Valid_Tiles(pieceSel);
            pieceSelect = pieceSel;
        }
    }

    //Method to move a piece to the clicked tile
    public void MovePiece(Piece piece, Tile final)
    {
        if (piece.Position.row > final.row)
            piece.pieceObject.transform.rotation = quaternion.RotateY(math.PI);
        if (piece.Position.row < final.row)
            piece.pieceObject.transform.rotation = quaternion.RotateY(0.0f);
        piece.Position = final;
        final.piece = piece;

        piece.pieceObject.transform.position = final.tileObject.transform.position;


        mazeManager.PrintMaze();
    }

    //Checks if any piece fell into a trap
    public void CheckTraps()
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.team == turnManager.teams[turnManager.currentTurn])
            {
                if (piece.Position.trap != null)
                {
                    piece.Position.trap.Actived = true;
                    piece.Position.trap.Activate(piece);


                    mazeManager.PrintMaze();
                }
            }

        }
    }

    //Checks the life of each piece
    public void CheckLife()
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.life <= 0)
            {
                if (piece.HadKey)
                {
                    piece.HadKey = false;
                    piece.key.Fall_on_the_floor(piece.Position);
                    piece.key = null;
                }

                Tile spawn = piece.SpawnTile;
                MovePiece(piece, spawn);
                piece.life = piece.team.life;


            }
        }

    }

    //Checks if any piece is frozen
    public void CheckFreeze()
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.team == turnManager.teams[turnManager.currentTurn])
            {
                if (piece.freeze > 0) piece.freeze--;
            }

        }
    }

    //Updates the initial position of each piece at the start of the turn
    public void UpdateInitialPositions()
    {
        foreach (Piece piece in pieceList)
        {
            piece.PositionInitialTurn = piece.Position;
        }

    }

    //Sets each piece as not moved
    public void CheckMovement()
    {
        foreach (Piece piece in pieceList)
        {
            piece.Moved = false;
        }
    }

    //Decreases the cooldown of each piece
    public void CheckCooldown()
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.team == turnManager.teams[turnManager.currentTurn])
            {
                if (piece.cooldown > 0) piece.cooldown--;
            }

        }
    }
    //Decreases the slowness time of each piece
    public void CheckSlowness()
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.team == turnManager.teams[turnManager.currentTurn])
            {
                if (piece.slowness > 0) piece.slowness--;
                else
                {
                    piece.Speed = piece.team.speed;
                }
            }

        }
    }
    //Decreases the time with changed vision
    public void CheckLight()
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.team == turnManager.teams[turnManager.currentTurn])
            {
                if (piece.lighttime > 1) piece.lighttime--;
                else
                {
                    piece.pieceObject.GetComponent<Light2D>().pointLightOuterRadius = 6.0f;
                    piece.pieceObject.GetComponent<Light2D>().pointLightInnerRadius = 2.0f;
                }
            }

        }
    }
    //Decreases the shield time
    public void CheckShield()
    {
        foreach (Piece piece in pieceList)
        {
            if (piece.team == turnManager.teams[turnManager.currentTurn])
            {
                if (piece.shieldTime > 0) piece.shieldTime--;
                else
                {
                    piece.shield = false;
                }
            }

        }
    }

    //Checks some statistics without passing the turns
    public void CheckWithoutPassTurn()
    {
        CheckTraps();
        CheckLife();
        CheckFreeze();
        turnManager.CheckKeys();
        turnManager.CheckWin();
    }

}

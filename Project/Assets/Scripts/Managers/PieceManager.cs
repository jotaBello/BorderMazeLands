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

    //Metodo de seleccionar ficha para mover o usar habilidad
    public void SelectPiece(Piece pieceSel)
    {
        if (pieceSel.freeze <= 0)
        {
            if (!pieceSel.Moved && pieceSel.freeze <= 0) mazeManager.Show_Valid_Tiles(pieceSel);
            pieceSelect = pieceSel;
        }
    }

    //Metodo de mover ficha a la casilla clickeada
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

    //Revisa si alguna ficha cayo en una trampa
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

    //Revisa la vida de cada ficha
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

    //Revisa si alguna ficha esta congelada
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

    //Actualiza la posicion inicial en el turno de cada ficha
    public void UpdateInitialPositions()
    {
        foreach (Piece piece in pieceList)
        {
            piece.PositionInitialTurn = piece.Position;
        }

    }

    //Colocar cada ficha como no movida
    public void CheckMovement()
    {
        foreach (Piece piece in pieceList)
        {
            piece.Moved = false;
        }
    }

    //Disminuye el cooldown de cada ficha
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
    //Disminuye el tiempo de lentitud de cada ficha
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
    //Disminuye el tiempo con vision cambiada
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
    //Disminuye el timepo que se tiene el escudo
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

    //Revisar algunas estadisticas sin pasar el turno
    public void CheckWithoutPassTurn()
    {
        CheckTraps();
        CheckLife();
        CheckFreeze();
        turnManager.CheckKeys();
        turnManager.CheckWin();
    }

}

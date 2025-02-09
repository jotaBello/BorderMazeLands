using UnityEngine;
using UnityEngine.Rendering;


public class ClickPiece : MonoBehaviour
{
    public Turn_Manager turnManager;
    public PieceManager pieceManager;
    public MazeManager mazeManager;
    public Piece piece;

    public int life;
    public int freeze;


    void Start()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<Turn_Manager>();
        mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        pieceManager = GameObject.Find("PieceManager").GetComponent<PieceManager>();
    }

    //This method recognizes the click on the piece
    void OnMouseDown()
    {
        //If the piece corresponds to the current turn, select it
        if (piece.team == GameManager.Instance.users[turnManager.currentTurn])
        {
            pieceManager.SelectPiece(piece);
        }
        //Deselects the piece and prints the maze if it's not the current turn
        else
        {
            pieceManager.pieceSelect = null;
            mazeManager.PrintMaze();
        }
    }
}
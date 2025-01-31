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

    void Update()
    {
        life = piece.life;
        freeze = piece.freeze;
    }
    void OnMouseDown()
    {
        if (piece.team == GameManager.Instance.users[turnManager.currentTurn])
        {
            pieceManager.SelectPiece(piece);
        }
        else
        {
            pieceManager.pieceSelect = null;
            mazeManager.PrintMaze();
        }
    }
}
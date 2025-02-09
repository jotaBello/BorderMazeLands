using Unity.VisualScripting;
using UnityEngine;

public class ClickTile : MonoBehaviour
{
    public Turn_Manager turnManager;
    public PieceManager pieceManager;
    public MazeManager mazeManager;

    public Tile tile;


    void Start()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<Turn_Manager>();
        mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        pieceManager = GameObject.Find("PieceManager").GetComponent<PieceManager>();
    }

    //Este metodo reconoce el click sobre una casilla del laberinto
    void OnMouseDown()
    {
        if (pieceManager.pieceSelect != null)
        {
            //Si hay una casilla seleccionada y la casilla es alcanzable, mover la ficha
            if (mazeManager.IsValidTile(tile, pieceManager.pieceSelect) && pieceManager.pieceSelect.team == turnManager.teams[turnManager.currentTurn] && !pieceManager.pieceSelect.Moved && pieceManager.pieceSelect.freeze <= 0)
            {
                pieceManager.MovePiece(pieceManager.pieceSelect, tile);
                pieceManager.pieceSelect.Moved = true;
            }
            pieceManager.CheckWithoutPassTurn();
            mazeManager.PrintMaze();
        }
        else
        {
            mazeManager.PrintMaze();
        }
        pieceManager.pieceSelect = null;
    }
}

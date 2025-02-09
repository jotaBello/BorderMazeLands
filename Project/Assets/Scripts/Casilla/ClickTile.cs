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

    //This method recognizes a click on a maze tile
    void OnMouseDown()
    {
        if (pieceManager.pieceSelect != null)
        {
            //If a piece is selected and the tile is reachable, move the piece
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

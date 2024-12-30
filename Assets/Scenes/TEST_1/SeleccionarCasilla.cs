using Unity.VisualScripting;
using UnityEngine;

public class SeleccionarCasilla : MonoBehaviour
{
    public Casilla casilla;
    public MazeGeneration mazegen;

    public Vector2 position;

    TurnManager turnManager;

    void Start()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        mazegen = GameObject.Find("MazeGen").GetComponent<MazeGeneration>();
        position = transform.position;
        casilla = mazegen.laberinto[-((int)position.y - mazegen.laberinto.GetLength(0) / 2), (int)position.x + mazegen.laberinto.GetLength(1) / 2];
    }
    void OnMouseDown()
    {
        //Debug.Log($"tocaste la casilla {casilla.fila},{casilla.columna}");
        if (turnManager.fichaSelecc != null)
        {
            //Debug.Log("fichasell no es null");
            turnManager.MoverFicha(casilla);
        }
        else
        {
            Debug.Log("fichasell es null");
            turnManager.PonerNegrasCasillas();
        }
        turnManager.fichaSelecc = null;
    }
}


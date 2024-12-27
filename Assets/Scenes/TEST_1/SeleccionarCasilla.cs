using Unity.VisualScripting;
using UnityEngine;

public class SeleccionarCasilla : MonoBehaviour
{
    Casilla casilla;
    public MazeGeneration mazegen;

    Vector2 position;

    void Start()
    {
        mazegen = GameObject.Find("MazeGen").GetComponent<MazeGeneration>();
        position = transform.position;
        casilla = mazegen.laberinto[-((int)position.y - mazegen.laberinto.GetLength(0) / 2), (int)position.x + mazegen.laberinto.GetLength(1) / 2];
    }
    void OnMouseDown()
    {
        Debug.Log($"tocaste la casilla {casilla.fila},{casilla.columna}");
        /*if (ficha.Jugador == GameManager.Instance.turnoManager.jugadores[GameManager.Instance.turnoManager.turnoActual])
        {
            // Si la ficha pertenece al jugador cuyo turno es
            GameManager.Instance.SeleccionarFicha(ficha);
        }*/


    }
}


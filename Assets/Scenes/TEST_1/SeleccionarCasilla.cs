using Unity.VisualScripting;
using UnityEngine;

public class SeleccionarCasilla : MonoBehaviour
{
    Casilla casilla;

    void Start()
    {
        
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


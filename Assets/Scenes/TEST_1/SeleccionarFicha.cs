using UnityEngine;

public class SeleccionarFicha : MonoBehaviour
{
    public Ficha ficha;  // La ficha a la que está asociada este script
    public GameObject fichaObject;

    void OnMouseDown()
    {
        Debug.Log($"me tocaste, soy una ficha de {ficha.team}");
        /*if (ficha.Jugador == GameManager.Instance.turnoManager.jugadores[GameManager.Instance.turnoManager.turnoActual])
        {
            // Si la ficha pertenece al jugador cuyo turno es
            GameManager.Instance.SeleccionarFicha(ficha);
        }*/


    }
}


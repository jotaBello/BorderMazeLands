using UnityEngine;

public class SeleccionarFicha : MonoBehaviour
{
    public Ficha ficha;  // La ficha a la que está asociada este script
    public GameObject fichaObject;
    TurnManager turnManager;

    public MazeGeneration mazegen;

    Vector2 position;
    //  (int, int) positionMatrix;

    void Start()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        position = transform.position;
        //        positionMatrix = (-((int)position.y - mazegen.laberinto.GetLength(0) / 2), (int)position.x + mazegen.laberinto.GetLength(1) / 2);
        //ficha.positionF = position;
    }

    void OnMouseDown()
    {
        Debug.Log($"me tocaste, soy una ficha de {ficha.team}");
        if (ficha.team == GameManager.Instance.users[turnManager.turnoActual])
        {
            turnManager.SeleccionarFicha(ficha);
            Debug.Log("casilla seleccionada");
        }



    }
}


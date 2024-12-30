using UnityEngine;

public class SeleccionarFicha : MonoBehaviour
{
    public Ficha ficha;  // La ficha a la que está asociada este script
    public GameObject fichaObject;
    TurnManager turnManager;

    public Casilla casillaPosicion;
    public Casilla PosicionInicialTurno;

    public MazeGeneration mazegen;

    Vector2 position;
    //  (int, int) positionMatrix;

    void Start()
    {

        mazegen = GameObject.Find("MazeGen").GetComponent<MazeGeneration>();
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        position = transform.position;
        //        positionMatrix = (-((int)position.y - mazegen.laberinto.GetLength(0) / 2), (int)position.x + mazegen.laberinto.GetLength(1) / 2);
        //ficha.positionF = position;
        casillaPosicion = mazegen.laberinto[-((int)position.y - mazegen.laberinto.GetLength(0) / 2), (int)position.x + mazegen.laberinto.GetLength(1) / 2];
        PosicionInicialTurno = casillaPosicion;
    }

    void OnMouseDown()
    {
        Debug.Log($"me tocaste, soy una ficha de {ficha.team}");
        if (ficha.team == GameManager.Instance.users[turnManager.turnoActual])
        {
            turnManager.PonerVerdeCasillasValidas(ficha);
            turnManager.SeleccionarFicha(ficha);
            Debug.Log("casilla seleccionada");
        }
        else
        {
            turnManager.fichaSelecc = null;
            turnManager.PonerNegrasCasillas();
        }



    }
}


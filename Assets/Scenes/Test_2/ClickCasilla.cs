using Unity.VisualScripting;
using UnityEngine;

public class ClickCasilla : MonoBehaviour
{
    public Turn_Manager turnManager;
    public FichaManager fichaManager;
    public MazeManager mazeManager;

    public Casilla casilla;

    //Test
    public Casilla.SpriteType testSpriteType;
    public int testFila;
    public int testColumna;

    void Start()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<Turn_Manager>();
        mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        fichaManager = GameObject.Find("FichaManager").GetComponent<FichaManager>();
    }
    void OnMouseDown()
    {
        if (fichaManager.fichaSelecc != null)
        {
            if (mazeManager.IsValidCasilla(casilla, fichaManager.fichaSelecc) && fichaManager.fichaSelecc.team == turnManager.equipos[turnManager.turnoActual])
                fichaManager.MoverFicha(fichaManager.fichaSelecc, casilla);
            mazeManager.PrintMaze();
        }
        else
        {
            Debug.Log("fichasell es null");
            mazeManager.PrintMaze();
        }
        fichaManager.fichaSelecc = null;
    }

    void Update()
    {
        testSpriteType = casilla.spriteType;
        testFila = casilla.fila;
        testColumna = casilla.columna;
    }
}

using UnityEngine;

public class ClickCasilla : MonoBehaviour
{
    public Turn_Manager turnManager;
    public FichaManager fichaManager;
    public MazeManager mazeManager;

    public Casilla casilla;

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
}

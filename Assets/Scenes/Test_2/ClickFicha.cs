using UnityEngine;
using UnityEngine.Rendering;


public class ClickFicha : MonoBehaviour
{
    public Turn_Manager turnManager;
    public FichaManager fichaManager;
    public MazeManager mazeManager;
    public Ficha ficha;

    public int vida;
    public int freeze;


    void Start()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<Turn_Manager>();
        mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        fichaManager = GameObject.Find("FichaManager").GetComponent<FichaManager>();
    }

    void Update()
    {
        vida = ficha.vida;
        freeze = ficha.freeze;
    }
    void OnMouseDown()
    {
        if (ficha.team == GameManager.Instance.users[turnManager.turnoActual])
        {
            fichaManager.SeleccionarFicha(ficha);
        }
        else
        {
            fichaManager.fichaSelecc = null;
            mazeManager.PrintMaze();
        }
    }
}
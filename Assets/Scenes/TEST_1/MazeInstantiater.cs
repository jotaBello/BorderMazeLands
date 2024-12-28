using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeInstantiater : MonoBehaviour
{
    public MazeGeneration mazeGen;
    Casilla[,] maze;

    public GameObject wall;
    public GameObject path;

    public GameObject player;

    public GameManager gameManager;

    List<(int, int)> listInitialPositions;

    List<Ficha> fichaList;


    void Start()
    {
        maze = mazeGen.laberinto;
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Vector2 position = new Vector2(j - maze.GetLength(1) / 2, -i + maze.GetLength(0) / 2);
                if (!maze[i, j].EsCamino) Instantiate(wall, position, Quaternion.identity);
                if (maze[i, j].EsCamino) maze[i, j].casillaObject = Instantiate(path, position, Quaternion.identity);
            }
        }
        InstantiatePlayer();
    }

    void InstantiatePlayer()
    {
        GeneratePositions();
        fichaList = new List<Ficha>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < gameManager.users.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Ficha ficha = new Ficha(gameManager.users[i]);
                GameObject fichaObj;

                fichaObj = InstanciarFicha(ficha, 2 * i + j);

                ficha.fichaObj = fichaObj;
                fichaObj.GetComponent<SeleccionarFicha>().fichaObject = fichaObj;
                fichaObj.GetComponent<SeleccionarFicha>().ficha = ficha;
                fichaObj.GetComponent<SpriteRenderer>().color = gameManager.users[i].colort;
                fichaList.Add(ficha);
            }
        }
    }

    void GeneratePositions()
    {
        listInitialPositions = new List<(int, int)>();
        listInitialPositions.Add((1, 1));
        listInitialPositions.Add((maze.GetLength(0) - 2, maze.GetLength(1) - 2));
        listInitialPositions.Add((maze.GetLength(0) - 2, 1));
        listInitialPositions.Add((1, maze.GetLength(1) - 2));
    }

    GameObject InstanciarFicha(Ficha ficha, int i)
    {
        Vector2 position = new Vector2(listInitialPositions[i].Item2 - maze.GetLength(0) / 2, -listInitialPositions[i].Item1 + maze.GetLength(0) / 2);
        return Instantiate(player, position, Quaternion.identity);
    }
}

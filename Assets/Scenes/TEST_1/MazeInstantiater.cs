using UnityEngine;

public class MazeInstantiater : MonoBehaviour
{
    public MazeGeneration mazeGen;
    Casilla[,] maze;

    public GameObject wall;
    public GameObject path;

    public GameObject player;
    public Vector2 Initialposition;

    void Start()
    {
        maze = mazeGen.laberinto;
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Vector2 position = new Vector2(i - maze.GetLength(0) / 2, j - maze.GetLength(1) / 2);
                if (!maze[i, j].EsCamino) Instantiate(wall, position, Quaternion.identity);
                if (maze[i, j].EsCamino) Instantiate(path, position, Quaternion.identity);
            }
        }

        InstantiatePlayer();
    }

    void InstantiatePlayer()
    {
        Initialposition = new Vector2(Initialposition.x - maze.GetLength(0) / 2, Initialposition.y - maze.GetLength(1) / 2);
        Instantiate(player, Initialposition, Quaternion.identity);
    }
}

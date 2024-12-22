using UnityEngine;

public class MazeInstantiater : MonoBehaviour
{
    public MazeGeneration mazeGen;
    int[,] maze;

    public GameObject wall;

    void Start()
    {
        maze = mazeGen.laberinto;
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); jj++)
            {
                Instantiate
            }
        }
    }
}

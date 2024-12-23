/*using System.Threading;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Teams myteam;
    public MazeGeneration maze;

    int positionI;
    int positionJ;

    void Start()
    {
        maze.laberinto[29, 1] = 2;
        positionI = 29;
        positionJ = 1;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D) && maze.laberinto[positionI, positionJ + 1] != 0)
        {
            Debug.Log("d");
            maze.laberinto[positionI, positionJ + 1] = 2;
            maze.laberinto[positionI, positionJ] = 1;
            positionJ++;
            transform.position = new Vector3(transform.position.x + 32, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.A) && maze.laberinto[positionI, positionJ - 1] != 0)
        {
            Debug.Log("a");
            maze.laberinto[positionI, positionJ - 1] = 2;
            maze.laberinto[positionI, positionJ] = 1;
            positionJ--;
            transform.position = new Vector3(transform.position.x - 32, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.W) && maze.laberinto[positionI - 1, positionJ] != 0)
        {
            Debug.Log("w");
            maze.laberinto[positionI - 1, positionJ] = 2;
            maze.laberinto[positionI, positionJ] = 1;
            positionI--;
            transform.position = new Vector3(transform.position.x, transform.position.y + 32, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.S) && maze.laberinto[positionI + 1, positionJ] != 0)
        {
            Debug.Log("s");
            maze.laberinto[positionI + 1, positionJ] = 2;
            maze.laberinto[positionI, positionJ] = 1;
            positionI++;
            transform.position = new Vector3(transform.position.x, transform.position.y - 32, transform.position.z);
        }
    }
}
*/
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTeam", menuName = "New Team")]
public class Teams : ScriptableObject
{
    public Sprite teamImage;
    public string teamName;
    public string teamDescription;

    public int velocidad;

    public Color colort;
    public Sprite playerSprite;
    public int habilidadEnfriamiento;

    public int vida;


    public void Habilidad(Ficha ficha)
    {
        switch (teamName)
        {
            case "Elemental":
                ElementalAbility(ficha);
                break;
            case "Hyperion":
                HyperionAbility(ficha);
                break;
            case "Jackobs":
                JackobsAbility(ficha);
                break;
            case "Torgue":
                TorgueAbility(ficha);
                break;
            default:
                Debug.LogError("Ability from an Unknow Team");
                break;
        }
    }

    void ElementalAbility(Ficha ficha)
    {
        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();

        Casilla[,] maze = mazeManager.maze;
        List<Ficha> listNearEnemies = FindNearEnemies(ficha);

        foreach (Ficha f in listNearEnemies)
        {
            f.freeze = 2;
        }



        List<Ficha> FindNearEnemies(Ficha ficha)
        {
            List<Ficha> listNearEnemies = new List<Ficha>();
            Casilla casilla = ficha.Posicion;

            (int, int) casillaPlayer = (casilla.fila, casilla.columna);

            int[,] bfs = mazeManager.BFS(casillaPlayer);


            for (int i = 0; i < bfs.GetLength(0); i++)
            {
                for (int j = 0; j < bfs.GetLength(1); j++)
                {

                    if (maze[i, j].ficha != null)
                    {
                        //Debug.LogError($"hay una ficha en la posicion {i}, {j}");
                        //Debug.LogError($"para llegar a la posicion {i}, {j} se necesitan {bfs[i, j]}");
                        if (bfs[i, j] <= 2)
                        {
                            // Debug.LogError($"hay una casilla valida en la posicion {i}, {j}");
                            if (maze[i, j].ficha != ficha)
                            {
                                // Debug.LogError($"hay una ficha distinta en la posicion {i}, {j}");
                                listNearEnemies.Add(maze[i, j].ficha);
                            }
                        }
                    }

                }
            }


            //if (listNearEnemies.Count > 0) Debug.LogWarning("he encontrado al menos uno");
            return listNearEnemies;
        }

    }

    void HyperionAbility(Ficha ficha)
    {
        ficha.shield = true;

        //falta implementar quitar el escudo
    }

    void JackobsAbility(Ficha ficha)
    {
        Debug.LogError("entro en jackobs ability");

        (int, int)[] directions =
        {
        (0,-1),
        (1,0),
        (0,1),
        (-1,0)

    };

        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        Casilla[,] maze = mazeManager.maze;


        Casilla casilla = ficha.Posicion;
        (int, int) casillaPlayer = (casilla.fila, casilla.columna);
        List<Ficha> enemies = FindEnemies(casillaPlayer);

        Debug.Log($"La habilidad comienza en {casilla.fila}, {casilla.columna}");

        foreach (Ficha enemie in enemies)
        {
            enemie.vida = 0;
            Debug.LogWarning("Die");
        }

        List<Ficha> FindEnemies((int, int) casilla)
        {
            List<Ficha> enemies = new List<Ficha>();

            foreach (var dir in directions)
            {
                int fila = casilla.Item1 + dir.Item1;
                int columna = casilla.Item2 + dir.Item2;

                while (fila >= 0 && fila < maze.GetLength(0) && columna >= 0 && columna < maze.GetLength(1) && maze[fila, columna].EsCamino)
                {
                    if (maze[fila, columna].ficha != null)
                    {
                        enemies.Add(maze[fila, columna].ficha);
                    }
                    fila += dir.Item1;
                    columna += dir.Item2;
                }
            }

            if (enemies.Count > 0) Debug.LogWarning($"matare al menos uno ");
            Debug.LogWarning($"{enemies.Count}");
            return enemies;
        }

    }

    void TorgueAbility(Ficha ficha)
    {
        Debug.Log("Entro en torgue ability");

        (int, int)[] directions =
        {
        (0,-1),
        (1,0),
        (0,1),
        (-1,0)

    };

        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        Casilla[,] maze = mazeManager.maze;

        Casilla casilla = ficha.Posicion;
        (int, int) casillaPlayer = (casilla.fila, casilla.columna);

        foreach (var dir in directions)
        {
            int fila = casillaPlayer.Item1 + dir.Item1;
            int columna = casillaPlayer.Item2 + dir.Item2;

            if (fila >= 0 && fila < maze.GetLength(0) && columna >= 0 && columna < maze.GetLength(1))
            {
                if (!maze[fila, columna].EsCamino)
                {
                    maze[fila, columna].EsCamino = true;
                    maze[fila, columna].casillaObject.GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
        mazeManager.PonerVerdeCasillasValidas(ficha);
    }
}

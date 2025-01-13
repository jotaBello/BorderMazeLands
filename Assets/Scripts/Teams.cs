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
        TurnManager turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        Casilla[,] maze = turnManager.mazeInst.mazeGen.laberinto;
        List<Ficha> listNearEnemies = FindNearEnemies(ficha);

        foreach (Ficha f in listNearEnemies)
        {
            f.freeze = 2;
        }



        List<Ficha> FindNearEnemies(Ficha ficha)
        {
            //Debug.LogWarning("entro en findnear");
            List<Ficha> listNearEnemies = new List<Ficha>();

            Vector2 position = ficha.fichaObj.transform.position;
            Casilla casilla = maze[-((int)position.y - maze.GetLength(0) / 2), (int)position.x + maze.GetLength(1) / 2];
            (int, int) casillaPlayer = (casilla.fila, casilla.columna);

            int[,] bfs = turnManager.BFS(casillaPlayer);


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
                                maze[i, j].ficha.freeze = 69;
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

        TurnManager turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        Casilla[,] maze = turnManager.mazeInst.mazeGen.laberinto;


        Vector2 position = ficha.fichaObj.transform.position;
        Casilla casilla = maze[-((int)position.y - maze.GetLength(0) / 2), (int)position.x + maze.GetLength(1) / 2];
        (int, int) casillaPlayer = (casilla.fila, casilla.columna);
        List<Ficha> enemies = FindEnemies(casillaPlayer);

        Debug.Log($"La habilidad comienza en {casilla.fila}, {casilla.columna}");

        foreach (Ficha enemie in enemies)
        {
            //enemie.vida = 0;
            Debug.LogWarning("Die");
        }


        List<Ficha> FindEnemies((int, int) casilla)
        {
            List<Ficha> enemies = new List<Ficha>();


            //ERROR NO BUSCA BIEN LAS

            foreach (var dir in directions)
            {
                int i = 1;
                while (maze[casilla.Item1 + i * dir.Item1, casilla.Item1 + i * dir.Item2].EsCamino)
                {
                    Debug.LogWarning($"la casilla {casilla.Item1 + i * dir.Item1},{casilla.Item1 + i * dir.Item2} es camino");
                    if (maze[casilla.Item1 + i * dir.Item1, casilla.Item1 + i * dir.Item2].ficha != null)
                    {
                        enemies.Add(maze[casilla.Item1 + dir.Item1, casilla.Item1 + dir.Item2].ficha);
                    }
                    i++;
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

        TurnManager turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        Casilla[,] maze = turnManager.mazeInst.mazeGen.laberinto;


        Vector2 position = ficha.fichaObj.transform.position;
        Casilla casilla = maze[-((int)position.y - maze.GetLength(0) / 2), (int)position.x + maze.GetLength(1) / 2];
        (int, int) casillaPlayer = (casilla.fila, casilla.columna);

        foreach (var dir in directions)
        {
            Debug.Log("Entro FOREACH en torgue ability");
            Casilla cas = maze[casillaPlayer.Item1 + dir.Item1, casillaPlayer.Item2 + dir.Item2];
            if (!cas.EsCamino)
            {
                cas.casillaObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}

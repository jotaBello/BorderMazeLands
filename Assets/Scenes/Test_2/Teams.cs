using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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
            case "Maya":
                MayaAbility(ficha);
                break;
            case "Axton":
                AxtonAbility(ficha);
                break;
            case "Zero":
                ZeroAbility(ficha);
                break;
            case "Krieg":
                KriegAbility(ficha);
                break;
            case "Gaige":
                GaigeAbility(ficha);
                break;
            case "Salvador":
                SalvadorAbility(ficha);
                break;
            default:
                Debug.LogError("Ability from an Unknow Team");
                break;
        }
    }

    void MayaAbility(Ficha ficha)
    {
        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();

        Casilla[,] maze = mazeManager.maze;
        List<Ficha> listNearEnemies = FindNearEnemies(ficha);

        foreach (Ficha f in listNearEnemies)
        {
            f.freeze = 3;
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
                        if (bfs[i, j] <= 4)
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

    void AxtonAbility(Ficha ficha)
    {
        ficha.shield = true;
        ficha.shieldTime = 3;
    }

    void ZeroAbility(Ficha ficha)
    {


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


        foreach (Ficha enemie in enemies)
        {
            if (enemie != ficha)
                enemie.vida -= 5;
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


            return enemies;
        }

    }

    void KriegAbility(Ficha ficha)
    {

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

            if (fila > 0 && fila < maze.GetLength(0) - 1 && columna > 0 && columna < maze.GetLength(1) - 1)
            {
                if (!maze[fila, columna].EsCamino)
                {
                    maze[fila, columna].EsCamino = true;
                    mazeManager.HacerCamino(fila, columna);
                }
            }
        }
        mazeManager.PonerVerdeCasillasValidas(ficha);
    }

    void SalvadorAbility(Ficha ficha)
    {
        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();


        ficha.Velocidad += 2;
        ficha.lighttime = 1;
        ficha.fichaObj.GetComponent<Light2D>().pointLightOuterRadius *= 1.5f;
        ficha.fichaObj.GetComponent<Light2D>().pointLightInnerRadius *= 1.5f;
        mazeManager.PonerVerdeCasillasValidas(ficha);

    }

    void GaigeAbility(Ficha ficha)
    {
        (int, int)[] directions =
                {
        (0,-1),
        (1,0),
        (0,1),
        (-1,0)

    };
   //Debug.LogError("Paso1");
        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();

        Casilla[,] maze = mazeManager.maze;

        Casilla goal = ficha.Posicion;
        List<(Casilla key, int distance)> keys = new List<(Casilla keys, int distance)>();
        Casilla key = ficha.Posicion;

        int[,] bfs = mazeManager.BFS((ficha.Posicion.fila, ficha.Posicion.columna));

        foreach (Casilla casilla in maze)
        {
            if (casilla.isGoal) goal = casilla;
            else if (casilla.key != null) keys.Add((casilla, bfs[casilla.fila, casilla.columna]));
        }



        key = keys[0].key;
        foreach (var Key in keys)
        {
            if (Key.distance < bfs[key.fila, key.columna])
            {
                key = Key.key;
            }
        }



        if (ficha.HadKey)
        {
            List<Casilla> paths = GiveMeThePaths(ficha.Posicion, goal);
            MostrarCasillas(paths);
        }
        else
        {
            List<Casilla> paths = GiveMeThePaths(ficha.Posicion, key);
           // Debug.LogError("Paso2");
            MostrarCasillas(paths);
        }





        void MostrarCasillas(List<Casilla> paths)
        {
           // Debug.LogError("Paso3");

            if (paths.Count >= 3)
            {
              
                GameObject squareGaige1 = Instantiate(mazeManager.squareGaige, new Vector2(paths[paths.Count - 2].fila, paths[paths.Count - 2].columna), Quaternion.identity);
                mazeManager.squareSelectionList.Add(squareGaige1);
                GameObject squareGaige2 = Instantiate(mazeManager.squareGaige, new Vector2(paths[paths.Count - 3].fila, paths[paths.Count - 3].columna), Quaternion.identity);
                mazeManager.squareSelectionList.Add(squareGaige2);
                //Debug.LogError("Paso4");
            }
            else if (paths.Count == 2)
            {
                
                GameObject squareGaige1 = Instantiate(mazeManager.squareGaige, new Vector2(paths[paths.Count - 2].fila, paths[paths.Count - 2].columna), Quaternion.identity);
                mazeManager.squareSelectionList.Add(squareGaige1);
               // Debug.LogError("Paso4");
            }
            else
            {
                Debug.LogError("no entro en ninguna, maya");
            }
        }





        List<Casilla> GiveMeThePaths(Casilla inicio, Casilla destino)
        {
            int distance = bfs[destino.fila, destino.columna];
            Casilla current = destino;
            List<Casilla> paths = new List<Casilla>();


            while (distance > 0)
            {

                paths.Add(LaMinimaCasillaAdyascente(current));
                current = paths[paths.Count - 1];
                distance = bfs[current.fila, current.columna];
            }

            return paths;

        }

        Casilla LaMinimaCasillaAdyascente(Casilla current)
        {

            List<Casilla> adyascentes = new List<Casilla>();
            Casilla posible = null;
            int posibleDistance = int.MaxValue;

            foreach (var dir in directions)
            {
                adyascentes.Add(maze[current.fila + dir.Item1, current.columna + dir.Item2]);
            }
            foreach (var ady in adyascentes)
            {
                if (ady.EsCamino && bfs[ady.fila, ady.columna] < posibleDistance)
                {
                    posibleDistance = bfs[ady.fila, ady.columna];
                    posible = ady;
                }
            }
            if (posible == null) Debug.LogError("Posibler nulo");
            return posible;

        }

    }

}

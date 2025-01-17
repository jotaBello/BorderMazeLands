using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using URandom = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

public class MazeManager : MonoBehaviour
{

    public static int filas = 31, columnas = 31;
    public Casilla[,] maze = new Casilla[filas, columnas];
    System.Random rand = new System.Random();
    List<(int, int, int, int)> paredes = new List<(int, int, int, int)>();


    public GameManager gameManager;
    public FichaManager fichaManager;

    public GameObject MainCamera;

    public GameObject slot;
    public GameObject player;


    public GameObject squareSelection;
    public List<GameObject> squareSelectionList = new List<GameObject>();



    void Generar()
    {
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                // Asume que todo es pared por defecto
                maze[i, j] = new Casilla(false, i, j);
            }
        }


        // Selecciona una celda inicial en una posición impar
        int x = 15, y = 15;
        maze[x, y].EsCamino = true;  // Marca la celda como camino

        // Añade las paredes iniciales de esta celda
        AgregarParedes(x, y);


        // Procesa las paredes hasta que se acaben
        while (paredes.Count > 0)
        {
            // Elige una pared al azar y la elimina de la lista
            int indice = rand.Next(paredes.Count);
            var (px, py, cx, cy) = paredes[indice];
            paredes.RemoveAt(indice);

            // Si la celda conectada no ha sido visitada
            if (!maze[cx, cy].EsCamino)
            {
                maze[px, py].EsCamino = true; // Elimina la pared entre las celdas
                maze[cx, cy].EsCamino = true; // Marca la nueva celda como camino
                AgregarParedes(cx, cy); // Añade las paredes de la nueva celda
            }
        }


    }

    void AgregarParedes(int x, int y)
    {
        // Añade las paredes de las celdas adyacentes (solo celdas impares)
        if (x > 1) paredes.Add((x - 1, y, x - 2, y)); // Arriba
        if (x < filas - 2) paredes.Add((x + 1, y, x + 2, y)); // Abajo
        if (y > 1) paredes.Add((x, y - 1, x, y - 2)); // Izquierda
        if (y < columnas - 2) paredes.Add((x, y + 1, x, y + 2)); // Derecha
    }

    void ColocarTrampas()
    {
        int cantidadTrampas = 15; // Número de trampas que quieres colocar
        int intentos = 0;

        while (cantidadTrampas > 0 && intentos < 1000)
        {
            int x = rand.Next(1, filas - 1);
            int y = rand.Next(1, columnas - 1);

            if (maze[x, y].EsCamino && maze[x, y].trampa == null && maze[x, y].ficha == null)
            {
                int tipoTrampa = rand.Next(0, 3);

                switch (tipoTrampa)
                {
                    case 0:

                        maze[x, y].trampa = new Trampa(maze[x, y], "Tele");
                        break;
                    case 1:
                        maze[x, y].trampa = new Trampa(maze[x, y], "Damage");
                        break;
                    case 2:
                        maze[x, y].trampa = new Trampa(maze[x, y], "Freeze");
                        break;
                }

                cantidadTrampas--;
            }

            intentos++;
        }
    }

    void ColocarJugadores()
    {
        List<(int x, int y)> listInitialPositions = GeneratePositions();
        listInitialPositions = ShuffleList(listInitialPositions);

        for (int i = 0; i < gameManager.users.Count; i++)
        {
            Ficha ficha = new Ficha(gameManager.users[i]);

            ficha.Posicion = maze[listInitialPositions[i].x, listInitialPositions[i].y];
            ficha.Spawn = ficha.Posicion;
            ficha.PosicionInicialTurno = ficha.Posicion;


            maze[listInitialPositions[i].x, listInitialPositions[i].y].ficha = ficha;
            fichaManager.fichaList.Add(ficha);

        }

        List<(int x, int y)> ShuffleList(List<(int x, int y)> list)
        {
            List<(int x, int y)> shuffledList = new List<(int x, int y)>();

            while (list.Count > 0)
            {
                int random = rand.Next(0, list.Count);
                shuffledList.Add(list[random]);
                list.RemoveAt(random);
            }
            return shuffledList;
        }
    }

    List<(int, int)> GeneratePositions()
    {
        List<(int, int)> listInitialPositions = new List<(int x, int y)>();


        listInitialPositions = new List<(int, int)>();
        listInitialPositions.Add((1, 1));
        listInitialPositions.Add((maze.GetLength(0) - 2, maze.GetLength(1) - 2));
        listInitialPositions.Add((maze.GetLength(0) - 2, 1));
        listInitialPositions.Add((1, maze.GetLength(1) - 2));

        return listInitialPositions;
    }

    void ColocarMeta()
    {
        maze[15, 15].isGoal = true;
    }

    void InstanciarLaberinto()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Vector2 position = new Vector2(i, j);
                maze[i, j].casillaObject = Instantiate(slot, position, Quaternion.identity);
                maze[i, j].clickCasilla = maze[i, j].casillaObject.GetComponent<ClickCasilla>();
                maze[i, j].clickCasilla.casilla = maze[i, j];
            }
        }

    }

    void InstanciarJugadores()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j].ficha != null)
                {
                    Vector2 position = new Vector2(i, j);
                    maze[i, j].ficha.fichaObj = Instantiate(player, position, Quaternion.identity);
                    maze[i, j].ficha.clickFicha = maze[i, j].ficha.fichaObj.GetComponent<ClickFicha>();
                    maze[i, j].ficha.clickFicha.ficha = maze[i, j].ficha;

                    IdentificarFicha(maze[i, j].ficha);
                }
            }
        }
    }

    public void PrintMaze()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j].EsCamino)
                {
                    HacerCamino(i, j);
                }
                else
                {
                    HacerPared(i, j);
                }

                if (maze[i, j].trampa != null)
                {
                    HacerTrampa(i, j);
                }
                if (maze[i, j].isGoal)
                {
                    HacerGoal(i, j);
                }

            }


            foreach (GameObject square in squareSelectionList)
            {
                Destroy(square);
            }
        }
    }

    void HacerCamino(int i, int j)
    {
        maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.black;
    }
    void HacerPared(int i, int j)
    {
        maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void HacerTrampa(int i, int j)
    {
        //if (maze[i, j].ficha != null) Debug.Log($"trampa en {i},{j}");
        if (maze[i, j].trampa.tipo == "Freeze") maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        if (maze[i, j].trampa.tipo == "Damage") maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.red;
        if (maze[i, j].trampa.tipo == "Tele") maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    void HacerGoal(int i, int j)
    {
        maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    void ColocarCamara()
    {
        MainCamera.transform.position = new Vector3(maze.GetLength(0) / 2, maze.GetLength(0) / 2, -10);
        Camera.main.orthographicSize = (float)maze.GetLength(0) / 2;
    }

    public void PonerVerdeCasillasValidas(Ficha ficha)
    {
        (int x, int y) Start = (ficha.PosicionInicialTurno.fila, ficha.PosicionInicialTurno.columna);
        int[,] bfs = BFS(Start);
        for (int i = 0; i < bfs.GetLength(0); i++)
        {
            for (int j = 0; j < bfs.GetLength(1); j++)
            {
                if (bfs[i, j] <= ficha.team.velocidad)
                {
                    if (maze[i, j].EsCamino)
                        PonerVerde(i, j);
                }
            }
        }

    }

    void PonerVerde(int i, int j)
    {
        // maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.green;
        GameObject squareSel = Instantiate(squareSelection, new Vector2(i, j), Quaternion.identity);
        squareSelectionList.Add(squareSel);
    }

    public int[,] BFS((int, int) casillaInicio)
    {
        (int, int)[] directions = { (1, 0), (0, 1), (-1, 0), (0, -1) };
        int[,] bfs = new int[maze.GetLength(0), maze.GetLength(1)];

        bfs[casillaInicio.Item1, casillaInicio.Item2] = 0;

        for (int i = 0; i < bfs.GetLength(0); i++)
        {
            for (int j = 0; j < bfs.GetLength(1); j++)
            {
                if (!maze[i, j].EsCamino)
                {
                    bfs[i, j] = -1;
                }
                if (maze[i, j].EsCamino)
                {
                    bfs[i, j] = 0;
                }
            }
        }


        Queue<(int, int)> cola = new Queue<(int, int)>();

        cola.Enqueue(casillaInicio);

        while (cola.Count > 0)
        {
            (int, int) currCasilla = cola.Dequeue();
            int distancia = bfs[currCasilla.Item1, currCasilla.Item2];

            for (int i = 0; i < directions.Length; i++)
            {
                if (IsOnTheBounds((currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2)) && bfs[currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2] == 0)
                {
                    bfs[currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2] = distancia + 1;
                    cola.Enqueue((currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2));
                }
            }
        }

        return bfs;

        bool IsOnTheBounds((int, int) casilla)
        {
            return casilla.Item1 >= 0 && casilla.Item1 < bfs.GetLength(0) && casilla.Item2 >= 0 && casilla.Item2 < bfs.GetLength(1);
        }
    }

    public bool IsValidCasilla(Casilla destino, Ficha ficha)
    {
        Casilla inicioC = ficha.PosicionInicialTurno;
        if (inicioC == null) Debug.LogError($"Pinga la posicion inicial");
        (int, int) inicio = (inicioC.fila, inicioC.columna);
        (int, int) destinoC = (destino.fila, destino.columna);


        int[,] bfs = BFS(inicio);



        bool b = (bfs[destinoC.Item1, destinoC.Item2] > 0) && (bfs[destinoC.Item1, destinoC.Item2] <= ficha.Velocidad);
        if (!b)
            Debug.LogWarning(bfs[destinoC.Item1, destinoC.Item2]);
        return b;
    }

    void IdentificarFicha(Ficha ficha)
    {
        //ficha.fichaObj.GetComponent<SpriteRenderer>().color = ficha.team.colort;
        ficha.fichaObj.GetComponent<SpriteRenderer>().sprite = ficha.team.playerSprite;
    }

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        Generar();
        ColocarJugadores();
        ColocarTrampas();
        ColocarMeta();

        InstanciarLaberinto();
        PrintMaze();
        InstanciarJugadores();

        ColocarCamara();
    }

}

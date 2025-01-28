using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using URandom = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;
using System.IO;

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



    public List<GameObject> squareSelectionList = new List<GameObject>();


    //SPRITES
    public GameObject squareSelection;
    public GameObject squareGaige;
    public Sprite path1;

    public Sprite wallLimitDown, wallLimitLeft, wallLimitUp, wallLimitRight;

    public Sprite wallCornerDowLeft, wallCornerDowRight, wallCornerUpLeft, wallCornerUpRight;

    public Sprite tMinus90, tPlus90;
    public Sprite wallHorizontal, wallVertical;
    public Sprite wallT, wallX;
    public Sprite L, LReves;
    public Sprite PointUp, PointDown;
    public Sprite LMinus90, LMinus180;
    public Sprite PointRight, PointLeft;

    public Sprite TrampaDamage;
    public Sprite TrampaTele;
    public Sprite TrampaFreeze;
    public Sprite TrampaCoolDown;
    public Sprite TrampaSlowness;
    public Sprite TrampaLight;


    public GameObject Goal;

    public GameObject Key;




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
        int cantidadTrampas = 40; // Número de trampas que quieres colocar
        int intentos = 0;
        int cantidadBuff = 0;

        while (cantidadTrampas > 0 && intentos < 1000)
        {
            int x = rand.Next(1, filas - 1);
            int y = rand.Next(1, columnas - 1);




            if (maze[x, y].EsCamino && maze[x, y].trampa == null && maze[x, y].ficha == null && maze[x, y].key == null && !maze[x, y].isGoal)
            {
                int tipoTrampa;

                if (cantidadBuff >= 5)
                {
                    tipoTrampa = rand.Next(2, 6);
                }
                else
                {
                    tipoTrampa = rand.Next(0, 6);
                }

                switch (tipoTrampa)
                {
                    case 0:
                        maze[x, y].trampa = new Trampa(maze[x, y], "Tele");
                        cantidadBuff++;
                        break;
                    case 1:
                        maze[x, y].trampa = new Trampa(maze[x, y], "Light");
                        cantidadBuff++;
                        break;
                    case 2:
                        maze[x, y].trampa = new Trampa(maze[x, y], "Freeze");
                        break;
                    case 3:
                        maze[x, y].trampa = new Trampa(maze[x, y], "CoolDown");
                        break;
                    case 4:
                        maze[x, y].trampa = new Trampa(maze[x, y], "Slowness");
                        break;
                    case 5:
                        maze[x, y].trampa = new Trampa(maze[x, y], "Damage");
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
        Instantiate(Goal, new Vector2(15, 15), Quaternion.identity);
    }
    void ColocarKeys()
    {
        maze[15, 29].key = Instantiate(Key, new Vector2(15, 29), Quaternion.identity);
        maze[29, 15].key = Instantiate(Key, new Vector2(29, 15), Quaternion.identity);
        maze[15, 1].key = Instantiate(Key, new Vector2(15, 1), Quaternion.identity);
        maze[1, 15].key = Instantiate(Key, new Vector2(1, 15), Quaternion.identity);

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
                    //HacerGoal(i, j);
                }

            }


            foreach (GameObject square in squareSelectionList)
            {
                Destroy(square);
            }
        }
    }

    public void HacerCamino(int i, int j)
    {
        maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = path1;
    }
    void HacerPared(int i, int j)
    {
        switch (maze[i, j].spriteType)
        {
            case Casilla.SpriteType.wallLimitDown:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallLimitDown;
                break;
            case Casilla.SpriteType.wallLimitLeft:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallLimitLeft;
                break;
            case Casilla.SpriteType.wallLimitUp:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallLimitUp;
                break;
            case Casilla.SpriteType.wallLimitRight:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallLimitRight;
                break;
            case Casilla.SpriteType.wallCornerDowLeft:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallCornerDowLeft;
                break;
            case Casilla.SpriteType.wallCornerDowRight:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallCornerDowRight;
                break;
            case Casilla.SpriteType.wallCornerUpLeft:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallCornerUpLeft;
                break;
            case Casilla.SpriteType.wallCornerUpRight:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallCornerUpRight;
                break;
            case Casilla.SpriteType.tMinus90:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = tMinus90;
                break;
            case Casilla.SpriteType.tPlus90:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = tPlus90;
                break;
            case Casilla.SpriteType.wallHorizontal:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallHorizontal;
                break;
            case Casilla.SpriteType.wallVertical:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallVertical;
                break;
            case Casilla.SpriteType.wallT:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallT;
                break;
            case Casilla.SpriteType.wallX:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = wallX;
                break;
            case Casilla.SpriteType.L:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = L;
                break;
            case Casilla.SpriteType.LReves:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = LReves;
                break;
            case Casilla.SpriteType.PointUp:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = PointUp;
                break;
            case Casilla.SpriteType.PointDown:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = PointDown;
                break;
            case Casilla.SpriteType.LMinus90:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = LMinus90;
                break;
            case Casilla.SpriteType.LMinus180:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = LMinus180;
                break;
            case Casilla.SpriteType.PointRight:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = PointRight;
                break;
            case Casilla.SpriteType.PointLeft:
                maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = PointLeft;
                break;


            default:
                break;
        }
    }

    void HacerTrampa(int i, int j)
    {
        Sprite sprite = null;
        switch (maze[i, j].trampa.tipo)
        {
            case "Damage":
                sprite = TrampaDamage;
                break;
            case "Tele":
                sprite = TrampaTele;
                break;
            case "Freeze":
                sprite = TrampaFreeze;
                break;
            case "CoolDown":
                sprite = TrampaCoolDown;
                break;
            case "Slowness":
                sprite = TrampaSlowness;
                break;
            case "Light":
                sprite = TrampaLight;
                break;
        }
        if (maze[i, j].trampa.Actived || maze[i, j].trampa.tipo == "Tele" || maze[i, j].trampa.tipo == "Light")
            maze[i, j].casillaObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void HacerGoal(int i, int j)
    {
        //maze[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.red;

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
                if (bfs[i, j] <= ficha.Velocidad)
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

        bfs[casillaInicio.Item1, casillaInicio.Item2] = 0;

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

    void IdentificarCasilla(Casilla casilla)
    {
        int x = casilla.fila;
        int y = casilla.columna;
        //wallHorizontal
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino) casilla.spriteType = Casilla.SpriteType.wallHorizontal;

        //wallvertical
        if (!(y == 0 || y == maze.GetLength(0) - 1) && !maze[x, y - 1].EsCamino && !maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.wallVertical;

        //wallT
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino) casilla.spriteType = Casilla.SpriteType.wallT;


        //L
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino && maze[x, y - 1].EsCamino && !maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.L;

        //LReves
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].EsCamino && maze[x + 1, y].EsCamino && maze[x, y - 1].EsCamino && !maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.LReves;

        //PointUp
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].EsCamino && maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino && maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.PointUp;

        //PointDown
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].EsCamino && maze[x + 1, y].EsCamino && maze[x, y - 1].EsCamino && !maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.PointDown;

        //LMinus90
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino && maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.LMinus90;

        //LMinus180
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].EsCamino && maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino && maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.LMinus180;

        //PointLeft
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino && maze[x, y - 1].EsCamino && maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.PointLeft;

        //PointRight
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].EsCamino && maze[x + 1, y].EsCamino && maze[x, y - 1].EsCamino && maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.PointRight;




        //wallLimitDown
        if (y == 0) casilla.spriteType = Casilla.SpriteType.wallLimitDown;
        //wallLimitRigth
        if (x == maze.GetLength(1) - 1) casilla.spriteType = Casilla.SpriteType.wallLimitRight;
        //wallLimitUp
        if (y == maze.GetLength(0) - 1) casilla.spriteType = Casilla.SpriteType.wallLimitUp;
        //wallLimitLeft
        if (x == 0) casilla.spriteType = Casilla.SpriteType.wallLimitLeft;
        //tMinus90
        if ((casilla.spriteType == Casilla.SpriteType.wallLimitLeft && !maze[x + 1, y].EsCamino) || (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino && !maze[x, y + 1].EsCamino)) casilla.spriteType = Casilla.SpriteType.tMinus90;
        //tPlus90
        if ((x == maze.GetLength(1) - 1 && !maze[x - 1, y].EsCamino) || (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].EsCamino && maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino && !maze[x, y + 1].EsCamino)) casilla.spriteType = Casilla.SpriteType.tPlus90;

        //wallT
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0) && !maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino) casilla.spriteType = Casilla.SpriteType.wallT;
        //wallX
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].EsCamino && !maze[x + 1, y].EsCamino && !maze[x, y - 1].EsCamino && !maze[x, y + 1].EsCamino) casilla.spriteType = Casilla.SpriteType.wallX;




        //wallCornerDowLeft
        if (y == 0 && x == 0) casilla.spriteType = Casilla.SpriteType.wallCornerDowLeft;
        //wallCornerDowRight
        if (y == 0 && x == maze.GetLength(1) - 1) casilla.spriteType = Casilla.SpriteType.wallCornerDowRight;
        //wallCornerUpLeft
        if (y == maze.GetLength(0) - 1 && x == 0) casilla.spriteType = Casilla.SpriteType.wallCornerUpLeft;
        //wallCornerUpRigh
        if (y == maze.GetLength(0) - 1 && x == maze.GetLength(1) - 1) casilla.spriteType = Casilla.SpriteType.wallCornerUpRight;



    }

    void IdentificarCasillasLaberinto()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                IdentificarCasilla(maze[i, j]);
            }
        }
    }
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        Generar();
        ColocarJugadores();
        ColocarTrampas();
        ColocarMeta();
        ColocarKeys();


        IdentificarCasillasLaberinto();
        InstanciarLaberinto();
        PrintMaze();
        InstanciarJugadores();

        ColocarCamara();
    }

}

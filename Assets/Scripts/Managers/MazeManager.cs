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

    public static int rows = 31, columns = 31;
    public Tile[,] maze = new Tile[rows, columns];
    System.Random rand = new System.Random();
    List<(int, int, int, int)> walls = new List<(int, int, int, int)>();


    public GameManager gameManager;
    public PieceManager pieceManager;

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

    public Sprite TrapDamage;
    public Sprite TrapTele;
    public Sprite TrapFreeze;
    public Sprite TrapCoolDown;
    public Sprite TrapSlowness;
    public Sprite TrapLight;


    public GameObject Goal;

    public GameObject Key;




    void Generar()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Asume que todo es pared por defecto
                maze[i, j] = new Tile(false, i, j);
            }
        }


        // Selecciona una celda inicial en una posición impar
        int x = 15, y = 15;
        maze[x, y].isPath = true;  // Marca la celda como camino

        // Añade las walls iniciales de esta celda
        PutWalls(x, y);


        // Procesa las walls hasta que se acaben
        while (walls.Count > 0)
        {
            // Elige una pared al azar y la elimina de la lista
            int index = rand.Next(walls.Count);
            var (px, py, cx, cy) = walls[index];
            walls.RemoveAt(index);

            // Si la celda conectada no ha sido visitada
            if (!maze[cx, cy].isPath)
            {
                maze[px, py].isPath = true; // Elimina la pared entre las celdas
                maze[cx, cy].isPath = true; // Marca la nueva celda como camino
                PutWalls(cx, cy); // Añade las walls de la nueva celda
            }
        }


    }

    void PutWalls(int x, int y)
    {
        // Añade las walls de las celdas adyacentes (solo celdas impares)
        if (x > 1) walls.Add((x - 1, y, x - 2, y)); // Arriba
        if (x < rows - 2) walls.Add((x + 1, y, x + 2, y)); // Abajo
        if (y > 1) walls.Add((x, y - 1, x, y - 2)); // Izquierda
        if (y < columns - 2) walls.Add((x, y + 1, x, y + 2)); // Derecha
    }

    void PutTraps()
    {
        int amountOfTraps = 40; // Número de traps que quieres colocar
        int tries = 0;
        int amountOfBuffs = 0;

        while (amountOfTraps > 0 && tries < 1000)
        {
            int x = rand.Next(1, rows - 1);
            int y = rand.Next(1, columns - 1);




            if (maze[x, y].isPath && maze[x, y].trap == null && maze[x, y].piece == null && maze[x, y].key == null && !maze[x, y].isGoal)
            {
                int tipeTrap;

                if (amountOfBuffs >= 5)
                {
                    tipeTrap = rand.Next(2, 6);
                }
                else
                {
                    tipeTrap = rand.Next(0, 6);
                }

                switch (tipeTrap)
                {
                    case 0:
                        maze[x, y].trap = new Trap(maze[x, y], "Tele");
                        amountOfBuffs++;
                        break;
                    case 1:
                        maze[x, y].trap = new Trap(maze[x, y], "Light");
                        amountOfBuffs++;
                        break;
                    case 2:
                        maze[x, y].trap = new Trap(maze[x, y], "Freeze");
                        break;
                    case 3:
                        maze[x, y].trap = new Trap(maze[x, y], "CoolDown");
                        break;
                    case 4:
                        maze[x, y].trap = new Trap(maze[x, y], "Slowness");
                        break;
                    case 5:
                        maze[x, y].trap = new Trap(maze[x, y], "Damage");
                        break;
                }
                amountOfTraps--;
            }

            tries++;
        }
    }

    void PutPlayers()
    {
        List<(int x, int y)> listInitialPositions = GeneratePositions();
        listInitialPositions = ShuffleList(listInitialPositions);

        for (int i = 0; i < gameManager.users.Count; i++)
        {
            Piece piece = new Piece(gameManager.users[i]);

            piece.Position = maze[listInitialPositions[i].x, listInitialPositions[i].y];
            piece.SpawnTile = piece.Position;
            piece.PositionInitialTurn = piece.Position;


            maze[listInitialPositions[i].x, listInitialPositions[i].y].piece = piece;
            pieceManager.pieceList.Add(piece);

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

    void PutGoal()
    {
        maze[15, 15].isGoal = true;
        Instantiate(Goal, new Vector2(15, 15), Quaternion.identity);
    }
    void PutKeys()
    {
        maze[15, 29].key = Instantiate(Key, new Vector2(15, 29), Quaternion.identity);
        maze[15, 29].key.GetComponent<KeyScript>().currentTile = maze[15, 29];
        maze[29, 15].key = Instantiate(Key, new Vector2(29, 15), Quaternion.identity);
        maze[15, 29].key.GetComponent<KeyScript>().currentTile = maze[29, 15];
        maze[15, 1].key = Instantiate(Key, new Vector2(15, 1), Quaternion.identity);
        maze[15, 29].key.GetComponent<KeyScript>().currentTile = maze[15, 1];
        maze[1, 15].key = Instantiate(Key, new Vector2(1, 15), Quaternion.identity);
        maze[15, 29].key.GetComponent<KeyScript>().currentTile = maze[1, 15];

    }

    void InstantiateMaze()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Vector2 position = new Vector2(i, j);
                maze[i, j].tileObject = Instantiate(slot, position, Quaternion.identity);
                maze[i, j].clickTile = maze[i, j].tileObject.GetComponent<ClickTile>();
                maze[i, j].clickTile.tile = maze[i, j];
            }
        }

    }

    void InstantiatePlayers()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j].piece != null)
                {
                    Vector2 position = new Vector2(i, j);
                    maze[i, j].piece.pieceObject = Instantiate(player, position, Quaternion.identity);
                    maze[i, j].piece.clickPiece = maze[i, j].piece.pieceObject.GetComponent<ClickPiece>();
                    maze[i, j].piece.clickPiece.piece = maze[i, j].piece;

                    IdentifyPiece(maze[i, j].piece);
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
                if (maze[i, j].isPath)
                {
                    MakePath(i, j);
                }
                else
                {
                    MakeWall(i, j);
                }

                if (maze[i, j].trap != null)
                {
                    MakeTrap(i, j);
                }
                if (maze[i, j].isGoal)
                {
                    //MakeGoal(i, j);
                }

            }


            foreach (GameObject square in squareSelectionList)
            {
                Destroy(square);
            }
        }
    }

    public void MakePath(int i, int j)
    {
        maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = path1;
    }
    void MakeWall(int i, int j)
    {
        switch (maze[i, j].spriteType)
        {
            case Tile.SpriteType.wallLimitDown:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallLimitDown;
                break;
            case Tile.SpriteType.wallLimitLeft:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallLimitLeft;
                break;
            case Tile.SpriteType.wallLimitUp:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallLimitUp;
                break;
            case Tile.SpriteType.wallLimitRight:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallLimitRight;
                break;
            case Tile.SpriteType.wallCornerDowLeft:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallCornerDowLeft;
                break;
            case Tile.SpriteType.wallCornerDowRight:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallCornerDowRight;
                break;
            case Tile.SpriteType.wallCornerUpLeft:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallCornerUpLeft;
                break;
            case Tile.SpriteType.wallCornerUpRight:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallCornerUpRight;
                break;
            case Tile.SpriteType.tMinus90:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = tMinus90;
                break;
            case Tile.SpriteType.tPlus90:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = tPlus90;
                break;
            case Tile.SpriteType.wallHorizontal:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallHorizontal;
                break;
            case Tile.SpriteType.wallVertical:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallVertical;
                break;
            case Tile.SpriteType.wallT:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallT;
                break;
            case Tile.SpriteType.wallX:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = wallX;
                break;
            case Tile.SpriteType.L:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = L;
                break;
            case Tile.SpriteType.LReves:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = LReves;
                break;
            case Tile.SpriteType.PointUp:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = PointUp;
                break;
            case Tile.SpriteType.PointDown:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = PointDown;
                break;
            case Tile.SpriteType.LMinus90:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = LMinus90;
                break;
            case Tile.SpriteType.LMinus180:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = LMinus180;
                break;
            case Tile.SpriteType.PointRight:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = PointRight;
                break;
            case Tile.SpriteType.PointLeft:
                maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = PointLeft;
                break;


            default:
                break;
        }
    }

    void MakeTrap(int i, int j)
    {
        Sprite sprite = null;
        switch (maze[i, j].trap.tipe)
        {
            case "Damage":
                sprite = TrapDamage;
                break;
            case "Tele":
                sprite = TrapTele;
                break;
            case "Freeze":
                sprite = TrapFreeze;
                break;
            case "CoolDown":
                sprite = TrapCoolDown;
                break;
            case "Slowness":
                sprite = TrapSlowness;
                break;
            case "Light":
                sprite = TrapLight;
                break;
        }
        if (maze[i, j].trap.Actived || maze[i, j].trap.tipe == "Tele" || maze[i, j].trap.tipe == "Light")
            maze[i, j].tileObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void MakeGoal(int i, int j)
    {
        //maze[i, j].tileObject.GetComponent<SpriteRenderer>().color = Color.red;

    }

    void PutCamera()
    {
        MainCamera.transform.position = new Vector3(maze.GetLength(0) / 2, maze.GetLength(0) / 2, -10);
        Camera.main.orthographicSize = (float)maze.GetLength(0) / 2;
    }

    public void Show_Valid_Tiles(Piece piece)
    {
        (int x, int y) Start = (piece.PositionInitialTurn.row, piece.PositionInitialTurn.column);
        int[,] bfs = BFS(Start);
        for (int i = 0; i < bfs.GetLength(0); i++)
        {
            for (int j = 0; j < bfs.GetLength(1); j++)
            {
                if (bfs[i, j] <= piece.Speed)
                {
                    if (maze[i, j].isPath)
                        PutValid(i, j);
                }
            }
        }

    }

    void PutValid(int i, int j)
    {
        // maze[i, j].tileObject.GetComponent<SpriteRenderer>().color = Color.green;
        GameObject squareSel = Instantiate(squareSelection, new Vector2(i, j), Quaternion.identity);
        squareSelectionList.Add(squareSel);
    }

    public int[,] BFS((int, int) initialTile)
    {
        (int, int)[] directions = { (1, 0), (0, 1), (-1, 0), (0, -1) };
        int[,] bfs = new int[maze.GetLength(0), maze.GetLength(1)];

        bfs[initialTile.Item1, initialTile.Item2] = 0;

        for (int i = 0; i < bfs.GetLength(0); i++)
        {
            for (int j = 0; j < bfs.GetLength(1); j++)
            {
                if (!maze[i, j].isPath)
                {
                    bfs[i, j] = -1;
                }
                if (maze[i, j].isPath)
                {
                    bfs[i, j] = 0;
                }
            }
        }


        Queue<(int, int)> queue = new Queue<(int, int)>();

        queue.Enqueue(initialTile);

        while (queue.Count > 0)
        {
            (int, int) currTile = queue.Dequeue();
            int distance = bfs[currTile.Item1, currTile.Item2];

            for (int i = 0; i < directions.Length; i++)
            {
                if (IsOnTheBounds((currTile.Item1 + directions[i].Item1, +currTile.Item2 + directions[i].Item2)) && bfs[currTile.Item1 + directions[i].Item1, +currTile.Item2 + directions[i].Item2] == 0)
                {
                    bfs[currTile.Item1 + directions[i].Item1, +currTile.Item2 + directions[i].Item2] = distance + 1;
                    queue.Enqueue((currTile.Item1 + directions[i].Item1, +currTile.Item2 + directions[i].Item2));
                }
            }
        }

        bfs[initialTile.Item1, initialTile.Item2] = 0;

        return bfs;

        bool IsOnTheBounds((int, int) Tile)
        {
            return Tile.Item1 >= 0 && Tile.Item1 < bfs.GetLength(0) && Tile.Item2 >= 0 && Tile.Item2 < bfs.GetLength(1);
        }
    }

    public bool IsValidTile(Tile destino, Piece piece)
    {
        Tile initialTile = piece.PositionInitialTurn;

        (int, int) startTile = (initialTile.row, initialTile.column);
        (int, int) finalTile = (destino.row, destino.column);


        int[,] bfs = BFS(startTile);



        bool b = (bfs[finalTile.Item1, finalTile.Item2] > 0) && (bfs[finalTile.Item1, finalTile.Item2] <= piece.Speed);
        if (!b)
            Debug.LogWarning(bfs[finalTile.Item1, finalTile.Item2]);
        return b;
    }

    void IdentifyPiece(Piece piece)
    {
        //piece.pieceObject.GetComponent<SpriteRenderer>().color = piece.team.colort;
        piece.pieceObject.GetComponent<SpriteRenderer>().sprite = piece.team.playerSprite;
    }

    void IdentifyTile(Tile Tile)
    {
        int x = Tile.row;
        int y = Tile.column;
        //wallHorizontal
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !maze[x - 1, y].isPath && !maze[x + 1, y].isPath) Tile.spriteType = Tile.SpriteType.wallHorizontal;

        //wallvertical
        if (!(y == 0 || y == maze.GetLength(0) - 1) && !maze[x, y - 1].isPath && !maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.wallVertical;

        //wallT
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].isPath && !maze[x + 1, y].isPath && !maze[x, y - 1].isPath) Tile.spriteType = Tile.SpriteType.wallT;


        //L
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].isPath && !maze[x + 1, y].isPath && maze[x, y - 1].isPath && !maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.L;

        //LReves
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].isPath && maze[x + 1, y].isPath && maze[x, y - 1].isPath && !maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.LReves;

        //PointUp
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].isPath && maze[x + 1, y].isPath && !maze[x, y - 1].isPath && maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.PointUp;

        //PointDown
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].isPath && maze[x + 1, y].isPath && maze[x, y - 1].isPath && !maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.PointDown;

        //LMinus90
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].isPath && !maze[x + 1, y].isPath && !maze[x, y - 1].isPath && maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.LMinus90;

        //LMinus180
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].isPath && maze[x + 1, y].isPath && !maze[x, y - 1].isPath && maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.LMinus180;

        //PointLeft
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].isPath && !maze[x + 1, y].isPath && maze[x, y - 1].isPath && maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.PointLeft;

        //PointRight
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].isPath && maze[x + 1, y].isPath && maze[x, y - 1].isPath && maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.PointRight;




        //wallLimitDown
        if (y == 0) Tile.spriteType = Tile.SpriteType.wallLimitDown;
        //wallLimitRigth
        if (x == maze.GetLength(1) - 1) Tile.spriteType = Tile.SpriteType.wallLimitRight;
        //wallLimitUp
        if (y == maze.GetLength(0) - 1) Tile.spriteType = Tile.SpriteType.wallLimitUp;
        //wallLimitLeft
        if (x == 0) Tile.spriteType = Tile.SpriteType.wallLimitLeft;
        //tMinus90
        if ((Tile.spriteType == Tile.SpriteType.wallLimitLeft && !maze[x + 1, y].isPath) || (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && maze[x - 1, y].isPath && !maze[x + 1, y].isPath && !maze[x, y - 1].isPath && !maze[x, y + 1].isPath)) Tile.spriteType = Tile.SpriteType.tMinus90;
        //tPlus90
        if ((x == maze.GetLength(1) - 1 && !maze[x - 1, y].isPath) || (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].isPath && maze[x + 1, y].isPath && !maze[x, y - 1].isPath && !maze[x, y + 1].isPath)) Tile.spriteType = Tile.SpriteType.tPlus90;

        //wallT
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0) && !maze[x - 1, y].isPath && !maze[x + 1, y].isPath && !maze[x, y - 1].isPath) Tile.spriteType = Tile.SpriteType.wallT;
        //wallX
        if (!(x == 0 || x == maze.GetLength(1) - 1) && !(y == 0 || y == maze.GetLength(0) - 1) && !maze[x - 1, y].isPath && !maze[x + 1, y].isPath && !maze[x, y - 1].isPath && !maze[x, y + 1].isPath) Tile.spriteType = Tile.SpriteType.wallX;




        //wallCornerDowLeft
        if (y == 0 && x == 0) Tile.spriteType = Tile.SpriteType.wallCornerDowLeft;
        //wallCornerDowRight
        if (y == 0 && x == maze.GetLength(1) - 1) Tile.spriteType = Tile.SpriteType.wallCornerDowRight;
        //wallCornerUpLeft
        if (y == maze.GetLength(0) - 1 && x == 0) Tile.spriteType = Tile.SpriteType.wallCornerUpLeft;
        //wallCornerUpRigh
        if (y == maze.GetLength(0) - 1 && x == maze.GetLength(1) - 1) Tile.spriteType = Tile.SpriteType.wallCornerUpRight;



    }

    void IdentifyTilesMaze()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                IdentifyTile(maze[i, j]);
            }
        }
    }
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        Generar();
        PutPlayers();
        PutTraps();
        PutGoal();
        PutKeys();


        IdentifyTilesMaze();
        InstantiateMaze();
        PrintMaze();
        InstantiatePlayers();

        PutCamera();
    }

}

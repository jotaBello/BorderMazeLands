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

    public int speed;
    public Sprite playerSprite;
    public int cooldown;





    public int life;


    public void Ability(Piece piece)
    {
        switch (teamName)
        {
            case "Maya":
                MayaAbility(piece);
                break;
            case "Axton":
                AxtonAbility(piece);
                break;
            case "Zero":
                ZeroAbility(piece);
                break;
            case "Krieg":
                KriegAbility(piece);
                break;
            case "Gaige":
                GaigeAbility(piece);
                break;
            case "Salvador":
                SalvadorAbility(piece);
                break;
            default:
                Debug.LogError("Ability from an Unknow Team");
                break;
        }
    }

    void MayaAbility(Piece piece)
    {
        HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();
        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();

        Tile[,] maze = mazeManager.maze;
        List<Piece> listNearEnemies = FindNearEnemies(piece);

        foreach (Piece p in listNearEnemies)
        {
            p.freeze = 3;
            hudManager.PutMessage($"Congelaste a {p.team.name}");
        }



        List<Piece> FindNearEnemies(Piece piece)
        {
            List<Piece> listNearEnemies = new List<Piece>();
            Tile tile = piece.Position;

            (int, int) playerTile = (tile.row, tile.column);

            int[,] bfs = mazeManager.BFS(playerTile);


            for (int i = 0; i < bfs.GetLength(0); i++)
            {
                for (int j = 0; j < bfs.GetLength(1); j++)
                {

                    if (maze[i, j].piece != null)
                    {
                        if (bfs[i, j] <= 4)
                        {
                            if (maze[i, j].piece != piece)
                            {
                                listNearEnemies.Add(maze[i, j].piece);
                            }
                        }
                    }

                }
            }



            return listNearEnemies;
        }

    }

    void AxtonAbility(Piece piece)
    {
        HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();
        piece.shield = true;
        piece.shieldTime = 3;
        hudManager.PutMessage($"Activaste tu escudo");
    }

    void ZeroAbility(Piece piece)
    {
        HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();

        (int, int)[] directions =
        {
        (0,-1),
        (1,0),
        (0,1),
        (-1,0)

    };

        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        Tile[,] maze = mazeManager.maze;


        Tile tile = piece.Position;
        (int, int) playerTile = (tile.row, tile.column);
        List<Piece> enemies = FindEnemies(playerTile);


        foreach (Piece enemie in enemies)
        {
            if (enemie != piece)
            {
                enemie.life -= 5;
                hudManager.PutMessage($"Dañaste a {enemie.team.name}");
            }
        }

        List<Piece> FindEnemies((int, int) tile)
        {
            List<Piece> enemies = new List<Piece>();

            foreach (var dir in directions)
            {
                int row = tile.Item1 + dir.Item1;
                int column = tile.Item2 + dir.Item2;

                while (row >= 0 && row < maze.GetLength(0) && column >= 0 && column < maze.GetLength(1) && maze[row, column].isPath)
                {
                    if (maze[row, column].piece != null)
                    {
                        enemies.Add(maze[row, column].piece);
                    }
                    row += dir.Item1;
                    column += dir.Item2;
                }
            }


            return enemies;
        }

    }

    void KriegAbility(Piece piece)
    {
        HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();

        (int, int)[] directions =
        {
        (0,-1),
        (1,0),
        (0,1),
        (-1,0)

    };

        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        Tile[,] maze = mazeManager.maze;

        Tile tile = piece.Position;
        (int, int) playerTile = (tile.row, tile.column);

        foreach (var dir in directions)
        {
            int row = playerTile.Item1 + dir.Item1;
            int column = playerTile.Item2 + dir.Item2;

            if (row > 0 && row < maze.GetLength(0) - 1 && column > 0 && column < maze.GetLength(1) - 1)
            {
                if (!maze[row, column].isPath)
                {
                    maze[row, column].isPath = true;
                    mazeManager.MakePath(row, column);
                    hudManager.PutMessage($"EXPLOSIONEES");
                }
            }
        }
        mazeManager.Show_Valid_Tiles(piece);
    }

    void SalvadorAbility(Piece piece)
    {
        HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();
        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();


        piece.Speed += 2;
        piece.lighttime = 1;
        piece.pieceObject.GetComponent<Light2D>().pointLightOuterRadius *= 1.5f;
        piece.pieceObject.GetComponent<Light2D>().pointLightInnerRadius *= 1.5f;
        mazeManager.Show_Valid_Tiles(piece);

        hudManager.PutMessage($"Aumentaste tu vision y velocidad");

    }

    void GaigeAbility(Piece piece)
    {
        HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();
        (int, int)[] directions =
                {
        (0,-1),
        (1,0),
        (0,1),
        (-1,0)

    };
        //Debug.LogError("Paso1");
        MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();

        Tile[,] maze = mazeManager.maze;

        Tile goal = piece.Position;
        List<(Tile key, int distance)> keys = new List<(Tile keys, int distance)>();
        Tile key = piece.Position;

        int[,] bfs = mazeManager.BFS((piece.Position.row, piece.Position.column));

        foreach (Tile tile in maze)
        {
            if (tile.isGoal) goal = tile;
            else if (tile.key != null) keys.Add((tile, bfs[tile.row, tile.column]));
        }



        key = keys[0].key;
        foreach (var Key in keys)
        {
            if (Key.distance < bfs[key.row, key.column])
            {
                key = Key.key;
            }
        }



        if (piece.HadKey)
        {
            List<Tile> paths = GiveMeThePaths(piece.Position, goal);
            ShowTiles(paths);
            hudManager.PutMessage($"Este es el camino hacia la Camara");
        }
        else
        {
            List<Tile> paths = GiveMeThePaths(piece.Position, key);
            // Debug.LogError("Paso2");
            ShowTiles(paths);
            hudManager.PutMessage($"Este es el camino hacia la LLave");
        }





        void ShowTiles(List<Tile> paths)
        {
            // Debug.LogError("Paso3");

            if (paths.Count >= 3)
            {

                GameObject squareGaige1 = Instantiate(mazeManager.squareGaige, new Vector2(paths[paths.Count - 2].row, paths[paths.Count - 2].column), Quaternion.identity);
                mazeManager.squareSelectionList.Add(squareGaige1);
                GameObject squareGaige2 = Instantiate(mazeManager.squareGaige, new Vector2(paths[paths.Count - 3].row, paths[paths.Count - 3].column), Quaternion.identity);
                mazeManager.squareSelectionList.Add(squareGaige2);
                //Debug.LogError("Paso4");

            }
            else if (paths.Count == 2)
            {

                GameObject squareGaige1 = Instantiate(mazeManager.squareGaige, new Vector2(paths[paths.Count - 2].row, paths[paths.Count - 2].column), Quaternion.identity);
                mazeManager.squareSelectionList.Add(squareGaige1);
                // Debug.LogError("Paso4");
            }
        }





        List<Tile> GiveMeThePaths(Tile start, Tile final)
        {
            int distance = bfs[final.row, final.column];
            Tile current = final;
            List<Tile> paths = new List<Tile>();


            while (distance > 0)
            {

                paths.Add(The_Min_Ady_Tile(current));
                current = paths[paths.Count - 1];
                distance = bfs[current.row, current.column];
            }

            return paths;

        }

        Tile The_Min_Ady_Tile(Tile current)
        {

            List<Tile> adys = new List<Tile>();
            Tile possible = null;
            int possibleDistance = int.MaxValue;

            foreach (var dir in directions)
            {
                adys.Add(maze[current.row + dir.Item1, current.column + dir.Item2]);
            }
            foreach (var ady in adys)
            {
                if (ady.isPath && bfs[ady.row, ady.column] < possibleDistance)
                {
                    possibleDistance = bfs[ady.row, ady.column];
                    possible = ady;
                }
            }
            if (possible == null) Debug.LogError("Possible null");
            return possible;

        }

    }

}

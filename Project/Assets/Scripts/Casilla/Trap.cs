using UnityEngine;
using UnityEngine.Rendering.Universal;
using URandom = UnityEngine.Random;

public class Trap
{
    public Tile associatedTile;
    public Tile linkedTile;
    public string tipe;
    public bool Actived;

    MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
    PieceManager pieceManager = GameObject.Find("PieceManager").GetComponent<PieceManager>();
    HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();

    public Trap(Tile tile, string tipe)
    {
        associatedTile = tile;
        this.tipe = tipe;

        if (tipe == "Tele")
        {
            linkedTile = Search_a_random_tile_for_TeleTrap();

        }
    }


    public void Activate(Piece piece)
    {
        switch (tipe)
        {
            case "Tele":
                TrapTele(piece);
                associatedTile.trap = null;
                break;
            case "Damage":
                TrapDamage(piece);
                if (!piece.shield)
                    hudManager.PutMessage("Activaste una trampa de Daño");
                break;
            case "Freeze":
                TrapFreeze(piece);
                if (!piece.shield)
                    hudManager.PutMessage("Activaste una trampa de Congelamiento");
                break;
            case "CoolDown":
                TrapCoolDown(piece);
                if (!piece.shield)
                    hudManager.PutMessage("Activaste una trampa de Habilidad");
                break;
            case "Slowness":
                TrapSlowness(piece);
                if (!piece.shield)
                    hudManager.PutMessage("Activaste una trampa de Lentitud");
                break;

            case "Light":
                TrapLight(piece);
                associatedTile.trap = null;
                break;
        }



    }

    void TrapTele(Piece piece)
    {
        pieceManager.MovePiece(piece, linkedTile);

    }
    void TrapDamage(Piece piece)
    {
        int damage = URandom.Range(1, 4);

        if (!piece.shield)
            piece.life -= damage;

    }
    void TrapFreeze(Piece piece)
    {
        int time = URandom.Range(3, 5);

        if (!piece.shield)
            piece.freeze = time;

    }
    void TrapCoolDown(Piece piece)
    {
        if (!piece.shield)
            piece.cooldown = piece.team.cooldown;
    }
    void TrapSlowness(Piece piece)
    {
        if (!piece.shield)
        {
            piece.Speed -= 2;
            piece.slowness = 2;
        }
    }
    void TrapLight(Piece piece)
    {
        piece.lighttime = 2;
        piece.pieceObject.GetComponent<Light2D>().pointLightOuterRadius *= 1.5f;
        piece.pieceObject.GetComponent<Light2D>().pointLightInnerRadius *= 1.5f;
    }


    Tile Search_a_random_tile_for_TeleTrap()
    {
        int x, y;
        Tile[,] maze = mazeManager.maze;


        do
        {
            x = URandom.Range(1, maze.GetLength(0) - 1);
            y = URandom.Range(1, maze.GetLength(1) - 1);


        } while (!maze[x, y].isPath || maze[x, y].trap != null || maze[x, y].piece != null);
        return maze[x, y];
    }





}

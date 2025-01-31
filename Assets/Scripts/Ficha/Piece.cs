using UnityEngine;
public class Piece
{
    public int Speed;
    public Tile Position;
    public Tile PositionInitialTurn;
    public Tile SpawnTile;
    public Teams team;
    public ClickPiece clickPiece;

    public int freeze;
    public int slowness;
    public int lighttime;
    public int life;
    public bool shield;
    public int shieldTime;

    public GameObject pieceObject;

    public bool Moved;

    public bool HadKey;
    public KeyScript key;
    public int cooldown;




    public Piece(Teams team)
    {
        this.team = team;
        life = team.life;
        Speed = team.speed;
        freeze = 0;
        slowness = 0;
        shield = false;
        cooldown = team.cooldown;
    }





}

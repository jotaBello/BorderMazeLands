public class Casilla
{
    public bool EsCamino { get; set; }
    public bool EsPared { get; set; }
    public Jugador Jugador { get; set; } // Si hay un jugador en la casilla

    public Casilla(bool esCamino, bool esPared)
    {
        EsCamino = esCamino;
        EsPared = esPared;
        Jugador = null; // No hay jugador al principio
    }
}


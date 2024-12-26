public class Casilla
{
    public bool EsCamino { get; set; }
    //public Jugador Jugador { get; set; } // Si hay un jugador en la casilla

    public Casilla(bool esCamino)
    {
        EsCamino = esCamino;
        //Jugador = null; // No hay jugador al principio
    }
}


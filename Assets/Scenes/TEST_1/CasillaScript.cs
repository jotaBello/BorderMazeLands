public class Casilla
{
    public int fila;
    public int columna;
    public bool EsCamino { get; set; }
    public Jugador Jugador { get; set; } // Si hay un jugador en la casilla

    public Casilla(bool esCamino, int f, int c)
    {
        fila = c;
        columna = f;
        EsCamino = esCamino;
        Jugador = null; // No hay jugador al principio
    }
}


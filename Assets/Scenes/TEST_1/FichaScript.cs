public class Ficha
{
    public string Nombre { get; set; }
    public int Velocidad { get; set; }
    public int HabilidadEnfriamiento { get; set; } // Tiempo de enfriamiento para la habilidad
    public Casilla Posicion { get; set; }

    public Ficha(string nombre, int velocidad, int habilidadEnfriamiento)
    {
        Nombre = nombre;
        Velocidad = velocidad;
        HabilidadEnfriamiento = habilidadEnfriamiento;
    }
}

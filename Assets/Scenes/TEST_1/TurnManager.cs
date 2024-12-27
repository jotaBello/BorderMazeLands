using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Jugador> jugadores;
    public int turnoActual;

    private void Start()
    {
        // Inicializa la lista de jugadores (puedes agregarlos manualmente o desde otro lugar)
        jugadores = new List<Jugador>();
        jugadores.Add(new Jugador("Jugador1"));
        jugadores.Add(new Jugador("Jugador2"));

        // Comienza el turno del primer jugador
        turnoActual = 0;
        //IniciarTurno();
    }

    private void IniciarTurno()
    {
        Debug.Log($"Turno de {jugadores[turnoActual].Nombre}");

        


        FinalizarTurno();
    }

    public void FinalizarTurno()
    {
        // Aquí puedes realizar acciones al final del turno (por ejemplo, verificar condiciones de victoria, etc.).

        // Cambia al siguiente jugador
        turnoActual = (turnoActual + 1) % jugadores.Count;
        IniciarTurno();
    }
}


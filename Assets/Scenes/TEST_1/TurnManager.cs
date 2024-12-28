using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<Teams> equipos;
    public int turnoActual;

    MazeGeneration mazeGeneration;

    public Ficha fichaSelecc = null;

    (int, int)[] directions = { (1, 0), (0, 1), (-1, 0), (0, -1) };

    private void Start()
    {
        mazeGeneration = GameObject.Find("MazeGen").GetComponent<MazeGeneration>();
        // Inicializa la lista de equipos (puedes agregarlos manualmente o desde otro lugar)
        equipos = new List<Teams>();

        equipos = GameObject.Find("GameManager").GetComponent<GameManager>().users;

        // Comienza el turno del primer jugador
        turnoActual = 0;
        IniciarTurno();
    }

    private void IniciarTurno()
    {
        Debug.Log($"Turno de {equipos[turnoActual].teamName}");
    }

    public void FinalizarTurno()
    {
        // Aquí puedes realizar acciones al final del turno (por ejemplo, verificar condiciones de victoria, etc.).

        // Cambia al siguiente jugador
        turnoActual = (turnoActual + 1) % equipos.Count;
        IniciarTurno();
    }

    public void SeleccionarFicha(Ficha fichaSel)
    {
        fichaSelecc = fichaSel;
        Debug.Log($"seleccionada en turnmanager la ficha{fichaSelecc.team.teamName}");
    }

    public void MoverFicha(Casilla destino)
    {
        if (fichaSelecc != null)
        {
            if (fichaSelecc.team == equipos[turnoActual] && IsValidCasilla(destino, fichaSelecc))
            {
                fichaSelecc.fichaObj.transform.position = destino.casillaObject.transform.position;
            }
        }
    }

    bool IsValidCasilla(Casilla destino, Ficha fich)
    {
        //(int, int) inicio = (fich.Posicion.fila, fich.Posicion.columna);
        (int, int) inicio = (1, 1);
        (int, int) destinoC = (destino.fila, destino.columna);

        int[,] bfs = BFS(inicio);

        bool b = (bfs[destinoC.Item1, destinoC.Item2] > 0) && (bfs[destinoC.Item1, destinoC.Item2] <= fich.Velocidad);

        return b;
    }

    int[,] BFS((int, int) casillaInicio)
    {
        Casilla[,] maze = mazeGeneration.laberinto;

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
            }
        }


        Queue<(int, int)> cola = new Queue<(int, int)>();

        cola.Enqueue(casillaInicio);

        while (cola.Count > 0)
        {
            (int, int) currCasilla = cola.Dequeue();
            int distancia = bfs[currCasilla.Item1, currCasilla.Item2];

            foreach (var direction in directions)
            {
                if (IsOnTheBounds((currCasilla.Item1 + direction.Item1, +currCasilla.Item2 + direction.Item2)) && bfs[currCasilla.Item1 + direction.Item1, +currCasilla.Item2 + direction.Item2] != -1)
                {
                    bfs[currCasilla.Item1 + direction.Item1, +currCasilla.Item2 + direction.Item2] = distancia + 1;
                    cola.Enqueue((currCasilla.Item1 + direction.Item1, +currCasilla.Item2 + direction.Item2));
                }
            }
        }

        return bfs;

        bool IsOnTheBounds((int, int) casilla)
        {
            return casilla.Item1 >= 0 && casilla.Item1 < bfs.GetLength(0) && casilla.Item2 >= 0 && casilla.Item2 < bfs.GetLength(1);
        }
    }


}



using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<Teams> equipos;
    public int turnoActual;
    MazeGeneration mazeGeneration;
    public MazeInstantiater mazeInst;
    public Ficha fichaSelecc = null;

    static (int, int)[] directions = { (1, 0), (0, 1), (-1, 0), (0, -1) };

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FinalizarTurno();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (fichaSelecc != null)
            {
                fichaSelecc.team.Habilidad(fichaSelecc);
            }
        }
    }

    private void IniciarTurno()
    {
        Debug.Log($"Turno de {equipos[turnoActual].teamName}");

        ActualizarPosiciones();
    }

    public void FinalizarTurno()
    {
        // Aquí puedes realizar acciones al final del turno (por ejemplo, verificar condiciones de victoria, etc.).

        CheckTraps();
        CheckLife();
        UpdateFreeze();

        // Cambia al siguiente jugador
        turnoActual = (turnoActual + 1) % equipos.Count;
        IniciarTurno();
    }

    public void SeleccionarFicha(Ficha fichaSel)
    {

        if (fichaSel.freeze <= 0)
        {
            PonerVerdeCasillasValidas(fichaSel);
            fichaSelecc = fichaSel;
            Debug.Log($"seleccionada en turnmanager la ficha{fichaSelecc.team.teamName}");
        }

        else
        {

        }

    }

    public void MoverFicha(Casilla destino)
    {
        if (fichaSelecc != null)
        {
            if (fichaSelecc.team == equipos[turnoActual] && IsValidCasilla(destino, fichaSelecc))
            {
                fichaSelecc.fichaObj.GetComponent<SeleccionarFicha>().casillaPosicion.ficha = null;

                fichaSelecc.fichaObj.transform.position = destino.casillaObject.transform.position;
                fichaSelecc.fichaObj.GetComponent<SeleccionarFicha>().casillaPosicion = mazeGeneration.laberinto[-((int)fichaSelecc.fichaObj.transform.position.y - mazeGeneration.laberinto.GetLength(0) / 2), (int)fichaSelecc.fichaObj.transform.position.x + mazeGeneration.laberinto.GetLength(1) / 2];

                destino.ficha = fichaSelecc;
            }



        }
        PonerNegrasCasillas();
    }

    bool IsValidCasilla(Casilla destino, Ficha fich)
    {
        Casilla inicioC = fich.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno;
        if (inicioC == null) Debug.LogError($"Pinga la posicion inicial");
        (int, int) inicio = (inicioC.fila, inicioC.columna);
        (int, int) destinoC = (destino.fila, destino.columna);


        int[,] bfs = BFS(inicio);



        bool b = (bfs[destinoC.Item1, destinoC.Item2] > 0) && (bfs[destinoC.Item1, destinoC.Item2] <= fich.Velocidad);
        if (!b)
            Debug.LogWarning(bfs[destinoC.Item1, destinoC.Item2]);
        return b;
    }

    public int[,] BFS((int, int) casillaInicio)
    {
        Debug.Log("entro al bfs");
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
                if (maze[i, j].EsCamino)
                {
                    bfs[i, j] = 0;
                }
            }
        }


        Queue<(int, int)> cola = new Queue<(int, int)>();

        cola.Enqueue(casillaInicio);

        while (cola.Count > 0)
        {
            (int, int) currCasilla = cola.Dequeue();
            int distancia = bfs[currCasilla.Item1, currCasilla.Item2];

            for (int i = 0; i < directions.Length; i++)
            {
                if (IsOnTheBounds((currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2)) && bfs[currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2] == 0)
                {
                    bfs[currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2] = distancia + 1;
                    cola.Enqueue((currCasilla.Item1 + directions[i].Item1, +currCasilla.Item2 + directions[i].Item2));
                }
            }
        }
        Debug.LogWarning("salio del bfs");
        return bfs;

        bool IsOnTheBounds((int, int) casilla)
        {
            return casilla.Item1 >= 0 && casilla.Item1 < bfs.GetLength(0) && casilla.Item2 >= 0 && casilla.Item2 < bfs.GetLength(1);
        }
    }


    public void PonerVerdeCasillasValidas(Ficha fic)
    {
        Casilla inicio = fic.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno;
        Debug.LogWarning($"Ficha {fic.team.teamName} ahora su posicionInicialTurno es ({fic.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno.fila}, {fic.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno.columna})");
        int[,] bfs = BFS((inicio.fila, inicio.columna));
        for (int i = 0; i < bfs.GetLength(0); i++)
        {
            for (int j = 0; j < bfs.GetLength(1); j++)
            {
                if (bfs[i, j] <= fic.team.velocidad)
                {
                    if (mazeGeneration.laberinto[i, j].EsCamino)
                        mazeGeneration.laberinto[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }

    public void PonerNegrasCasillas()
    {
        for (int i = 0; i < mazeGeneration.laberinto.GetLength(0); i++)
        {
            for (int j = 0; j < mazeGeneration.laberinto.GetLength(1); j++)
            {
                if (mazeGeneration.laberinto[i, j].EsCamino && !(i == 15 && j == 15) && mazeGeneration.laberinto[i, j].trampa == null)
                {
                    mazeGeneration.laberinto[i, j].casillaObject.GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
    }

    void ActualizarPosiciones()
    {
        Debug.Log($"actualice la lista de fichas de tama;o {mazeInst.fichaList.Count}");
        for (int i = 0; i < mazeInst.fichaList.Count; i++)
        {
            mazeInst.fichaList[i].fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno = mazeInst.fichaList[i].fichaObj.GetComponent<SeleccionarFicha>().casillaPosicion;
        }
    }

    void CheckTraps()
    {

        // Activar la trampa si existe
        foreach (Ficha fich in mazeInst.fichaList)
        {
            if (fich.team == equipos[turnoActual])
            {
                if (fich.fichaObj.GetComponent<SeleccionarFicha>().casillaPosicion.trampa != null)
                {
                    fich.fichaObj.GetComponent<SeleccionarFicha>().casillaPosicion.trampa.Activar(fich);

                    Debug.Log($"Vida restante  : {fich.vida}");
                    Debug.Log($"Velocidad restante  : {fich.Velocidad}");
                    Debug.Log($"Freeze time  : {fich.freeze}");
                }
            }

        }
    }

    void CheckLife()
    {
        foreach (Ficha fich in mazeInst.fichaList)
        {
            if (fich.vida <= 0)
            {
                Casilla spawn = fich.fichaObj.GetComponent<SeleccionarFicha>().casillaPosicionSpawn;
                fich.fichaObj.transform.position = spawn.casillaObject.transform.position;
                fich.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno = spawn;
                fich.fichaObj.GetComponent<SeleccionarFicha>().casillaPosicion = spawn;
                fich.vida = fich.team.vida;
            }
        }

    }

    void UpdateFreeze()
    {
        foreach (Ficha fich in mazeInst.fichaList)
        {
            if (fich.team == equipos[turnoActual])
            {
                if (fich.freeze > 0) fich.freeze--;
            }

        }
    }

}




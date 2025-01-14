using UnityEngine;
using URandom = UnityEngine.Random;

public class Trampa
{
    public Casilla casillaAsociada;
    public string tipo;
    MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
    FichaManager fichaManager = GameObject.Find("FichaManager").GetComponent<FichaManager>();

    public Trampa(Casilla casilla, string tipo)
    {
        casillaAsociada = casilla;
        this.tipo = tipo;
    }


    public void Activar(Ficha ficha)
    {
        switch (tipo)
        {
            case "Tele":
                TrampaTele(ficha);
                casillaAsociada.trampa = null;
                break;
            case "Damage":
                TrampaDamage(ficha);
                break;
            case "Freeze":
                TrampaFreeze(ficha);
                break;
        }

    }

    void TrampaTele(Ficha ficha)
    {

        Casilla destino = BuscarCasillaAleatoria();



        if (!ficha.shield)
            if (destino != null && destino.EsCamino && destino.ficha == null)
            {
                fichaManager.MoverFicha(ficha, destino);
                Debug.Log($"Ficha {ficha.team.teamName} teletransportada a ({destino.fila}, {destino.columna})");
            }
            else
            {
                Debug.LogWarning("Destino de teletransporte inválido");
            }


        Casilla BuscarCasillaAleatoria()
        {
            int x, y;
            Casilla[,] maze = mazeManager.maze;


            do
            {
                x = URandom.Range(1, maze.GetLength(0) - 1);
                y = URandom.Range(1, maze.GetLength(1) - 1);
            } while (!maze[x, y].EsCamino || maze[x, y].trampa != null);
            return maze[x, y];
        }

    }
    void TrampaDamage(Ficha ficha)
    {
        int damage = URandom.Range(1, 4);

        if (!ficha.shield)
            ficha.vida -= damage;
        Debug.Log($"Ficha {ficha.team.teamName} sufrió {damage} puntos de daño");

    }
    void TrampaFreeze(Ficha ficha)
    {
        int time = URandom.Range(1, 4);

        if (!ficha.shield)
            ficha.freeze = time;
    }






}

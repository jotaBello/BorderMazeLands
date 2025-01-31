using UnityEngine;
using UnityEngine.Rendering.Universal;
using URandom = UnityEngine.Random;

public class Trampa
{
    public Casilla casillaAsociada;
    public Casilla casillaEnlazada;
    public string tipo;
    public bool Actived;

    MazeManager mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
    FichaManager fichaManager = GameObject.Find("FichaManager").GetComponent<FichaManager>();
    HudManager hudManager = GameObject.Find("Canvas").GetComponent<HudManager>();

    public Trampa(Casilla casilla, string tipo)
    {
        casillaAsociada = casilla;
        this.tipo = tipo;

        if (tipo == "Tele")
        {
            casillaEnlazada = BuscarCasillaAleatoriaTele();

        }
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
                if (!ficha.shield)
                    hudManager.PutMessage("Activaste una trampa de Daño");
                break;
            case "Freeze":
                TrampaFreeze(ficha);
                if (!ficha.shield)
                    hudManager.PutMessage("Activaste una trampa de Congelamiento");
                break;
            case "CoolDown":
                TrampaCoolDown(ficha);
                if (!ficha.shield)
                    hudManager.PutMessage("Activaste una trampa de Habilidad");
                break;
            case "Slowness":
                TrampaSlowness(ficha);
                if (!ficha.shield)
                    hudManager.PutMessage("Activaste una trampa de Lentitud");
                break;

            case "Light":
                TrampaLight(ficha);
                casillaAsociada.trampa = null;
                break;
        }



    }

    void TrampaTele(Ficha ficha)
    {
        fichaManager.MoverFicha(ficha, casillaEnlazada);

    }
    void TrampaDamage(Ficha ficha)
    {
        int damage = URandom.Range(1, 4);

        if (!ficha.shield)
            ficha.vida -= damage;

    }
    void TrampaFreeze(Ficha ficha)
    {
        int time = URandom.Range(3, 5);

        if (!ficha.shield)
            ficha.freeze = time;

    }
    void TrampaCoolDown(Ficha ficha)
    {
        if (!ficha.shield)
            ficha.cooldown = ficha.team.habilidadEnfriamiento;
    }
    void TrampaSlowness(Ficha ficha)
    {
        if (!ficha.shield)
        {
            ficha.Velocidad -= 2;
            ficha.slowness = 2;
        }
    }
    void TrampaLight(Ficha ficha)
    {
        ficha.lighttime = 2;
        ficha.fichaObj.GetComponent<Light2D>().pointLightOuterRadius *= 1.5f;
        ficha.fichaObj.GetComponent<Light2D>().pointLightInnerRadius *= 1.5f;
    }


    Casilla BuscarCasillaAleatoriaTele()
    {
        int x, y;
        Casilla[,] maze = mazeManager.maze;


        do
        {
            x = URandom.Range(1, maze.GetLength(0) - 1);
            y = URandom.Range(1, maze.GetLength(1) - 1);


        } while (!maze[x, y].EsCamino || maze[x, y].trampa != null || maze[x, y].ficha != null);
        return maze[x, y];
    }





}

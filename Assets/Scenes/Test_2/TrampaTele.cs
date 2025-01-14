/*using UnityEngine;

public class TrampaTeletransporte : Trampa
{
    private Casilla destino;

    public TrampaTeletransporte(Casilla casilla, Casilla casillaDestino) : base(casilla)
    {
        destino = casillaDestino;
    }

    public override void Activar(Ficha ficha)
    {
        FichaManager fichaManager = GameObject.Find("FichaManager").GetComponent<FichaManager>();

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
    }
}*/

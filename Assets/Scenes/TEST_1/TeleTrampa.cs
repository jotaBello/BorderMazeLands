using UnityEngine;

public class TrampaTeletransporte : Trampa
{
    private Casilla destino;

    public TrampaTeletransporte(Casilla casilla, Casilla casillaDestino) : base(casilla)
    {
        destino = casillaDestino;
    }

    public override void Activar(Ficha ficha)
    {
        if (!ficha.shield)
            if (destino != null && destino.EsCamino && destino.ficha == null)
            {
                // Mueve la ficha al destino
                ficha.fichaObj.transform.position = destino.casillaObject.transform.position;
                ficha.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno = destino;
                ficha.fichaObj.GetComponent<SeleccionarFicha>().casillaPosicion = destino;
                Debug.LogWarning($"Ficha {ficha.team.teamName} cahora su posicionInicialTurno es ({ficha.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno.fila}, {ficha.fichaObj.GetComponent<SeleccionarFicha>().PosicionInicialTurno.columna})");
                //ficha.Posicion.ficha = null;
                //ficha.Posicion = destino;
                //destino.ficha = ficha;

                Debug.Log($"Ficha {ficha.team.teamName} teletransportada a ({destino.fila}, {destino.columna})");
            }
            else
            {
                Debug.LogWarning("Destino de teletransporte inválido");
            }
    }
}

/*using UnityEngine;
using System;
using System.Collections.Generic;

public class Jugador
{
    public string Nombre { get; set; }
    public List<Ficha> Fichas { get; set; } // Cada jugador tiene una lista de fichas
    public int TurnosRestantesEnHabilidad { get; set; } // Para controlar el enfriamiento de habilidades

    public Jugador(string nombre)
    {
        Nombre = nombre;
        Fichas = new List<Ficha>();
        TurnosRestantesEnHabilidad = 0;
    }

    public void MoverFicha(Ficha ficha, Casilla destino)
    {
        // Aquí implementas la lógica de mover la ficha a la casilla destino
        if (destino.EsCamino && destino.Jugador == null) // Si la casilla es un camino y no está ocupada
        {
            ficha.Posicion = destino; // Mueve la ficha a la nueva casilla
            destino.Jugador = this;  // Marca la casilla como ocupada por el jugador
            ficha.Posicion.Jugador = this; // Actualiza la posición de la ficha del jugador
        }
    }

    public void UsarHabilidad(Ficha ficha)
    {
        if (TurnosRestantesEnHabilidad > 0)
        {
            Debug.Log($"{Nombre} no puede usar su habilidad, tiempo de enfriamiento.");
            return;
        }

        // Lógica para usar la habilidad
        Debug.Log($"{Nombre} usa la habilidad de {ficha.Nombre}.");
        TurnosRestantesEnHabilidad = ficha.HabilidadEnfriamiento;
    }
    
}
*/

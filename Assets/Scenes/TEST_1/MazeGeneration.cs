using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using URandom = UnityEngine.Random;

public class MazeGeneration : MonoBehaviour
{
    public static int filas = 31, columnas = 31; // Deben ser impares para permitir caminos
    public Casilla[,] laberinto = new Casilla[filas, columnas];
    System.Random rand = new System.Random();
    List<(int, int, int, int)> paredes = new List<(int, int, int, int)>();

    static string[] trampasArr = { "Daño", "Velocidad", "" };



    void Generar()
    {
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                // Asume que todo es pared por defecto
                laberinto[i, j] = new Casilla(false, i, j);
            }
        }


        // Selecciona una celda inicial en una posición impar
        int x = 15, y = 15;
        laberinto[x, y].EsCamino = true;  // Marca la celda como camino

        // Añade las paredes iniciales de esta celda
        AgregarParedes(x, y);


        // Procesa las paredes hasta que se acaben
        while (paredes.Count > 0)
        {
            // Elige una pared al azar y la elimina de la lista
            int indice = rand.Next(paredes.Count);
            var (px, py, cx, cy) = paredes[indice];
            paredes.RemoveAt(indice);

            // Si la celda conectada no ha sido visitada
            if (!laberinto[cx, cy].EsCamino)
            {
                laberinto[px, py].EsCamino = true; // Elimina la pared entre las celdas
                laberinto[cx, cy].EsCamino = true; // Marca la nueva celda como camino
                AgregarParedes(cx, cy); // Añade las paredes de la nueva celda
            }
        }


    }

    void AgregarParedes(int x, int y)
    {
        // Añade las paredes de las celdas adyacentes (solo celdas impares)
        if (x > 1) paredes.Add((x - 1, y, x - 2, y)); // Arriba
        if (x < filas - 2) paredes.Add((x + 1, y, x + 2, y)); // Abajo
        if (y > 1) paredes.Add((x, y - 1, x, y - 2)); // Izquierda
        if (y < columnas - 2) paredes.Add((x, y + 1, x, y + 2)); // Derecha
    }

    void ColocarTrampas()
    {
        int cantidadTrampas = 10; // Número de trampas que quieres colocar
        int intentos = 0;

        while (cantidadTrampas > 0 && intentos < 1000)
        {
            int x = rand.Next(1, filas - 1);
            int y = rand.Next(1, columnas - 1);

            if (laberinto[x, y].EsCamino && laberinto[x, y].trampa == null)
            {
                // Selecciona aleatoriamente el tipo de trampa
                int tipoTrampa = rand.Next(0, 3); // 0: Teletransporte, 1: Daño, 2: Pérdida de Turno

                switch (tipoTrampa)
                {
                    case 0: // Trampa de Teletransporte
                        Casilla destino = BuscarCasillaAleatoria(); // Implementa esta función para encontrar un destino válido
                        //laberinto[x, y].trampa = new TrampaTeletransporte(laberinto[x, y], destino);
                        break;
                    case 1: // Trampa de Daño
                        //laberinto[x, y].trampa = new TrampaDaño(laberinto[x, y], URandom.Range(1, 4));
                        break;
                    case 2: // Trampa de Freeze
                        //laberinto[x, y].trampa = new FreezeTrampa(laberinto[x, y], URandom.Range(1, 4));
                        break;
                }



                cantidadTrampas--;
            }

            intentos++;
        }
    }

    Casilla BuscarCasillaAleatoria()
    {
        int x, y;
        do
        {
            x = rand.Next(1, filas - 1);
            y = rand.Next(1, columnas - 1);
        } while (!laberinto[x, y].EsCamino || laberinto[x, y].trampa != null);
        return laberinto[x, y];
    }



    void Awake()
    {
        Debug.Log("hola");

        Generar();
        ColocarTrampas(); // Genera el laberinto

    }
}



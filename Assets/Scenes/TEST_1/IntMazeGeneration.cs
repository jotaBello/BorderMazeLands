using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;


public class IntMazeGeneration : MonoBehaviour
{
    public static int filas = 31, columnas = 31; // Deben ser impares para permitir caminos
    public int[,] laberinto = new int[filas, columnas];
    System.Random rand = new System.Random();
    List<(int, int, int, int)> paredes = new List<(int, int, int, int)>();

    IntMazeGeneration()
    {

    }

    void Generar()
    {
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                // Asume que todo es pared por defecto
                laberinto[i, j] = 0;
            }
        }


        // Selecciona una celda inicial en una posición impar
        int x = 15, y = 15;
        laberinto[x, y] = 1; // Marca la celda como camino

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
            if (laberinto[cx, cy] == 0)
            {
                laberinto[px, py] = 1; // Elimina la pared entre las celdas
                laberinto[cx, cy] = 1;
                laberinto[px, py] = 1; // Marca la nueva celda como camino
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

    void Awake()
    {
        Debug.Log("hola");
        Generar(); // Genera el laberinto

    }
}


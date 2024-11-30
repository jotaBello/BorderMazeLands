using UnityEngine;
using UnityEngine.UI;

public class CasillaScript : MonoBehaviour
{
    public MazeGeneration mazegen;
    SpriteRenderer objetoImagen;
    public Sprite imagenCaminoCentro;
    public Sprite imagenCaminoEsquina;
    public Sprite imagenCaminoLateral;
    public Sprite imagenCaminoSuperior;
    public Sprite imagenCaminoPedacito;
    public int filaI;
    public int columnaJ;
    public bool isWall;
    public bool isGoal;

    void Awake()
    {
        objetoImagen = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        filaI = filaI + 1;
        if (mazegen.laberinto[filaI, columnaJ] == 0)
        {
            isWall = true;
            objetoImagen.color = Color.gray;
        }
        if (mazegen.laberinto[filaI, columnaJ] == 1)
        {
            objetoImagen.color = Color.white;
            isWall = false;
            if (mazegen.laberinto[filaI, columnaJ - 1] == 0 && mazegen.laberinto[filaI - 1, columnaJ] == 1)
            {
                objetoImagen.sprite = imagenCaminoLateral;
            }
            if (mazegen.laberinto[filaI - 1, columnaJ] == 0 && mazegen.laberinto[filaI, columnaJ - 1] == 0)
            {
                objetoImagen.sprite = imagenCaminoEsquina;
            }
            if (mazegen.laberinto[filaI - 1, columnaJ] == 1 && mazegen.laberinto[filaI, columnaJ - 1] == 1)
            {
                objetoImagen.sprite = imagenCaminoCentro;
            }
            if (mazegen.laberinto[filaI - 1, columnaJ] == 0 && mazegen.laberinto[filaI, columnaJ - 1] == 1)
            {
                objetoImagen.sprite = imagenCaminoSuperior;
            }
            if (mazegen.laberinto[filaI - 1, columnaJ - 1] == 0 && mazegen.laberinto[filaI - 1, columnaJ] == 1 && mazegen.laberinto[filaI, columnaJ - 1] == 1)
            {
                objetoImagen.sprite = imagenCaminoPedacito;
            }
        }

        if (filaI == 16 && columnaJ == 16) isGoal = true;
        if (isGoal) objetoImagen.color = Color.red;
    }

}

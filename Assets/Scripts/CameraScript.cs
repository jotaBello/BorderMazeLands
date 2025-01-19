using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public MazeGeneration mazeGen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.main.orthographicSize = Mathf.Max(mazeGen.laberinto.GetLength(0), mazeGen.laberinto.GetLength(1)) / 2f;

   }

    // Update is called once per frame
    void Update()
    {

    }
}

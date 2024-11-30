using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject currentPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, -10);
    }
}

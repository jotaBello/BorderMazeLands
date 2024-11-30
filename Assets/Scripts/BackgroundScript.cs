using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public GameObject camera;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -1);
    }
}

using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public GameObject camara;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camara.transform.position.x, camara.transform.position.y, -1);
    }
}

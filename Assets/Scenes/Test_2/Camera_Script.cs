using Unity.VisualScripting;
using UnityEngine;

public class Camera_Script : MonoBehaviour
{

    public GameObject target;
    public FichaManager fichaManager;





    void Update()
    {

        gameObject.GetComponent<Camera>().orthographicSize = 7;
        if (target != null)
            transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.transform.position.x, target.transform.position.y, -10), Time.deltaTime);
    }
}

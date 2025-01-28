using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject target;

    void Start()
    {

    }

    void Update()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.transform.position.x, target.transform.position.y+1, -1), Time.deltaTime);
    }
}

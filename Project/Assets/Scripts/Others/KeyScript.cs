using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject target;
    public Tile currentTile;

    void Update()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.transform.position.x, target.transform.position.y + 1, -1), Time.deltaTime);
    }
    public void Fall_on_the_floor(Tile piso)
    {
        target = null;
        transform.position = piso.tileObject.transform.position;

        piso.key = gameObject;
        currentTile = piso;

    }
}

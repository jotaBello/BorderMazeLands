using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject target;
    public Casilla casillaActual;

    void Update()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.transform.position.x, target.transform.position.y + 1, -1), Time.deltaTime);
    }
    public void CaerEnELPiso(Casilla piso)
    {
        target = null;
        transform.position = piso.casillaObject.transform.position;
       // casillaActual.key = null;
        piso.key = gameObject;
        casillaActual=piso;

    }
}

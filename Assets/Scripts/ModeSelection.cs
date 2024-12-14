using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelection : MonoBehaviour
{
    public SelectionMenu canvas;
    public GameObject M;
    public GameObject P;
    public void TwoPlayerSelection()
    {
        canvas.usersLimit = 2;
        M.SetActive(false);
        P.SetActive(true);
    }
}

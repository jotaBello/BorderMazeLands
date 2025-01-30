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
    public void ThreePlayerSelection()
    {
        canvas.usersLimit = 3;
        M.SetActive(false);
        P.SetActive(true);
    }
    public void FourPlayerSelection()
    {
        canvas.usersLimit = 4;
        M.SetActive(false);
        P.SetActive(true);
    }
    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

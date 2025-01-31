using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    void Start()
    {

    }
    public void Play()
    {
        SceneManager.LoadScene("PlayerSelection");
    }
    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}

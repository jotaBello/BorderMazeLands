using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelection : MonoBehaviour
{
    public void toPlayerSelection()
    {
        Debug.Log("2 jugadores");
        SceneManager.LoadScene("PlayerSelection");
    }
}

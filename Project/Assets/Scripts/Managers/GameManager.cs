using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Teams> teams;
    public List<Teams> users;

    public Piece winner;

    private void Awake()
    {
        users = new List<Teams>();
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "PlayerSelection")
            {
                GameManager.Instance.users = new List<Teams>();
            }
            Destroy(gameObject);
        }
    }
}

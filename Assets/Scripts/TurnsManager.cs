using UnityEngine;

public class TurnsManager : MonoBehaviour
{
    GameObject gameMan;
    GameManager gameManager;
    public int usersLimit;
    void Start()
    {
        gameMan = GameObject.Find("GameManager");
        gameManager = gameMan.GetComponent<GameManager>();
        usersLimit = gameManager.users.Count;// get the limit from the game manager
    }
    void Update()
    {

    }
}

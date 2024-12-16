using UnityEngine;

public class TurnManager : MonoBehaviour
{
    GameObject gamManObject;
    GameManager gameManager;

    public int usersLimit;

    void Start()
    {
        gamManObject = GameObject.Find("GameManager");
        gameManager = gamManObject.GetComponent<GameManager>();
        usersLimit = gameManager.users.Count;
    }
    void Update()
    {

    }
}

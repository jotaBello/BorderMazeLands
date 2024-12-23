/*using UnityEngine;

public class TurnManager : MonoBehaviour
{
    GameObject gamManObject;
    GameManager gameManager;


    public PlayerScript[,] PlayerMatrix;
    Teams[] users;
    public int usersLimit;
    int playersPerTeam = 3;

    bool Win = false;


    void Start()
    {
        gamManObject = GameObject.Find("GameManager");
        gameManager = gamManObject.GetComponent<GameManager>();
        usersLimit = gameManager.users.Count;
        //usersLimit = 2;
        users = new Teams[usersLimit];
        for (int i = 0; i < usersLimit; i++)
        {
            users[i] = gameManager.users[i];
        }

        PlayerScript[,] PlayerMatrix = new PlayerScript[playersPerTeam, usersLimit];
        Turn(0);
    }
    void Update()
    {

    }

    void Turn(int currentTeam)
    {
        for (int i = 0; i < playersPerTeam; i++)
        {
            Play(PlayerMatrix[i, currentTeam]);
        }
    }

    void Play(PlayerScript currentPlayer)
    {
        return;
    }


}
*/

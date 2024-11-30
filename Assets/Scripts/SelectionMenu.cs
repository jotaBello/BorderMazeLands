using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    public GameObject playerSelection;
    public GameObject modeSelection;
    private int index;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI teamname;
    [SerializeField] private TextMeshProUGUI currentUser;
    [SerializeField] private TextMeshProUGUI teamDescription;
    private GameManager gameManager;
    private int usersLimit;

    private void Start()
    {
        gameManager = GameManager.Instance;
        index = PlayerPrefs.GetInt("PlayerIndex");

        if (index > gameManager.teams.Count - 1)
        {
            index = 0;
        }

        UpdateScreen();
    }

    private void UpdateScreen()
    {
        PlayerPrefs.SetInt("PlayerIndex", index);
        image.sprite = gameManager.teams[index].teamImage;
        teamname.text = $"EQUIPO {index + 1} : {gameManager.teams[index].name}";
        currentUser.text = $"JUGADOR {gameManager.users.Count + 1}";
        teamDescription.text = gameManager.teams[index].teamDescription;
    }

    public void NextTeam()
    {
        if (index == gameManager.teams.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        UpdateScreen();
    }

    public void PreviewTeam()
    {
        if (index == 0)
        {
            index = gameManager.teams.Count - 1;
        }
        else
        {
            index--;
        }
        UpdateScreen();
    }

    public void Select()
    {
        if (gameManager.users.Count == usersLimit)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            gameManager.users.Add(gameManager.teams[index]);
            UpdateScreen();
        }
    }

    public void Select2Player()
    {
        Debug.Log("locaaa");
        usersLimit = 2;
        modeSelection.SetActive(false);
        playerSelection.SetActive(true);
    }
    public void Select4Player()
    {
        usersLimit = 4;
        modeSelection.SetActive(false);
        playerSelection.SetActive(true);
    }
    public void Select8Player()
    {
        usersLimit = 8;
        modeSelection.SetActive(false);
        playerSelection.SetActive(true);
    }

}

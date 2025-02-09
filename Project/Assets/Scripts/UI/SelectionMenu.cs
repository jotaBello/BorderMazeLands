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
    [SerializeField] private TextMeshProUGUI currentPlayer;
    [SerializeField] private TextMeshProUGUI currentUser;


    [SerializeField] private TextMeshProUGUI LifeText;
    [SerializeField] private TextMeshProUGUI SpeedText;
    [SerializeField] private TextMeshProUGUI CoolDownText;
    [SerializeField] private TextMeshProUGUI AbilityText;



    public GameManager gameManager;

    public GameObject selectButton;
    public int usersLimit;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        index = PlayerPrefs.GetInt("PlayerIndex");

        if (index > gameManager.teams.Count - 1)
        {
            index = 0;
        }

        UpdateScreen();
    }

    //Actualizar le personaje y el texto en el menu de seleccion
    private void UpdateScreen()
    {
        if (index > gameManager.teams.Count - 1) index = 0;
        PlayerPrefs.SetInt("PlayerIndex", index);
        image.sprite = gameManager.teams[index].teamImage;
        currentPlayer.text = $"{gameManager.teams[index].name}";
        currentUser.text = $"JUGADOR {gameManager.users.Count + 1}";


        LifeText.text = gameManager.teams[index].life.ToString();
        SpeedText.text = gameManager.teams[index].life.ToString();
        CoolDownText.text = gameManager.teams[index].cooldown.ToString();
        AbilityText.text = gameManager.teams[index].teamDescription;

        CheckSelectButton();
    }

    //Revisa si el equipo ya fue seleccionado y ocultar el boton de seleccionar
    void CheckSelectButton()
    {
        bool wasSelected = false;

        foreach (Teams team in gameManager.users)
        {
            if (team.teamName == gameManager.teams[index].teamName)
            {
                wasSelected = true;
            }
        }
        if (!wasSelected) selectButton.SetActive(true);
        else selectButton.SetActive(false);
    }

    //Boton de proximo de equipo
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

    //Boton de anterior personaje
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

    //Boton de seleccionar equipo
    public void Select()
    {
        if (gameManager.users.Count == usersLimit - 1)
        {
            gameManager.users.Add(gameManager.teams[index]);
            SceneManager.LoadScene("Game");
        }
        else
        {
            gameManager.users.Add(gameManager.teams[index]);

            UpdateScreen();
        }
    }

    //Metodos para seleccionar varios jugadores

    public void Select2Player()
    {
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
    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

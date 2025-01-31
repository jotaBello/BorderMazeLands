using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private bool GamePaused;


    [SerializeField] private GameManager gameManager;
    [SerializeField] private Turn_Manager turnManager;
    [SerializeField] private FichaManager fichaManager;

    [SerializeField] private TextMeshProUGUI FichaName;
    [SerializeField] private TextMeshProUGUI Health;
    [SerializeField] private TextMeshProUGUI Velocity;
    [SerializeField] private TextMeshProUGUI CoolDown;

    [SerializeField] private GameObject TabButton;
    [SerializeField] private GameObject HideStatsButton;
    [SerializeField] private GameObject FichaStats;
    [SerializeField] private bool StatsHided;

    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private GameObject TutorialButton;
    [SerializeField] private GameObject HideTutorialButton;

    [SerializeField] private bool TutorialShown;

    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI winText;

    [SerializeField] private TextMeshProUGUI ConsoleMessage;
    [SerializeField] private GameObject Console;









    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        turnManager = GameObject.Find("TurnManager").GetComponent<Turn_Manager>();
        fichaManager = GameObject.Find("FichaManager").GetComponent<FichaManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (StatsHided)
            {
                ShowStats();
            }
            else
            {
                HideStats();
            }

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (TutorialShown)
            {
                HideTutorial();
            }
            else
            {
                ShowTutorial();
            }
        }


        Ficha ficha = null;
        foreach (var fich in fichaManager.fichaList)
        {
            if (fich.team == turnManager.equipos[turnManager.turnoActual])
            {
                ficha = fich;
            }
        }
        if (ficha != null)
        {
            FichaName.text = ficha.team.name;
            Health.text = $"Vida: {ficha.vida.ToString()}";
            Velocity.text = $"Velocidad: {ficha.Velocidad.ToString()}";
            CoolDown.text = $"Enfriamiento: {ficha.cooldown.ToString()}";
        }
    }
    public void Pause()
    {
        HideStats();
        Time.timeScale = 0f;
        PauseButton.SetActive(false);
        PauseMenu.SetActive(true);
        GamePaused = true;
        TutorialPanel.SetActive(false);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
        GamePaused = false;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowStats()
    {
        if (!GamePaused)
        {
            TabButton.SetActive(false);
            HideStatsButton.SetActive(true);
            FichaStats.SetActive(true);
            StatsHided = false;
        }
    }
    public void HideStats()
    {
        TabButton.SetActive(true);
        HideStatsButton.SetActive(false);
        FichaStats.SetActive(false);
        StatsHided = true;
    }

    public void ShowTutorial()
    {
        if (!GamePaused)
        {
            TutorialPanel.SetActive(true);
            TutorialButton.SetActive(false);
            HideTutorialButton.SetActive(true);
            TutorialShown = true;
        }
    }
    public void HideTutorial()
    {
        if (!GamePaused)
        {
            TutorialPanel.SetActive(false);
            TutorialButton.SetActive(true);
            HideTutorialButton.SetActive(false);
            TutorialShown = false;
        }
    }
    public void Win()
    {
        WinPanel.SetActive(true);
        winText.text = $"{gameManager.winner.team.name} GANO";
    }

    public void PutMessage(string message)
    {
        ConsoleMessage.text = message;
    }
}

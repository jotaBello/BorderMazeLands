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
    [SerializeField] private bool TutorialShown;

    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI winText;










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
            Show_Hide_Tutorial();
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
            Health.text = $"Health: {ficha.vida.ToString()}";
            Velocity.text = $"Velociity: {ficha.Velocidad.ToString()}";
            CoolDown.text = $"CoolDown: {ficha.cooldown.ToString()}";
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

    public void Show_Hide_Tutorial()
    {
        if (!GamePaused)
        {
            if (TutorialShown)
            {
                TutorialPanel.SetActive(false);
                TutorialShown = false;
            }
            else
            {
                TutorialPanel.SetActive(true);
                TutorialShown = true;
            }
        }
    }
    public void Win()
    {
        WinPanel.SetActive(true);
        winText.text = $"{gameManager.winner.team.name} WINS";
    }
}

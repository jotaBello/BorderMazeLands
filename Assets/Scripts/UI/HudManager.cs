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
    [SerializeField] private PieceManager pieceManager;

    [SerializeField] private TextMeshProUGUI pieceName;
    [SerializeField] private TextMeshProUGUI Life;
    [SerializeField] private TextMeshProUGUI Speed;
    [SerializeField] private TextMeshProUGUI CoolDown;

    [SerializeField] private GameObject TabButton;
    [SerializeField] private GameObject HideStatsButton;
    [SerializeField] private GameObject pieceStats;
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
        pieceManager = GameObject.Find("PieceManager").GetComponent<PieceManager>();
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


        Piece piece = null;
        foreach (var p in pieceManager.pieceList)
        {
            if (p.team == turnManager.teams[turnManager.currentTurn])
            {
                piece = p;
            }
        }
        if (piece != null)
        {
            pieceName.text = piece.team.name;
            Life.text = $"Vida: {piece.life.ToString()}";
            Speed.text = $"Velocidad: {piece.Speed.ToString()}";
            CoolDown.text = $"Enfriamiento: {piece.cooldown.ToString()}";
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
            pieceStats.SetActive(true);
            StatsHided = false;
        }
    }
    public void HideStats()
    {
        if (!GamePaused)
        {
            TabButton.SetActive(true);
            HideStatsButton.SetActive(false);
            pieceStats.SetActive(false);
            StatsHided = true;
        }
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

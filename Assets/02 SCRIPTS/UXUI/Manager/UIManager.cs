using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        HideAllPopups();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(IState newState)
    {
        HideAllPopups();

        if (newState is GamePausedState)
        {
            if (pausePanel != null) pausePanel.SetActive(true);
        }
        else if (newState is GameVictoryState)
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }
        else if (newState is GameGameOverState)
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }
    }

    private void HideAllPopups()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }


    // CLICK EVENT ON UI
    public void OnClickResume()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResumeGame();
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
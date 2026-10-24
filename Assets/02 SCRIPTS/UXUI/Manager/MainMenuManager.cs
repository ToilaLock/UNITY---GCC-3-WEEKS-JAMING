using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameplaySceneName = "GameStart";

    [Header("UI")]
    [SerializeField] private GameObject firstSelectedButton;

    private void Start()
    {
        Time.timeScale = 1f;

        if (firstSelectedButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit Game!");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
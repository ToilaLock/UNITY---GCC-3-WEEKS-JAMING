using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicController : MonoBehaviour
{
    [SerializeField] private SoundData sceneBGM;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ApplyMusic();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyMusic();
    }

    private void ApplyMusic()
    {
        if (AudioManager.Instance == null) return;

        if (sceneBGM != null)
        {
            AudioManager.Instance.PlayMusic(sceneBGM);
        }
        else
        {
            AudioManager.Instance.StopMusic();
        }
    }
}
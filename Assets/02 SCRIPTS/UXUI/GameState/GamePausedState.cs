using UnityEngine;

public class GamePausedState : IState
{
    private GameManager gameManager;

    public GamePausedState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Time.timeScale = 0f; 
        Debug.Log("Enter Pause State");

        gameManager.EnableUIInput();
    }

    public void Exit()
    {
        Time.timeScale = 1f; 
    }

    public void Update() { }
    public void FixedUpdate() { }
}
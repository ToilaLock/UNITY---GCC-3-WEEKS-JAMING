using UnityEngine;

public class GamePlayingState : IState
{
    private GameManager gameManager;

    public GamePlayingState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Time.timeScale = 1f; 
        Debug.Log("Enter Play State");

        gameManager.EnableGameplayInput();
    }

    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
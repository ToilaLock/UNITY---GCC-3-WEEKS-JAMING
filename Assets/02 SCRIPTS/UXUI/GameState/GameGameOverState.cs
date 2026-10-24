using UnityEngine;

public class GameGameOverState : IState
{
    private GameManager gameManager;

    public GameGameOverState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Enter Game Over State");
    }

    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
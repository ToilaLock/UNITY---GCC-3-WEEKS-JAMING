using UnityEngine;

public class GameVictoryState : IState
{
    private GameManager gameManager;

    public GameVictoryState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Enter Victory State");
    }

    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
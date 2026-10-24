using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private PlayerInputHandler inputHandler;

    private StateMachine stateMachine;

    //GAME STATES
    public GamePlayingState PlayingState { get; private set; }
    public GamePausedState PausedState { get; private set; }
    public GameVictoryState VictoryState { get; private set; }
    public GameGameOverState GameOverState { get; private set; }

    public event Action<IState> OnGameStateChanged;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        stateMachine = new StateMachine();
        PlayingState = new GamePlayingState(this);
        PausedState = new GamePausedState(this);
        VictoryState = new GameVictoryState(this);
        GameOverState = new GameGameOverState(this);
    }

    private void Start()
    {
        if (playerHealth == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerHealth = playerObj.GetComponent<PlayerHealth>();
        }

        if (bossHealth == null)
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
            if (bossObj != null) bossHealth = bossObj.GetComponent<BossHealth>();
        }

        ChangeState(PlayingState);
    }

    private void OnEnable()
    {
        playerHealth.OnDeath += HandlePlayerDeath;
        bossHealth.OnDeath += HandleBossDeath;

        inputHandler.PausePressed += TogglePause;
    }

    private void OnDisable()
    {
        playerHealth.OnDeath -= HandlePlayerDeath;
        bossHealth.OnDeath -= HandleBossDeath;

        inputHandler.PausePressed -= TogglePause;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
        OnGameStateChanged?.Invoke(newState);
    }

    // CHANGE STATE
    public void PauseGame()
    {
        if (stateMachine.CurrentState == PlayingState)
            ChangeState(PausedState);
    }

    public void ResumeGame()
    {
        if (stateMachine.CurrentState == PausedState)
            ChangeState(PlayingState);
    }

    public void TogglePause()
    {
        if (stateMachine.CurrentState == PlayingState)
            PauseGame();
        else if (stateMachine.CurrentState == PausedState)
            ResumeGame();
    }

        //GAME OVER
    private void HandlePlayerDeath()
    {
        if (stateMachine.CurrentState != VictoryState)
        {
            ChangeState(GameOverState);
        }
    }

        //VICTORY
    private void HandleBossDeath()
    {
        if (stateMachine.CurrentState != GameOverState)
        {
            ChangeState(VictoryState);
        }
    }

    // GAME PLAY / MENU
    public void EnableGameplayInput()
    {
        if (inputHandler != null)
            inputHandler.EnableGameplayInput();
    }

    public void EnableUIInput()
    {
        if (inputHandler != null)
            inputHandler.EnableUIInput();
    }
}
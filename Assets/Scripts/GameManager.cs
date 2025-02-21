using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public UnityEvent OnGameOver;
    [HideInInspector] public UnityEvent OnGameStarts;
    [HideInInspector] public UnityEvent OnPauseToggled;
    [field:SerializeField] public bool IsGamePaused { get; private set; }
    [field:SerializeField] public bool IsGameRunning { get; private set; }
    [field:SerializeField] public float ScoreMultiplier { get; private set; }
    [field:SerializeField] public float Score { get; private set; }
    [field:SerializeField] public PlayerController PlayerController { get; private set; }
    [field:SerializeField] public CameraController CameraController { get; private set; }
    [field:SerializeField] public PlanetController PlanetController { get; private set; }   

    private void Awake()
    {
        if(Instance != null )
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        IsGameRunning = false;
    }

    private void Start()
    {
        OnGameStarts.AddListener(() => InitializeGame());
        OnGameOver.AddListener(() => IsGameRunning = false);
    }

    private void Update()
    {
        if(IsGameRunning && !IsGamePaused)
            CalculateScore();
    }

    private void InitializeGame()
    {
        IsGameRunning = true;
        Score = 0;
    }

    public void GameOver()
    {
        print("GameOver");
        OnGameOver?.Invoke();
    }

    public void StartGame()
    {
        print("GameStart");
        OnGameStarts?.Invoke();
    }
    
    public void QuitGame()
    {
        print("GameQuit");
        Application.Quit();
    }

    public void ReturnToStartMenu()
    {
        IsGamePaused = false;
        ReloadScene();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TogglePause()
    {
        IsGamePaused = !IsGamePaused;
        OnPauseToggled?.Invoke();
        Time.timeScale = IsGamePaused ? 0 : 1;
    }

    private void CalculateScore()
    {
        Score += ScoreMultiplier * Time.deltaTime;
    }
}

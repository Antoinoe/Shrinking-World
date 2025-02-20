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
    public bool IsGamePaused { get; private set; }
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

        DontDestroyOnLoad(this);
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
}

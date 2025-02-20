using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public UnityEvent OnGameOver;
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

        OnGameOver.AddListener(() => GameOver());
    }

    public void Play()
    {
        print("play");
    }

    internal void GameOver()
    {
        print("GameOver");
        PlanetController.StopShrink();
    }
}

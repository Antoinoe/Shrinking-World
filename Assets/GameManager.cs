using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float startingShrinkingSpeed;
    [SerializeField] private float currentShrinkingSpeed;
    [SerializeField] private float shrinkingAcceleration;

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

    public void Play()
    {
        print("play");
    }
}

using UnityEngine;
using DG.Tweening;
using System;

public class PlanetController : MonoBehaviour
{
    public float Size => transform.localScale.x;

    [SerializeField] private bool shrinkAtStart;
    [field: SerializeField] public float StartSize { get; private set; }
    [field: SerializeField] public float EndSize { get; private set; }

    [SerializeField] private float startingShrinkingSpeed;
    [SerializeField] private float currentShrinkingSpeed;
    [SerializeField] private float shrinkingAcceleration;

    private bool canShrink = false;

    private void Awake()
    {
        canShrink = false;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarts.AddListener(() => OnGameStarts());
        GameManager.Instance.OnGameOver.AddListener(() => OnGameOver());
        StopShrink();
        ResetSize();
        if(shrinkAtStart)
            StartShrink();        
    }

    private void OnGameStarts()
    {
        StartShrink();
    }

    private void OnGameOver()
    {
        StopShrink();
    }

    public void StartShrink()
    {
        canShrink = true;
    }

    public void StopShrink()
    {
        canShrink = false;
    }

    public void ResetSize()
    {
        transform.localScale = new Vector3(StartSize, StartSize, StartSize);
    }

    private void Update()
    {
        if (!canShrink)
            return;

        Shrink();
        UpdateShrinkingSpeed();
        CheckSize();
    }

    private void CheckSize()
    {
        if(transform.localScale.x < EndSize)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void UpdateShrinkingSpeed()
    {
        currentShrinkingSpeed += shrinkingAcceleration * Time.deltaTime;
    }

    private void Shrink()
    {
        var currentScale = transform.localScale;
        currentScale = new Vector3(currentScale.x - currentShrinkingSpeed, currentScale.y - currentShrinkingSpeed, currentScale.z - currentShrinkingSpeed);
        transform.localScale = currentScale;
    }

    internal void SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, size);
    }
}

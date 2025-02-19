using UnityEngine;
using DG.Tweening;
using System;

public class PlanetController : MonoBehaviour
{
    [SerializeField] private float startSize;
    [SerializeField] private float endSize;
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
        StopShrink();
        ResetSize();
        StartShrink();        
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
        transform.localScale = new Vector3(startSize, startSize, startSize);
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
        if(transform.localScale.x < endSize)
        { 
            GameManager.Instance.OnGameOver?.Invoke();
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
}

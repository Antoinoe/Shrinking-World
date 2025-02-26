using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;
using System.Collections;

public class PlanetController : MonoBehaviour
{
    
    [HideInInspector] public UnityEvent OnPlanetExplosionEnd;
    [HideInInspector] public UnityEvent OnPlanetExplosionStart;

    public float Size => transform.localScale.x;
    [field: SerializeField] public float StartSize { get; private set; }
    [field: SerializeField] public float EndSize { get; private set; }

    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private bool shrinkAtStart;
    [SerializeField] private float startingShrinkingSpeed;
    [SerializeField] private float currentShrinkingSpeed;
    [SerializeField] private float shrinkingAcceleration;
    [SerializeField] private float rotationAnimationSpeed;
    [SerializeField] private float planetGameOverAnimationDuration;

    private bool canShrink = false;
    private bool doRotateAnimation = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        canShrink = false;
        doRotateAnimation = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        FinalShrinkAnimation();
    }

    private void FinalShrinkAnimation()
    {
        Sequence seq = DOTween.Sequence();
        doRotateAnimation = true;
        seq.Append(transform.DOScale(0, planetGameOverAnimationDuration)).OnComplete(()=>
        {
            StartCoroutine(DestroyPlanetCoroutine());
            spriteRenderer.enabled = false;
        });
    }

    private IEnumerator DestroyPlanetCoroutine()
    {
        OnPlanetExplosionStart?.Invoke();
        explosionParticle.Play();
        yield return new WaitForSeconds(explosionParticle.main.duration / 2f);
        OnPlanetExplosionEnd?.Invoke();
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
        if (doRotateAnimation)
            transform.Rotate(new Vector3(0,0, rotationAnimationSpeed * Time.deltaTime));

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

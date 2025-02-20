using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PowerUpGenerator : MonoBehaviour
{
    [SerializeField] private List<PowerUp> powerUpsPrefab;
    [SerializeField] private float delayBewteenPowerUpsSpawn;
    [SerializeField] private float spawnHeightOffset;
    private bool canSpawn;
    private bool canSpawnNextPowerUp;

    private void Start()
    {
        GameManager.Instance.OnGameStarts.AddListener(() => Initialize());
        GameManager.Instance.OnGameOver.AddListener(() => DeInitialize());
    }

    private void Initialize()
    {
        canSpawn = true;
        canSpawnNextPowerUp = true;
    }

    private void DeInitialize()
    {
        canSpawn = false;
    }

    private void Update()
    {
        if(canSpawn && canSpawnNextPowerUp)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }

    private IEnumerator SpawnCoroutine() 
    {
        canSpawnNextPowerUp = false;
        yield return new WaitForSeconds(delayBewteenPowerUpsSpawn);
        Spawn();
        canSpawnNextPowerUp = true;
    }

    private void Spawn()
    {
        var randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        var spawnPos = randomDir * GameManager.Instance.PlanetController.Size/1.98f /*+ new Vector2(spawnHeightOffset, spawnHeightOffset)*/;
        var randomPowerUpIndex = Random.Range(0, powerUpsPrefab.Count);
        var newPowerUp = Instantiate(powerUpsPrefab[randomPowerUpIndex]);
        newPowerUp.transform.position = spawnPos;
    }
}

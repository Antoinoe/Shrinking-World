using UnityEngine;
using System.Collections.Generic;

public class WallController : MonoBehaviour
{
    [SerializeField] private bool canGenerateWalls;
    [SerializeField] private List<GameObject> wallSections;
    [SerializeField] private float wallSpacementError;
    [SerializeField] private float startingAngle;
    [SerializeField] private float minHeight;

    private void Start()
    {
        GameManager.Instance.OnGameStarts.AddListener(() => GenerateWalls());
    }

    private void GenerateWalls()
    {
        if (wallSections.Count == 0 || !canGenerateWalls)
            return;

        var randomizedWallSection = wallSections;
        randomizedWallSection.Shuffle();

        var wallSpacement = 360 / wallSections.Count;
        var angle = startingAngle;

        foreach (var w in wallSections)
        {
            var wallSection = Instantiate(w, GameManager.Instance.PlanetController.transform.position, Quaternion.identity);
            wallSection.transform.eulerAngles = new Vector3(0, 0, angle);
            angle += Random.Range(wallSpacement - wallSpacementError, wallSpacement + wallSpacementError);
        }
    }
}

using UnityEngine;

public class SizeUp : PowerUp
{
    [SerializeField] private float size;
    protected override void ApplyEffect()
    {
        Debug.Log("Size Up!!!");
        GameManager.Instance.PlanetController.SetSize(GameManager.Instance.PlanetController.Size + size);
    }
}

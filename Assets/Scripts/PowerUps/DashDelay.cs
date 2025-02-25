using UnityEngine;

public class DashDelay : PowerUp
{
    [SerializeField] private float dashDelay;
    protected override void ApplyEffect()
    {
        GameManager.Instance.PlayerController.SetDashDelay(dashDelay);
    }
}

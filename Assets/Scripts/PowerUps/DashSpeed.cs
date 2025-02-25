using UnityEngine;

public class DashSpeed : PowerUp
{
    [SerializeField] private float dashSpeed;
    protected override void ApplyEffect()
    {
        GameManager.Instance.PlayerController.SetDashSpeed(dashSpeed);
    }
}

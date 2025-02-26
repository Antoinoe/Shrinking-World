using UnityEngine;

public class DashSpeed : PowerUp
{
    [SerializeField] private float dashSpeed;
    protected override void ApplyEffect()
    {
        var player = GameManager.Instance.PlayerController;
        player.SetDashSpeed(player.DashSpeed - dashSpeed);
    }
}

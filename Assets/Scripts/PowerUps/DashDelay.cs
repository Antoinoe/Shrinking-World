using UnityEngine;

public class DashDelay : PowerUp
{
    [SerializeField] private float dashDelay;
    protected override void ApplyEffect()
    {
        var player = GameManager.Instance.PlayerController;
        player.SetDashDelay(player.DashCooldown - dashDelay);
    }
}

using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var powerups = collision.GetComponents<PowerUp>();

        if (powerups?.Length<=0)
            return;

        foreach (var p in powerups)
        {
            p.OnPowerUpCollected?.Invoke();
        }
    }
}

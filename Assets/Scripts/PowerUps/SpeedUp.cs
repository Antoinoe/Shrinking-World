using UnityEngine;

public class SpeedUp : PowerUp
{
    [SerializeField] private float speed;
    
    protected override void ApplyEffect()
    {
        base.ApplyEffect();
        Debug.Log("Speed Up!!!");
        GameManager.Instance.PlayerController.SetSpeed(GameManager.Instance.PlayerController.WalkSpeed + speed);
    }
}
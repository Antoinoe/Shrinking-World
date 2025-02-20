using System.Drawing;
using UnityEngine;

public class JumpUp : PowerUp
{
    [SerializeField] private float jumpForce;
    protected override void ApplyEffect()
    {
        Debug.Log("Jump Up!!!");
        GameManager.Instance.PlayerController.SetJumpForce(GameManager.Instance.PlayerController.JumpForce + jumpForce);
    }
}

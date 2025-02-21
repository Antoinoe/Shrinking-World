using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        print("collided");
        if (!collider.CompareTag("Ground"))
            return;
        GameManager.Instance.PlayerController.IsGrounded = true;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!collider.CompareTag("Ground"))
            return;
        GameManager.Instance.PlayerController.IsGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Ground"))
            return;
        GameManager.Instance.PlayerController.IsGrounded = false;
    }
}

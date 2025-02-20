using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public Transform PlayerVisual { get; private set; }
    [field: SerializeField] public float MoveSpeed {get; private set;}
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float GravitySpeed { get; private set; }
    [field: SerializeField] public float GravityMultiplier { get; private set; }

    private Rigidbody2D playerRB2D;

    public void SetSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    private void Awake()
    {
        playerRB2D = PlayerVisual.GetComponent<Rigidbody2D>();
        if(playerRB2D == null)
        {
            Debug.LogError("could not retrieve rigidbody2D form player visual");
            return;
        }
    }

    private void Update()
    {
        GetInputs();
        ApplyGravity();
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        float angle = Vector2.SignedAngle(Vector2.down, GetOrientation());
        PlayerVisual.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private Vector2 GetOrientation()
        => ((Vector2)transform.position - (Vector2)PlayerVisual.transform.position).normalized;

    private void ApplyGravity()
    {
        playerRB2D.AddForce(GravityMultiplier * GravitySpeed * Time.fixedDeltaTime * GetOrientation());
    }

    private void GetInputs()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            print("left");
            transform.Rotate(new Vector3(0f, 0f, MoveSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            print("right");
            transform.Rotate(new Vector3(0f, 0f, -MoveSpeed * Time.deltaTime));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            print("jump");
            playerRB2D.AddForce(-GetOrientation() * JumpForce, ForceMode2D.Impulse);
        }
    }

    internal void SetJumpForce(float jumpForce)
    {
        JumpForce = jumpForce;
    }
}

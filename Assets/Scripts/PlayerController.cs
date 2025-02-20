using System;
using System.Collections;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [field: SerializeField] public Transform PlayerVisual { get; private set; }
    [field: SerializeField] public float MoveSpeed {get; private set;}
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float GravitySpeed { get; private set; }
    [field: SerializeField] public float GravityMultiplier { get; private set; }
    [field: SerializeField] public float DashSpeed { get; private set; }
    [field: SerializeField] public float DashMultiplier { get; private set; }
    [field: SerializeField] public float DashDuration { get; private set; }
    [field: SerializeField] public float DashCooldown { get; private set; }
    public Direction Direction { get; private set; }

    private Rigidbody2D playerRB2D;
    private float dashCurrentCooldown;
    private bool recordInput;
    [SerializeField] private bool canDash; //remove sf
    private bool canMove;
    private bool isDashing;

    public void InitPlayer()
    {
        canMove = true;
        recordInput = true;
    }

    public void KillPlayer()
    {
        canMove = false;
        canDash = false;
        isDashing = false;
        recordInput = false;
    }

    public void SetSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    public void SetJumpForce(float jumpForce)
    {
        JumpForce = jumpForce;
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

    private void Start()
    {
        GameManager.Instance.OnGameOver.AddListener(()=>KillPlayer());
        InitPlayer();
    }

    private void Update()
    {
        ApplyDashCoolDown();
        ApplyInputs();
        ApplyGravity();
        ApplyDashVelocity();
        UpdateVisualRotation();
    }

    private void ApplyDashCoolDown()
    {
        dashCurrentCooldown -= Time.deltaTime;
        Math.Clamp(dashCurrentCooldown, 0, DashCooldown);
        canDash = dashCurrentCooldown <= 0;
    }

    private void ApplyDashVelocity()
    {
        if (!isDashing)
            return;

        var dashSpeed = DashSpeed * DashMultiplier * Time.deltaTime;
        dashSpeed = Direction == Direction.LEFT? dashSpeed : -dashSpeed;
        RotatePlayer(dashSpeed);
    }

    private void UpdateVisualRotation()
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

    private void ApplyInputs()
    {
        if (!recordInput)
            return;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            print("left");
            MovePlayer(Direction.LEFT);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            print("right");
            MovePlayer(Direction.RIGHT);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            print("jump");
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("dash");
            Dash();
        }
    }

    private void MovePlayer(Direction direction)
    {
        if (!canMove)
            return;
        Direction = direction;
        var moveSpeed = Direction == Direction.LEFT? MoveSpeed : -MoveSpeed;
        moveSpeed *= Time.deltaTime;
        RotatePlayer(moveSpeed);
    }

    private void RotatePlayer(float speed)
    {
        transform.Rotate(new Vector3(0f, 0f, speed));
    }

    private void Jump()
    {
        playerRB2D.AddForce(-GetOrientation() * JumpForce, ForceMode2D.Impulse);
    }

    private void Dash()
    {
        if (!canDash)
            return;

        recordInput = false;
        isDashing = true;
        StartCoroutine(EndDash());
        
    }

    private IEnumerator EndDash()
    {
        yield return new WaitForSeconds(DashDuration);
        OnDashEnds();
    }

    private void OnDashEnds()
    {
        recordInput = true;
        isDashing = false;
        dashCurrentCooldown = DashCooldown;
    }


}

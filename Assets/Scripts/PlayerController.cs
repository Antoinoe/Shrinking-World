using System;
using System.Collections;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [field: SerializeField] public Transform PlayerVisual { get; private set; }
    [field: SerializeField] public float WalkSpeed {get; private set;}
    [field: SerializeField] public float VelocityMultiplier {get; private set;}
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float GravitySpeed { get; private set; }
    [field: SerializeField] public float GravityMultiplier { get; private set; }
    [field: SerializeField] public float DashSpeed { get; private set; }
    [field: SerializeField] public float DashMultiplier { get; private set; }
    [field: SerializeField] public float DashDuration { get; private set; }
    [field: SerializeField] public float DashCooldown { get; private set; }
    public Direction Direction { get; private set; }
    public bool IsGrounded { get; internal set; }

    [SerializeField] private AnimationCurve VelocityOverDistance;

    private Rigidbody2D playerRB2D;
    private CapsuleCollider2D collider2D;
    private SpriteRenderer spriteRenderer;
    private float dashCurrentCooldown;
    private bool recordInput;
    private bool canDash;
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
        WalkSpeed = speed;
    }

    public void SetJumpForce(float jumpForce)
    {
        JumpForce = jumpForce;
    }

    private void Awake()
    {
        playerRB2D = PlayerVisual.GetComponent<Rigidbody2D>();
        spriteRenderer = PlayerVisual.GetComponent<SpriteRenderer>();
        collider2D = PlayerVisual.GetComponent<CapsuleCollider2D>();

        if (!playerRB2D || !spriteRenderer)
        {
            Debug.LogError("could not retrieve all components form player visual");
            return;
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGameOver.AddListener(()=>KillPlayer());
        GameManager.Instance.OnPauseToggled.AddListener(()=>OnPauseToggled());
        GameManager.Instance.PlanetController.OnPlanetExplosionStart.AddListener(() => OnPlanetExplosionStart());
        InitPlayer();
    }

    private void OnPlanetExplosionStart()
    {
        GameManager.Instance.CameraController.DoFollowPlayer = false;
        collider2D.enabled = false;
        Jump(JumpForce * 10, true);
    }

    private void OnPauseToggled()
    {
        recordInput = !GameManager.Instance.IsGamePaused;
    }

    private void Update()
    {
        ApplyVelocityMultiplier();
        ApplyDashCoolDown();
        HandleInputs();
        ApplyGravity();
        ApplyDashVelocity();
        UpdateVisualRotation();
        UpdateSpriteFlipX();
    }

    private void UpdateSpriteFlipX()
    {
        spriteRenderer.flipX = Direction == Direction.RIGHT;
    }

    private void ApplyVelocityMultiplier()
    {
        var planetController = GameManager.Instance.PlanetController;
        var distanceNormalized = (planetController.Size - planetController.EndSize) / (planetController.StartSize - planetController.EndSize);
        print($"{distanceNormalized}");
        var multiplier = 1 + VelocityOverDistance.Evaluate(distanceNormalized);
        VelocityMultiplier = multiplier;
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

        var dashSpeed = DashSpeed * DashMultiplier * VelocityMultiplier* Time.deltaTime;
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

    private void HandleInputs()
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
            Jump(JumpForce);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("dash");
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("pause");
            GameManager.Instance.TogglePause();
        }
    }

    private void MovePlayer(Direction direction)
    {
        if (!canMove)
            return;
        Direction = direction;
        var moveSpeed = Direction == Direction.LEFT? WalkSpeed : -WalkSpeed;
        moveSpeed *= Time.deltaTime * VelocityMultiplier;
        RotatePlayer(moveSpeed);
    }

    private void RotatePlayer(float speed)
    {
        transform.Rotate(new Vector3(0f, 0f, speed));
    }

    private void Jump(float jumpForce, bool forceJump = false)
    {
        if(IsGrounded || forceJump)
            playerRB2D.AddForce(-GetOrientation() * jumpForce, ForceMode2D.Impulse);
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

    internal void SetDashSpeed(float dashSpeed)
    {
        DashSpeed = dashSpeed;
    }

    internal void SetDashDelay(float dashDelay)
    {
        DashCooldown = Math.Clamp(dashDelay, DashDuration, 10);
    }
}

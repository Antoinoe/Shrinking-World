using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float gravitySpeed = 9.81f;
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] private float jumpForce = 30f;

    private Rigidbody2D playerRB2D;

    private void Awake()
    {
        playerRB2D = playerVisual.GetComponent<Rigidbody2D>();
        if(playerRB2D == null)
        {
            Debug.LogError("could not retrieve rigidbody2D form player visual");
            return;
        }
    }

    private void Update()
    {
        playerVisual.transform.LookAt(new Vector3(playerVisual.transform.position.x, playerVisual.transform.position.y, transform.position.z));
        GetInputs();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        var dir = ((Vector2)transform.position - (Vector2)playerVisual.transform.position).normalized;
        playerRB2D.AddForce(dir * gravitySpeed * gravityMultiplier * Time.fixedDeltaTime);
    }

    private void GetInputs()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            print("left");
            transform.Rotate(new Vector3(0f, 0f, moveSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            print("right");
            transform.Rotate(new Vector3(0f, 0f, -moveSpeed * Time.deltaTime));
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            print("jump");
            var dir = ((Vector2)transform.position - (Vector2)playerVisual.transform.position).normalized;
            playerRB2D.AddForce(-dir * jumpForce, ForceMode2D.Impulse);
        }
    }
}

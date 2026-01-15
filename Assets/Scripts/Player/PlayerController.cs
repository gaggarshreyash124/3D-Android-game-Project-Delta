using Unity.Android.Gradle.Manifest;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public PlayerInputHandler inputHandler;
    public Rigidbody rb;
    [Header("Movement Values")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Check Variables")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    [Header("Cam Sensitivity")]
    public float Sensitivity = 2f;

    [Header("Physics Values")]
    public float GravityScale = 10;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }
    void FixedUpdate()
    {   
        RotatePlayer(inputHandler.lookInput);
        rb.linearVelocity = new Vector3(inputHandler.MoveInput.x * moveSpeed, rb.linearVelocity.y ,inputHandler.MoveInput.y * moveSpeed);
    }
    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position,1f,groundLayer);
    }
    public void RotatePlayer(Vector2 lookInput)
    {
        Vector3 rotation = new Vector3(0, lookInput.x, 0);  
        transform.Rotate(rotation.normalized * Sensitivity);
    }
}

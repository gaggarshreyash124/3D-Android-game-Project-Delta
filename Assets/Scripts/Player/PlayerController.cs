using System.Collections;
using Unity.Android.Gradle.Manifest;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputHandler inputHandler;
    public Rigidbody rb;
    public PlayerData playerData;
    public Transform GroundCheck;
    [Header("Cam Sensitivity")]
    public CinemachineOrbitalFollow cam;


    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody>();
    }   
    void FixedUpdate()
    {   
        if (!playerData.inCombat)
        {
            
            HandleMovement();
            if (playerData.EnterCombat != 1)
            {
                playerData.EnterCombat = 1;
            }
        }
        else if (playerData.inCombat && playerData.EnterCombat == 1)
        {
            playerData.EnterCombat -= 1;
            rb.Move(CombatManager.instance.PlayerPos.position,quaternion.identity);
        }
        
    }
    void Update()
    {
        HandleDrag();
        SpeedControl();
        if (inputHandler.InteractInput)
        {
            Transform Target = CombatManager.instance.Enemy1Pos;
            StartCoroutine(CombatManager.instance.OnPlayerAttack(rb,Target,2f, playerData.PositionMoveSpeed));
            inputHandler.InteractInput = false;
        }
    }
    void HandleMovement()
    {
        Vector2 input = inputHandler.MoveInput;
        if (input.sqrMagnitude < 0.01f) return;

        // Camera-relative directions
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        // Remove vertical influence
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Final movement direction
        Vector3 moveDir = camForward * input.y + camRight * input.x;

        if(IsGrounded())
            rb.AddForce(moveDir * playerData.moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!IsGrounded())
            rb.AddForce(moveDir * playerData.moveSpeed * 10f * playerData.airMultiplier, ForceMode.Force);

        // Rotate player toward movement
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));

    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > playerData.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * playerData.moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    
    void HandleDrag()
    {
        rb.linearDamping = IsGrounded() ? playerData.groundDrag : playerData.airDrag;
    }
    public bool IsGrounded()
    { 
        return Physics.CheckSphere(GroundCheck.position, .5f, playerData.groundLayer);
    }

    public IEnumerator Rocksmash()
    {
        yield return null;
    }
}




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
        SetTarget();
        if (inputHandler.InteractInput)
        {
            StartCoroutine(CombatManager.instance.OnPlayerAttack(rb,2f, playerData.PositionMoveSpeed));
            inputHandler.InteractInput = false;
        }
    }
    void HandleMovement()
    {
        Vector2 input = inputHandler.MoveInput;

        if (input.sqrMagnitude < 0.01f) return;

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;

        if(IsGrounded())
            rb.AddForce(moveDir * playerData.moveSpeed * 10f, ForceMode.Force);

        else if(!IsGrounded())
            rb.AddForce(moveDir * playerData.moveSpeed * 10f * playerData.airMultiplier, ForceMode.Force);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));

    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

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

    public void SpawnRock()
    {
       Instantiate(playerData.Rock,transform.position + transform.forward * 2f, Quaternion.identity);
    }
    
    public void SetTarget()
    {
        if (Time.time - playerData.lastTargetSetTime < playerData.targetSetCooldown) return;

        switch (inputHandler.SetTarget)
        {
            case Vector2 v when v == Vector2.up:
                CombatManager.instance.SetCombatTarget(CombatManager.instance.Enemy1Pos);
                playerData.lastTargetSetTime = Time.time;
                break;

            case Vector2 v when v == Vector2.down:
                CombatManager.instance.SetCombatTarget(CombatManager.instance.Enemy2Pos);
                playerData.lastTargetSetTime = Time.time;
                break;

            case Vector2 v when v == Vector2.left:
                CombatManager.instance.SetCombatTarget(CombatManager.instance.Enemy3Pos);
                playerData.lastTargetSetTime = Time.time;
                break;

            case Vector2 v when v == Vector2.right:
                CombatManager.instance.SetCombatTarget(CombatManager.instance.Enemy4Pos);
                playerData.lastTargetSetTime = Time.time;
                break;
        }
    }
}
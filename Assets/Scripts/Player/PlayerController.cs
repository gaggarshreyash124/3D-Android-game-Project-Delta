using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    #region References

    [Header("Core")]
    public PlayerData playerData;
    public Transform attackPoint;
    public Transform groundCheck;
    public CinemachineCamera cam;

    private PlayerInputHandler inputHandler;
    private Rigidbody rb;

    #endregion

    #region Attack Settings

    [Header("Attack Settings")]
    [SerializeField] private float capsuleRadius = 0.5f;
    [SerializeField] private float capsuleHalfLength = 1f;

    private LayerMask enemyLayer;

    #endregion

    #region Initialization

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody>();

        enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Start()
    {
        // Safety reset
        if (playerData.inCombat && playerData.EnterCombat <= 1)
        {
            playerData.inCombat = false;
            playerData.EnterCombat = 0;
        }
    }

    #endregion

    #region Unity Loops

    private void Update()
    {
        HandleCombatInput();
        HandleTargetSelection();
    }

    private void FixedUpdate()
    {
        if (!playerData.inCombat)
            HandleMovement();

        ApplyDrag();
        LimitSpeed();
    }

    #endregion

    #region Ground Check

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position ,0.5f ,playerData.groundLayer );
    }

    #endregion

    #region Combat

    private void HandleCombatInput()
    {
        if (!inputHandler.InteractInput) return;

        if (playerData.EnterCombat != 0) return;

        if (TryGetEnemy(out GameObject enemy))
        {
            playerData.inCombat = true;
            playerData.EnterCombat = 1;

            CombatManager.Instance.StartCombat(this.transform, enemy.transform);
            CamaraManager.instance.Switch(CamaraManager.instance.CombatCam);
        }
        rb.isKinematic = true;
        
    }

    public bool TryGetEnemy(out GameObject enemy)
    {
        enemy = null;

        Vector3 forward = attackPoint.forward;

        Vector3 pointA = attackPoint.position + forward * capsuleHalfLength;
        Vector3 pointB = attackPoint.position - forward * capsuleHalfLength;

        Collider[] hits = Physics.OverlapCapsule( pointA, pointB, capsuleRadius, enemyLayer);

        if (hits.Length == 0) return false;

        enemy = hits[0].gameObject;
        return true;
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        Vector2 input = inputHandler.MoveInput;
        if (input.sqrMagnitude < 0.01f) return;

        bool grounded = IsGrounded();

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;

        float multiplier = grounded ? 1f : playerData.airMultiplier;

        rb.AddForce(moveDir * playerData.moveSpeed * 10f * multiplier, ForceMode.Force);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        rb.MoveRotation(
            Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime)
        );
    }

    private void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVelocity.magnitude <= playerData.moveSpeed) return;

        Vector3 limited = flatVelocity.normalized * playerData.moveSpeed;

        rb.linearVelocity = new Vector3(
            limited.x,
            rb.linearVelocity.y,
            limited.z
        );
    }

    private void ApplyDrag()
    {
        rb.linearDamping = IsGrounded()
            ? playerData.groundDrag
            : playerData.airDrag;
    }

    #endregion

    #region Target Selection

    private void HandleTargetSelection()
    {
        if (Time.time - playerData.lastTargetSetTime < playerData.targetSetCooldown) return;

        Vector2 input = inputHandler.SetTarget;

        if (input == Vector2.up)
            SetTarget(CombatManager.Instance.enemy1Position);

        else if (input == Vector2.down)
            SetTarget(CombatManager.Instance.enemy2Position);

        else if (input == Vector2.left)
            SetTarget(CombatManager.Instance.enemy3Position);

        else if (input == Vector2.right)
            SetTarget(CombatManager.Instance.enemy4Position);
    }

    private void SetTarget(Transform target)
    {
        CombatManager.Instance.SetCombatTarget(target);
        playerData.lastTargetSetTime = Time.time;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (!attackPoint) return;

        Gizmos.color = Color.red;

        Vector3 forward = attackPoint.forward;
        Vector3 pointA = attackPoint.position + forward * capsuleHalfLength;
        Vector3 pointB = attackPoint.position - forward * capsuleHalfLength;

        Gizmos.DrawWireSphere(pointA, capsuleRadius);
        Gizmos.DrawWireSphere(pointB, capsuleRadius);
        Gizmos.DrawLine(pointA, pointB);
    }

    #endregion
}

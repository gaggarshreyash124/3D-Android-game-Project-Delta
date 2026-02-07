using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement Values")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float airMultiplier;

    [Header("Physics Values")]
     public float groundDrag = 5f;
    public float airDrag = 0f;

    [Header("Combat Variables")]
    public bool inCombat;
    public float EnterCombat;
    public float PositionMoveSpeed = 5f;
    public float targetSetCooldown = 3f;
    public float lastTargetSetTime = 0;

    [Header("Check Variables")]
    public LayerMask groundLayer;

    [Header("Cam Sensitivity")]
    public float Sensitivity = 2f;

    [Header("Weapons")]
    public GameObject Rock;

}

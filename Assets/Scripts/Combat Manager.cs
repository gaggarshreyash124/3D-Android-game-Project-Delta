using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region Singleton

    public static CombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #endregion

    #region Inspector References

    [Header("Combat Positions")]
    public Transform playerPosition;
    public Transform enemy1Position;
    public Transform enemy2Position;
    public Transform enemy3Position;
    public Transform enemy4Position;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    #endregion

    #region Runtime State

    private Transform currentTarget;
    private int playerCombatOffset = -2;

    private readonly Collider[] enemyBuffer = new Collider[5]; // Non-alloc buffer
    private LayerMask enemyLayer;

    public States CurrentState { get; private set; } = States.Player;

    #endregion

    #region Initialization

    private void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        currentTarget = enemy1Position;

    }

    #endregion

    #region Unity Loops

    void Update()
    {
        CamaraManager.instance.CombatCam.LookAt = currentTarget;
    }
    #endregion

    #region Combat Flow

    public void StartCombat(Transform player, Transform selectedEnemy)
    {
        player.position = playerPosition.position;

        SpawnEnemies();

        CurrentState = States.Player;
    }

    private void SpawnEnemies()
    {
        if (enemyPrefabs.Length < 2)
        {
            Debug.LogError("Not enough enemy prefabs assigned.");
            return;
        }

        GameObject[] spawnSet = { enemyPrefabs[0], enemyPrefabs[1], enemyPrefabs[1], enemyPrefabs[0] };

        Transform[] spawnPositions ={ enemy1Position, enemy2Position, enemy3Position, enemy4Position };

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Instantiate(spawnSet[i], spawnPositions[i].position, Quaternion.identity);
        }
    }

    #endregion

    #region Targeting

    public void SetCombatTarget(Transform newTarget)
    {
        if (newTarget == null) return;

        currentTarget = newTarget;
        Debug.Log($"Target Set: {currentTarget.name}");
    }

    public bool TryGetSelectedEnemy(out EnemyController enemy)
    {
        enemy = null;

        if (currentTarget == null)
            return false;

        int hitCount = Physics.OverlapSphereNonAlloc( currentTarget.position, 2f, enemyBuffer, enemyLayer);

        if (hitCount == 0)
            return false;

        enemy = enemyBuffer[0].GetComponent<EnemyController>();
        return enemy != null;
    }

    #endregion

    #region Player Attack

    public IEnumerator PlayerAttackRoutine(Rigidbody rb, float delayTime, float speed)
    {
        Vector3 startPos = playerPosition.position;
        Vector3 attackPos = GetPlayerAttackPosition();

        yield return MoveOverTime(rb, startPos, attackPos, speed);

        yield return new WaitForSeconds(delayTime);

        yield return MoveOverTime(rb, attackPos, startPos, speed);
    }

    #endregion

    #region Enemy Attack

    public IEnumerator EnemyAttackRoutine(Rigidbody rb, float delayTime, float speed)
    {
        Vector3 startPos = currentTarget.position;
        Vector3 attackPos = GetEnemyAttackPosition();

        yield return MoveOverTime(rb, startPos, attackPos, speed);

        yield return new WaitForSeconds(delayTime);

        yield return MoveOverTime(rb, attackPos, startPos, speed);
    }

    #endregion

    #region Shared Movement Logic

    private IEnumerator MoveOverTime(Rigidbody rb, Vector3 from, Vector3 to, float speed)
    {
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * speed;
            rb.MovePosition(Vector3.Lerp(from, to, elapsed));
            yield return null;
        }

        rb.MovePosition(to);
    }

    private Vector3 GetPlayerAttackPosition()
    {
        return new Vector3( currentTarget.position.x, currentTarget.position.y, currentTarget.position.z + playerCombatOffset);
    }

    private Vector3 GetEnemyAttackPosition()
    {
        return new Vector3( playerPosition.position.x, playerPosition.position.y, playerPosition.position.z - playerCombatOffset);
    }

    #endregion

    #region State Machine

    public void SwitchState(States newState)
    {
        CurrentState = newState;

        switch (CurrentState)
        {
            case States.Player:
                HandlePlayerTurn();
                break;

            case States.Attack:
                break;

            case States.Enemy:
                break;
        }
    }

    private void HandlePlayerTurn()
    {
        // Future expansion
    }

    #endregion
}

public enum States
{
    Player,
    Attack,
    Enemy
}

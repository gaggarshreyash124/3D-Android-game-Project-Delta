using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [Header("Positions")]
    public Transform PlayerPos;
    public Transform Enemy1Pos;
    public Transform Enemy2Pos;
    public Transform Enemy3Pos;
    public Transform Enemy4Pos;
    Transform Target ;
    EnemyController enemy = null;
    bool inCombat = false;
    private int playerCombatPos = -2;
    public Vector3 cuurentpos;
    public Vector3 enemypos;

    public GameObject[] Enemies;

    public States states = States.Player;

    public bool SelectedEnemy(out EnemyController enemy)
    {
        enemy = null;

        Collider[] Hits = Physics.OverlapSphere(Target.position,2f,LayerMask.GetMask("Enemy"));

        if (Hits.Length == 0)
            return false;

        enemy = Hits[0].GetComponent<EnemyController>();
        return true;
    }
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Target = Enemy1Pos;
    }

    public void StartCombat(Transform player,Transform Senemy)
    {
        player.position = PlayerPos.position;

        GameObject[] spawnPrefabs = { Enemies[0], Enemies[1], Enemies[1], Enemies[0] };
        Transform[] spawnPositions = { Enemy1Pos, Enemy2Pos, Enemy3Pos, Enemy4Pos };

        // Spawn loop
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Spawn");
            Instantiate(spawnPrefabs[i], spawnPositions[i].position, Quaternion.identity);
        }
    }

    public void SetCombatTarget(Transform NewTarget)
    {
        Target = NewTarget;
        Debug.Log("Target Set" + Target.name);
    }

    public IEnumerator OnPlayerAttack(Rigidbody Rb, float DelayTime,float Speed)
    {
        float ElapsedTime = 0;

        while (ElapsedTime < 1)
        {
            Rb.MovePosition(Vector3.Lerp(PlayerPos.position, new Vector3(Target.position.x,Target.position.y,Target.position.z + playerCombatPos),ElapsedTime));
            ElapsedTime += Time.deltaTime * Speed;
            yield return null;
        }

        yield return new WaitForSeconds(DelayTime);
        ElapsedTime = 0;
        
        while (ElapsedTime < 1)
        {
            Rb.MovePosition(Vector3.Lerp( new Vector3(Target.position.x,Target.position.y,Target.position.z + playerCombatPos),PlayerPos.position,ElapsedTime));
            ElapsedTime += Time.deltaTime * Speed;
            yield return null;
        }
    }

    public IEnumerator OnEnemyAttack(Rigidbody Rb, float DelayTime, float Speed)
    {
        float ElapsedTime = 0;

        while (ElapsedTime < 1)
        {
            Rb.MovePosition(Vector3.Lerp(Target.position, new Vector3(PlayerPos.position.x, PlayerPos.position.y, PlayerPos.position.z + -playerCombatPos), ElapsedTime));
            ElapsedTime += Time.deltaTime * Speed;
            yield return null;
        }

        yield return new WaitForSeconds(DelayTime);
        ElapsedTime = 0;

        while (ElapsedTime < 1)
        {
            Rb.MovePosition(Vector3.Lerp(new Vector3(PlayerPos.position.x, PlayerPos.position.y, PlayerPos.position.z + -playerCombatPos), Target.position, ElapsedTime));
            ElapsedTime += Time.deltaTime * Speed;
            yield return null;
        }
    }

    public void PlayerTurn()
    {
        
    }
    public void StateSwitch()
    {
        switch(states)
        {
            case States.Player:
                break;
            case States.Attack:
                break;
            case States.Enemy:
                break;
        }
    }
}
public enum States
{
    Player,
    Attack,
    Enemy,
}
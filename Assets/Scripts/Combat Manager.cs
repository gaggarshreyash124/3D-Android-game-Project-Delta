using System.Collections;
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
    public void SetCombatTarget(Transform NewTarget)
    {
        Target = NewTarget;
        Debug.Log("Target Set" + Target.name);
    }
    public IEnumerator OnPlayerAttack(Rigidbody Rb, float DelayTime,float Speed)
    {
        inCombat = true;
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
        inCombat = false;
    }

    public IEnumerator OnEnemyAttack()
    {
        yield return new WaitForSeconds(2);
        
    }
    public void GetEnemyHealthBar(Transform Target)
    {
        Collider Hit = Physics.OverlapSphere(Target.position, 0.5f, LayerMask.GetMask("Enemy"))[0];
        if (Hit != null)
        {
            enemy = Hit.GetComponent<EnemyController>();
        }
    }
}

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

    float CustomTime;
    private int playerCombatPos = -2;
    bool reached;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public IEnumerator OnPlayerAttack(Rigidbody Rb, Transform Target, float DelayTime,float Speed)
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

    public IEnumerator OnEnemyAttack()
    {
        yield return new WaitForSeconds(2);
        
    }
}

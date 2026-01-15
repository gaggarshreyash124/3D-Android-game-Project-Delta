using UnityEngine;
public class EnemyControllerbitch : MonoBehaviour
{
    public float PatrolSpeed;
    public float TimeCounter;
    public float PatrolTime;
    public float IdleTime;
    Vector3 PatrolDirection;
    bool Patrolling = true;
    bool Idleing = false;
    public void FixedUpdate()
    {
        TimeCounter +=Time.fixedDeltaTime;
        if (TimeCounter >= PatrolTime && Patrolling)
        {
            PatrolSpeed = 0f;   
            Idleing = true;
            Patrolling = !Patrolling;
            TimeCounter = 0;
        }
        else if(TimeCounter >= IdleTime && Idleing)
        {
            PatrolSpeed = 10;
            Patrolling = true;
            Idleing = !Idleing;
            TimeCounter = 0;
            setDirection();
            Debug.Log(PatrolDirection);
        }     
        Debug.Log(TimeCounter);
        transform.Translate(PatrolDirection * PatrolSpeed * Time.fixedDeltaTime);
    }
    void setDirection()
    {
        PatrolDirection = new Vector3(Random.Range(0,20),0,Random.Range(0,20));
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour,IDamagable
{
    public float PatrolSpeed;
    public NavMeshAgent En;
    public float Range = 10f;
    public float MaxHealth = 200f;
    public float CurrentHealth;


    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    void FixedUpdate()
    {
        if (En.pathPending || !En.isOnNavMesh || En.remainingDistance > 0.1f) return;
        StartCoroutine(StartPatrol());
    }
    
    IEnumerator StartPatrol()
    {
        yield return new WaitForSeconds(2f);
        En.destination = new Vector3(Random.Range(transform.position.x + Range,transform.position.x-Range),0,Random.Range(transform.position.z + Range,transform.position.z - Range));
    }

    public void TakeDamage(float damage,int attackcount)
    {
        for (int i = 0; i < attackcount; i++)
        {
            CurrentHealth -= damage;
        }
        if (CurrentHealth <= 0)
        {
            Debug.Log("Enemy Defeated");
            Destroy(gameObject,2f);
        }
    } 
}

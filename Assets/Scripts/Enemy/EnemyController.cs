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

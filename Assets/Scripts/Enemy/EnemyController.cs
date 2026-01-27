using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public float PatrolSpeed;
    public NavMeshAgent En;
    public float Range = 10f;


    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if (En.pathPending || !En.isOnNavMesh || En.remainingDistance > 0.1f)
                return;
        StartCoroutine(StartPatrol());
        
    }

    IEnumerator StartPatrol()
    {
        yield return new WaitForSeconds(2f);
        En.destination = new Vector3(Random.Range(transform.position.x + Range,transform.position.x-Range),0,Random.Range(transform.position.z + Range,transform.position.z - Range));
    }
}

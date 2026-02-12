using UnityEngine;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] PlayerController player;
    CinemachineCamera NewCam;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}

public interface IDamagable
{
    public void TakeDamage(float damage,int attackcount);
}
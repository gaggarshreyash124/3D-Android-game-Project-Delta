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
    void Update()
    {
        if (player.playerData.inCombat)
        {
            NewCam = CamaraManager.instance.CombatCam;
            CamaraManager.instance.Switch(NewCam);
        }
        else
        {
            NewCam = CamaraManager.instance.PlayerFollow;
            CamaraManager.instance.Switch(NewCam);  
        }
    }
}
public interface IDamagable
{
    public void TakeDamage(float damage,int attackcount);
}
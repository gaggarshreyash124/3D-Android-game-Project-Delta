using UnityEngine;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public bool InCombat;
    public CinemachineCamera FollowCam;
    public CinemachineCamera CombatCam;
    private CinemachineCamera Activecam;
    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            instance = this;
        }
    }
    public void SwitchCam(CinemachineCamera NewCam)
    {
        NewCam = Activecam;
        NewCam.Priority = 10;
    }
    

}

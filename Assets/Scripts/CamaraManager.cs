
using UnityEngine;
using Unity.Cinemachine;

public class CamaraManager : MonoBehaviour
{
    public CinemachineCamera CurrentCam;
    public CinemachineCamera PlayerFollow;
    public CinemachineCamera CombatCam;
    public static CamaraManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        Initialized(PlayerFollow);
    }
    void Update()
    {
    
    }
    public void Initialized(CinemachineCamera NewCam)
    {
        CurrentCam = NewCam;
        CurrentCam.Priority = 10;
    }
    public void Switch(CinemachineCamera NewCam)
    {
        CurrentCam.Priority = 0;
        CurrentCam = NewCam;
        CurrentCam.Priority = 10;
    }

}
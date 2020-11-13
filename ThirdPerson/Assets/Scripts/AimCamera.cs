using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimCamera : MonoBehaviour
{
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    [SerializeField]
    CinemachineVirtualCamera aimCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }
}

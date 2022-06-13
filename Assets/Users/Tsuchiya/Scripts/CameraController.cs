using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook _cinemachineFreeLook;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 Axis = SwitchInputManager.Instance.GetAxis(SwitchInputManager.AnalogStick.Right);
        _cinemachineFreeLook.m_YAxis.m_InputAxisValue = Axis.y;
        _cinemachineFreeLook.m_XAxis.m_InputAxisValue = Axis.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook _cinemachineFreeLook;

    public bool isControll;
    // Start is called before the first frame update
    void Start()
    {
        isControll = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isControll == true)
        {
            Vector2 Axis = SwitchInputManager.Instance.GetAxis(SwitchInputManager.AnalogStick.Right);
            _cinemachineFreeLook.m_YAxis.m_InputAxisValue = Axis.y;
            _cinemachineFreeLook.m_XAxis.m_InputAxisValue = Axis.x;
        }
    }
}

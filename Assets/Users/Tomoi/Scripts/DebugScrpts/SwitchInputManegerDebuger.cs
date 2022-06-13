using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInputManegerDebuger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
        {
            Debug.Log("if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))");
        }
        else if (SwitchInputManager.Instance.GetKey(SwitchInputManager.JoyConButton.A))
        {
            Debug.Log("        else if (SwitchInputManager.Instance.GetKey(SwitchInputManager.JoyConButton.A))");
        }else {
            Debug.Log("false");
        }
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.Up))
        {
            Debug.Log("if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.Up))");
        }
        else if (SwitchInputManager.Instance.GetKey(SwitchInputManager.JoyConButton.Up))
        {
            Debug.Log("        else if (SwitchInputManager.Instance.GetKey(SwitchInputManager.JoyConButton.Up))");
        }else {
            Debug.Log("false");
        }
        */
        //if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
        {
            Debug.Log(SwitchInputManager.Instance.GetAxis(SwitchInputManager.AnalogStick.Right));   
            Debug.Log(SwitchInputManager.Instance.GetAxis(SwitchInputManager.AnalogStick.Left));   
        }
    }
}

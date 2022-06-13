using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyConAngleCheckTest : MonoBehaviour
{
    [SerializeField]
    private Image[] _DebugImages = new Image[8];

    [SerializeField] private Image[] _DebugPointerImages = new Image[2];
    void Update()
    {
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.Plus))
        {
            JoyConToScreenPointer.Instance.AngleReset();
        }

        _DebugPointerImages[0].transform.position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;
        _DebugPointerImages[1].transform.position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
        foreach (Image _image in _DebugImages)
        {
            _image.color = Color.black;
        }
        _DebugImages[(int)JoyConAngleCheck.GetJoyConAnglePosition(nn.hid.NpadJoyDeviceType.Right)].color = Color.red;
        _DebugImages[(int)JoyConAngleCheck.GetJoyConAnglePosition(nn.hid.NpadJoyDeviceType.Left)].color = Color.blue;
    }
}

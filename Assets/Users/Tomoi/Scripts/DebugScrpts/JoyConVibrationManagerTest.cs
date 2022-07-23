using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConVibrationManagerTest : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("右ジョイコン1秒"))
        {
            JoyConVibrationManager.Instance.JoyConVibration(JoyCon.Right,1f,JoyConVibrationManager.VibrationType.Random);
        }
        if (GUILayout.Button("左ジョイコン1秒"))
        {
            JoyConVibrationManager.Instance.JoyConVibration(JoyCon.Left,1f,JoyConVibrationManager.VibrationType.Random);
        }
        if (GUILayout.Button("両方のジョイコン1秒"))
        {
            JoyConVibrationManager.Instance.JoyConVibration(JoyCon.Both,1f,JoyConVibrationManager.VibrationType.Random);
        }
        if (GUILayout.Button("右ジョイコン3秒"))
        {
            JoyConVibrationManager.Instance.JoyConVibration(JoyCon.Right,3f,JoyConVibrationManager.VibrationType.Random);
        }
        if (GUILayout.Button("左ジョイコン3秒"))
        {
            JoyConVibrationManager.Instance.JoyConVibration(JoyCon.Left,3f,JoyConVibrationManager.VibrationType.Random);
        }
        if (GUILayout.Button("両方のジョイコン3秒"))
        {
            JoyConVibrationManager.Instance.JoyConVibration(JoyCon.Both,3f,JoyConVibrationManager.VibrationType.Random);
        }
    }
}

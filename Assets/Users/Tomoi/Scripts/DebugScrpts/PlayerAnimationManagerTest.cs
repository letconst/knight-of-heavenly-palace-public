using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManagerTest : MonoBehaviour
{
    [SerializeField] private float al;
    [SerializeField] private float ar;
    private void OnGUI()
    {
        if (GUILayout.Button("ThrowSwordR"))
        {
            PlayerAnimationManager.Instance.ThrowSwordR();
        }
        if (GUILayout.Button("ThrowSwordL"))
        {
            PlayerAnimationManager.Instance.ThrowSwordL();
        }
        
        if (GUILayout.Button("LeftHangingAttack"))
        {
            PlayerAnimationManager.Instance.LeftHangingAttack(al);
        }
        if (GUILayout.Button("RightHangingAttack"))
        {
            PlayerAnimationManager.Instance.RightHangingAttack(ar);
        }

        GUILayout.Space(30);
        
        if (GUILayout.Button("RightHandOpen"))
        {
            PlayerAnimationManager.Instance.RightHandOpen();
        }
        if (GUILayout.Button("RightHandClose"))
        {
            PlayerAnimationManager.Instance.RightHandClose();
        }
        if (GUILayout.Button("LeftHandOpen"))
        {
            PlayerAnimationManager.Instance.LeftHandOpen();
        }
        if (GUILayout.Button("LeftHandClose"))
        {
            PlayerAnimationManager.Instance.LeftHandClose();
        }
        if (GUILayout.Button("HandLayerFalse"))
        {
            PlayerAnimationManager.Instance.HandLayerFalse();
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Joyconの座標を習得して描画処理を行うクラス
public class CursorDraw : MonoBehaviour
{
    [SerializeField, Header("描画するオブジェクト")] private GameObject _drawObjectR ;
    [SerializeField, Header("描画するオブジェクト")] private GameObject _drawObjectL;

    private RectTransform _cursorObjR;
    private RectTransform _cursorObjL;

    private void Start()
    {
        _cursorObjR = _drawObjectR.GetComponent<RectTransform>();
        _cursorObjL = _drawObjectL.GetComponent<RectTransform>();
    }

    public void Draw()
    {
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
        {
            JoyConToScreenPointer.Instance.AngleReset();
        }
        
        _cursorObjR.position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
        _cursorObjL.position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;
    }
    
    /// <summary>
    /// カーソル表示を表示(アクティブ)と非表示(非アクティブ)にする関数
    /// </summary>
    /// <param name="isActive"> 表示するかしないか </param>>
    public void ActiveDraw(bool isActive)
    {
        _drawObjectR.SetActive(isActive);
        _drawObjectL.SetActive(isActive);
    }
}

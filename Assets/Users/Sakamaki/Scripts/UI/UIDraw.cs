using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI関係の描写を行うクラス
public class UIDraw : MonoBehaviour
{
    [SerializeField] private CursorDraw _cursorDraw;
    [SerializeField] private Text _text;            // 雑にステータスText実装

    private bool isCursorInit = false;              // カーソルが表示された後の初期化処理
    private void Start()
    {
        _cursorDraw = GetComponent<CursorDraw>();
    }

    void Update()
    {
        // 抜刀モードと投擲モード時にカーソルの表示処理と位置初期化
        if (PlayerStatus.playerAttackState == PlayerStatus.PlayerAttackState.SwordPulling ||
            PlayerStatus.playerAttackState == PlayerStatus.PlayerAttackState.Throwing)
        {
            if (!isCursorInit)
            {
                isCursorInit = true;
                JoyConToScreenPointer.Instance.AngleReset();
                _cursorDraw.ActiveDraw(true);
            }
            
            _cursorDraw.Draw();
        }
        // それ以外は非表示
        else
        {
            isCursorInit = false;
            _cursorDraw.ActiveDraw(false);
        }
        
#if UNITY_EDITOR
        if (_text)
        {
            _text.text = PlayerStatus.playerAttackState.ToString();
        }
#endif
    }
}

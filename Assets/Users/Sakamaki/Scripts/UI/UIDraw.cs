using UnityEngine;

// UI関係の描写を行うクラス
public class UIDraw : MonoBehaviour
{
    [SerializeField] private EnemyCount _enemyCount;
    [SerializeField] private CursorDraw _cursorDraw;
#if UNITY_EDITOR
    [SerializeField] private StateChecker _stateChecker; // ステートチェッカー
#endif
    [SerializeField, Tooltip("ステートのチェックを行うかどうかのデバックフラグ")] 
    private bool isStateCheck = false;
    
    private bool isCursorInit = false; // カーソルが表示された後の初期化処理

    private void Start()
    {
        _cursorDraw = GetComponent<CursorDraw>();
        _enemyCount = GetComponent<EnemyCount>();
    }

    void Update()
    {
        // 抜刀モードと投擲モード時にカーソルの表示処理と位置初期化
        // 2022/08/30 投擲モードを追加したので抜刀時はカーソルを表示しないように変更
        if (/*PlayerStateManager.HasFlag(PlayerStatus.PlayerState.SwordPulling) ||*/
            PlayerStateManager.HasFlag(PlayerStatus.PlayerState.ThrowingMode))
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
        if (isStateCheck)
            _stateChecker.ViewState();
#endif
    }
}
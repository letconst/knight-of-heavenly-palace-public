using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // プレイヤーの移動処理関連を管理するクラス

    // 移動関係
    [SerializeField, Header("移動させるオブジェクト")]
    private GameObject _playerObject;
    [SerializeField, Header("正面設定を行うカメラオブジェクト")]
    private GameObject _cameraObject;
    
    private Rigidbody rigidbody;

    // プレイヤーの計算後速度(向きとか)
    private static Vector3 _moveVelocity = Vector3.zero;

    void Start()
    {
        rigidbody = _playerObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 移動処理
        Moved(PlayerStatus.moveSpeed, rigidbody, _moveVelocity);
    }

    /// <summary>
    /// プレイヤーの移動関数
    /// </summary>
    /// <param name="moveSpeed"> 移動速度(スピード) </param>
    /// <param name="moveObject"> 移動させるオブジェクト </param>
    /// <param name="velocity"> プレイヤーの計算速度 </param>
    private void Moved(float moveSpeed, Rigidbody moveObject, Vector3 velocity)
    {

        // 入力の値用意
        Vector2 rawSwitchInput = SwitchInputManager.Instance.GetAxis(SwitchInputManager.AnalogStick.Left);
        // y -> z に変換
        Vector3 switchInput = new Vector3(rawSwitchInput.x, 0, rawSwitchInput.y);

#if UNITY_EDITOR
        // UnityEditorで編集を行っているときは、Switchの入力 + キーボードの入力を受け取る
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        switchInput += input;
#endif
        
        // 移動を行うので、走るステートをつける
        PlayerStatus.playerMoveState = PlayerStatus.PlayerMoveState.Dash;

        // 移動計算処理
        // カメラを指定し x-z間のベクトルの習得
        Vector3 cameraForward = Vector3.Scale(_cameraObject.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 入力の値とカメラの向きで移動方向を決めて変数に代入
        Vector3 moveForward = cameraForward * switchInput.z + _cameraObject.transform.right * switchInput.x;
        
        // 向きと速度で掛けて、計算後の座標を出す
        velocity = moveForward * moveSpeed;
        transform.LookAt(moveObject.transform.position + switchInput);

        // 加速度を一定に制限を行う
        velocity = velocity - moveObject.velocity;
        velocity = new Vector3(Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed), 0f,
            Mathf.Clamp(velocity.z, -moveSpeed, moveSpeed));

        // 移動処理をおこなう
        rigidbody.AddForce(moveObject.mass * velocity / Time.deltaTime, ForceMode.Force);

        // moveForwardが0じゃなかった場合計算したカメラ向きを代入
        if (moveForward != Vector3.zero)
        {
            moveObject.rotation = Quaternion.LookRotation(moveForward);
        }
    }
}
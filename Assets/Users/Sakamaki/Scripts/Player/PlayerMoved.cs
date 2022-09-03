using UnityEngine;

/// <summary>
/// 移動を管理するPlayerMovementのpartialクラス
/// </summary>
public partial class PlayerMovement
{
    private Vector3 _moveVelocity = Vector3.zero;
    
    /// <summary>
    /// プレイヤーの移動関数
    /// </summary>
    /// <param name="moveSpeed"> 移動速度(スピード) </param>
    /// <param name="moveObject"> 移動させるオブジェクト </param>
    private void Moved(float moveSpeed, Rigidbody moveObject)
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
        if (switchInput.x != 0 || switchInput.y != 0)
        {
            // 移動を行うので、走るステートをつける (イベントの発行)
            PlayerInputEventEmitter.Instance.
                Broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.Move, 
                    PlayerStateChangeOptions.Add, null, null));
        }
        
        // 移動計算処理
        // カメラを指定し x-z間のベクトルの習得
        Vector3 cameraForward = Vector3.Scale(_cameraObject.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 入力の値とカメラの向きで移動方向を決めて変数に代入
        _moveForward = cameraForward * switchInput.z + _cameraObject.transform.right * switchInput.x;
        
        // 向きと速度で掛けて、計算後の座標を出す
        _moveVelocity = _moveForward * moveSpeed;
        transform.LookAt(moveObject.transform.position + switchInput);

        // 加速度を一定に制限を行う
        _moveVelocity = _moveVelocity - moveObject.velocity;
        _moveVelocity = new Vector3(Mathf.Clamp(_moveVelocity.x, -moveSpeed, moveSpeed), 0f,
            Mathf.Clamp(_moveVelocity.z, -moveSpeed, moveSpeed));

        // 移動処理をおこなう
        rigidbody.AddForce(moveObject.mass * _moveVelocity / Time.deltaTime, ForceMode.Force);

        // moveForwardが0じゃなかった場合計算したカメラ向きを代入
        if (_moveForward != Vector3.zero)
        {
            moveObject.rotation = Quaternion.LookRotation(_moveForward);
        }
        
        StoppingNowPlayer();
    }

    /// <summary>
    /// プレイヤーが移動をやめている時の処理関数
    /// </summary>
    private void StoppingNowPlayer()
    {
        // プレイヤーのmoveの値をみて 0,0,0 とかだったら止まっている判定にしイベントを飛ばす
        if (_moveForward == Vector3.zero)
        {
            PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnStateChangeRequest
                .GetEvent(PlayerStatus.PlayerState.Standing, PlayerStateChangeOptions.Add, null, null));
        }
    }
}

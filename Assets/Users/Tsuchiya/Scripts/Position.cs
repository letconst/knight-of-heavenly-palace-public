using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Position : MonoBehaviour
{
    
    private Vector3 currentPosition;

    private bool _isGrounded;
    public bool IsGrounded { get { return _isGrounded; } }

    [SerializeField,Header("プレイヤーを平地に戻す処理を実行する高さ")]
    private float RollbackBorder;

    [SerializeField, Header("平地と見なす地面の角度のしきい値")]
    private float AngleThreshold;
    void Start()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        //yが－20以下の時プレイヤーの位置を着地していた位置に戻す。
        if (transform.position.y < RollbackBorder)
        {
            // 落下した時に一旦剣の初期化を行う
            WeaponThrowing.Instance.SwordPositionReset(PlayerInputEvent.PlayerHand.Left);
            WeaponThrowing.Instance.SwordPositionReset(PlayerInputEvent.PlayerHand.Right);
            
            transform.position = currentPosition;
        }
        
        Ray ray = new Ray(transform.position, Vector3.down);

        //地面についている時にプレイヤーの位置を記録する。
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player"))) && PlayerGrounded.isGrounded)
        {
            float angle = Vector3.Angle(hit.normal, ray.direction) - 90f;
            // あたった地点の面が平面に近いかを判定
            
            if (angle >= AngleThreshold)
            {
                currentPosition = transform.position;
            }
        }
    }
      
}

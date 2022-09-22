using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class StateControllerBase : MonoBehaviour
{
    [NonSerialized] public bool playerIsInRange = false;
    public Rigidbody rigidbody { get; private set; }
    public Transform playerTransform { get; private set; }

    [SerializeField] protected Dictionary<int, StateChildBase> stateDic = new Dictionary<int, StateChildBase>();

    /// <summary>
    /// 地面のレイヤーマスク
    /// </summary>
    private int groundLayerMask;

    /*上下に揺れる処理系の変数*/
    [SerializeField, Header("地面からの最大距離")] private float groundDistanceMax = 20f;
    [SerializeField, Header("地面からの最小距離")] private float groundDistanceMin = 5f;
    [SerializeField, Header("上下に揺れるスピード")] private float fluffySpeed = 2f;
    [SerializeField, Header("ふわふわする可動域")] private float fluffyRange = 1f;

    public float groundYPosition { get; private set; }

    /*ふわふわ揺れるタイミングのオフセット*/
    private float fluffyTimeOffset = 0f;

    [SerializeField, Header("敵本体の大きさの半径")]
    private float dragonSphereRadius = 1f;

    // 現在のステート
    public int CurrentState { protected set; get; }


    // 初期化処理
    public abstract void Initialize(int initializeStateType);

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("PlayerModel").transform;
        fluffyTimeOffset = Random.value;
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    // 更新処理
    public void UpdateSequence()
    {
        int nextState = (int)stateDic[CurrentState].StateUpdate();
        AutoStateTransitionSequence(nextState);
    }

    public void UpdateSequence(int nextState)
    {
        AutoStateTransitionSequence(nextState);
    }

    // ステートの自動遷移
    protected void AutoStateTransitionSequence(int nextState)
    {
        if (CurrentState == nextState)
        {
            return;
        }

        stateDic[CurrentState].OnExit();
        CurrentState = nextState;
        stateDic[CurrentState].OnEnter();
    }


    private void Update()
    {
        SetGroundYPosition();
        //プレイヤーが範囲内にいない場合ふわふわさせる
        if (!playerIsInRange)
        {
            var v = transform.position;
            v.y += Fluffy();
            transform.position = v;
        }
    }

    private void SetGroundYPosition()
    {
        //常にワールド座標上の下方向に向けてRayを生成する
        if (Physics.SphereCast(transform.position, dragonSphereRadius, -Vector3.up, out RaycastHit raycastHit
                , Mathf.Infinity, groundLayerMask))
        {
            groundYPosition = raycastHit.point.y;
        }
    }

    public float WorldYPosition(float ratio)
    {
        return groundYPosition + Mathf.Lerp(groundDistanceMin, groundDistanceMax, ratio);
    }

    /// <summary>
    /// ふわふわ上下に揺れる計算用の関数
    /// </summary>
    /// <returns></returns>
    public float Fluffy()
    {
        return Mathf.Sin((Time.time * fluffySpeed) + fluffyTimeOffset) / fluffyRange;
    }
}
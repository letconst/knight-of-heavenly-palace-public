using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    [SerializeField]
    private EffectDataBase _effectDataBase;
    //EffectTypeの項目数プールを作成
    private List<ParticlePlayerPool> _ParticlePool = new List<ParticlePlayerPool>();

    //エフェクトの親オブジェクトのリスト
    private List<Transform> _ParentTransform = new List<Transform>();

    private void Start()
    {
        //poolの生成
        foreach (var effectData in _effectDataBase.EffectDataList)
        {
            var parentObj = new GameObject(effectData.EffectType.ToString());
            _ParentTransform.Add(parentObj.transform);

            _ParticlePool.Add(new ParticlePlayerPool(_ParentTransform[(int)effectData.EffectType], effectData.ParticlePlayer,effectData.EffectType));
        }
        //シーンをアロード時にpoolを削除する処理
        
        this.OnDestroyAsObservable().Subscribe(_ => { _ParticlePool.ForEach(pool =>
        {
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            pool.Dispose();
        }); }).AddTo(this);
    }

    /// <summary>
    /// エフェクトの再生
    /// </summary>
    /// <param name="_effectType"></param>
    /// <param name="_position">ワールド座標</param>
    /// <param name="_rotation">ワールド回転</param>
    public void EffectPlay(EffectType _effectType, Vector3 _position, Quaternion _rotation)
    {
        var p = _ParticlePool[(int)_effectType].Rent();
        p.PlayEffect(_position, _rotation);
    }

    /// <summary>
    /// エフェクトの再生
    /// ループ時は再生時間を指定する必要がある
    /// </summary>
    /// <param name="_effectType"></param>
    /// <param name="_position">ワールド座標</param>
    /// <param name="_rotation">ワールド回転</param>
    /// <param name="isLoop"></param>
    /// <param name="duratoin"></param>
    public void EffectPlay(EffectType _effectType, Vector3 _position, Quaternion _rotation, bool isLoop, float duration)
    {
        var p = _ParticlePool[(int)_effectType].Rent();
        if (isLoop)
        {
            //ループエフェクトの処理
            p.PlayEffect(_position, _rotation, duration).Forget();
        }
        else
        {
            p.PlayEffect(_position, _rotation);
        }
    }

    
    /// <summary>
    /// エフェクトの再生
    /// 親オブジェクトを指定するときはローカルのpositionとrotationを更新する
    /// </summary>
    /// <param name="_effectType"></param>
    /// <param name="_position">ローカル座標</param>
    /// <param name="_rotation">ローカル回転</param>
    /// <param name="parent">変更先の親オブジェクト</param>

    public void EffectPlay(EffectType _effectType, Vector3 _position, Quaternion _rotation,Transform parent)
    {
        var p = _ParticlePool[(int)_effectType].Rent();
        p.PlayEffect(_position, _rotation,parent);
    }

    /// <summary>
    /// エフェクトの再生
    /// ループ時は再生時間を指定する必要がある
    /// 親オブジェクトを指定するときはローカルのpositionとrotationを更新する
    /// </summary>
    /// <param name="_effectType"></param>
    /// <param name="_position">ローカル座標</param>
    /// <param name="_rotation">ローカル回転</param>
    /// <param name="parent">変更先の親オブジェクト</param>
    /// <param name="isLoop"></param>
    /// <param name="duratoin"></param>
    public void EffectPlay(EffectType _effectType, Vector3 _position, Quaternion _rotation,Transform parent, bool isLoop, float duration)
    {
        var p = _ParticlePool[(int)_effectType].Rent();
        if (isLoop)
        {
            //ループエフェクトの処理
            p.PlayEffect(_position, _rotation,parent, duration).Forget();
        }
        else
        {
            p.PlayEffect(_position, _rotation,parent);
        }
    }
    
    /// <summary>
    /// 再生が終了したエフェクトをプールに戻す関数
    /// </summary>
    /// <param name="effectType"></param>
    /// <param name="particlePlayer"></param>
    public void ReturnPool(EffectType effectType,ParticlePlayer particlePlayer)
    {
        //パーティクルの親オブジェクトの変更
        particlePlayer.transform.parent = _ParentTransform[(int)effectType];
        _ParticlePool[(int)effectType].Return(particlePlayer);
    }
}
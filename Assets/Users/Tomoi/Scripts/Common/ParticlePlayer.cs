using Cysharp.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlayer : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private bool _isEffectLoop = false;
    private EffectType _effectType;

    private ParticleSystem Particle =>
        //遅延初期化に変更
        _particleSystem ? _particleSystem : (_particleSystem = GetComponent<ParticleSystem>());

    /// <summary>
    /// オブジェクトの座標の変更
    /// </summary>
    private void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// オブジェクトの回転の変更
    /// </summary>
    private void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    /// <summary>
    /// オブジェクトのローカル座標の変更
    /// </summary>
    private void SetlocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    /// <summary>
    /// オブジェクトのローカル回転の変更
    /// </summary>
    private void SetlocalRotation(Quaternion rotation)
    {
        transform.localRotation = rotation;
    }
    public void SetEffectType(EffectType effectType)
    {
        _effectType = effectType;
    }

    /// <summary>
    /// ループエフェクトを再生する
    /// </summary>
    /// <param name="position">再生する座標</param>
    /// <param name="rotation">再生する際の回転</param>
    /// <param name="time">再生時間 入力なしの場合1ループして削除</param>
    /// <returns>再生終了通知</returns>
    public void PlayEffect(Vector3 position, Quaternion rotation)
    {
        SetPosition(position);
        SetRotation(rotation);
        _isEffectLoop = false;

        var main = Particle.main;
        main.loop = _isEffectLoop;
        
        //エフェクトを再生
        Particle.Play();
    }
    
    /// <summary>
    /// エフェクトを再生する
    /// </summary>
    /// <param name="position">再生するグローバル座標</param>
    /// <param name="rotation">再生する際の回転</param>
    /// <param name="time">再生時間 入力なしの場合1ループして削除</param>
    /// <returns>再生終了通知</returns>
    public async UniTaskVoid PlayEffect(Vector3 position, Quaternion rotation, float time)
    {
        SetlocalPosition(position);
        SetlocalRotation(rotation);
        _isEffectLoop = true;

        var main = Particle.main;
        main.loop = _isEffectLoop;
        //エフェクトを再生
        Particle.Play();

        //time秒後にエフェクトを止める
        await UniTask.Delay((int)(time * 1000)); 
        Particle.Stop();
    }
    
       
    /// <summary>
    /// ループエフェクトを再生する
    /// </summary>
    /// <param name="position">再生するローカル座標</param>
    /// <param name="rotation">再生する際の回転</param>
    /// <param name="parent">変更先の親オブジェクト</param>
    /// <param name="time">再生時間 入力なしの場合1ループして削除</param>
    /// <returns>再生終了通知</returns>
    public async UniTaskVoid PlayEffect(Vector3 position, Quaternion rotation,Transform parent, float time)
    {
        transform.parent = parent;
        SetlocalPosition(position);
        SetlocalRotation(rotation);
        _isEffectLoop = true;

        var main = Particle.main;
        main.loop = _isEffectLoop;
        //エフェクトを再生
        Particle.Play();

        //time秒後にエフェクトを止める
        await UniTask.Delay((int)(time * 1000)); 
        Particle.Stop();
    }
    #region 親オブジェクトの変更バージョン

    /// <summary>
    /// エフェクトを再生する
    /// </summary>
    /// <param name="position">再生する座標</param>
    /// <param name="rotation">再生する際の回転</param>
    /// <param name="time">再生時間 入力なしの場合1ループして削除</param>
    /// <param name="parent">変更先の親オブジェクト</param>
    /// <returns>再生終了通知</returns>
    public void PlayEffect(Vector3 position, Quaternion rotation,Transform parent)
    {
        transform.parent = parent;
        SetPosition(position);
        SetRotation(rotation);
        _isEffectLoop = false;

        var main = Particle.main;
        main.loop = _isEffectLoop;
        
        //エフェクトを再生
        Particle.Play();
    }
    #endregion
    
    /// <summary>
    /// パーティクルの再生が終わった時に実行される
    /// </summary>
    private void OnParticleSystemStopped(){
        //パーティクルをプールに返す
        EffectManager.Instance.ReturnPool(_effectType,this);
    }
}
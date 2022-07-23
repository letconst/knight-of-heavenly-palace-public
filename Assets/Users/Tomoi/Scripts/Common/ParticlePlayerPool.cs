using UniRx.Toolkit;
using UnityEngine;

public class ParticlePlayerPool : ObjectPool<ParticlePlayer>
{

    private readonly ParticlePlayer _particlePlayer;
    private readonly Transform _parenTransform;
    private readonly EffectType _effectType;

    //コンストラクタ
    public ParticlePlayerPool(Transform parenTransform, ParticlePlayer particle,EffectType effectType)
    {
        _parenTransform = parenTransform;
        _particlePlayer = particle;
        _effectType = effectType;
    }

    /// <summary>
    /// オブジェクトの追加生成時に実行される
    /// </summary>
    protected override ParticlePlayer CreateInstance()
    {
        //足りないプレファブの生成
        //ヒエラルキーが散らからないように一箇所にまとめる
        var p = Object.Instantiate(_particlePlayer, _parenTransform, true);
        p.SetEffectType(_effectType);
        return p;
    }
}

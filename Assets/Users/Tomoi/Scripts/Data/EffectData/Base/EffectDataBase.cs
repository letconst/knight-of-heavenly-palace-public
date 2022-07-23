using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "EffectDataBase/CreateEffectDataAsset")]
public class EffectDataBase : ScriptableObject
{
    [SerializeField] 
    public List<EffectData> EffectDataList = new List<EffectData>();
}

[System.Serializable]
public class EffectData
{
    [SerializeField, Header("Particleを生成するときのenumの名前")]
    public string EffectTypeString;

    [SerializeField, Header("ParticleのParticlePlayer")]
    public ParticlePlayer ParticlePlayer;

    [SerializeField , Header("enum生成時に自動でセットされる")]
    public EffectType EffectType;
}
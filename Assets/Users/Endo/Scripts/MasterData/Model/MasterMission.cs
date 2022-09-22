using UnityEngine;

[CreateAssetMenu(fileName = "NewMissionData", menuName = "マスターデータ/依頼")]
public sealed class MasterMission : MasterDataBaseWithId
{
    [SerializeField, Header(MasterDataModelConstants.Mission.MissionNameLabel)]
    private string missionName;

    /// <summary>ミッション名</summary>
    public string MissionName => missionName;

    [SerializeField, Header(MasterDataModelConstants.Mission.InitGeneratePrefabLabel)]
    private GameObject initGeneratePrefab;

    /// <summary>初期生成するプレハブ</summary>
    public GameObject InitGeneratePrefab => initGeneratePrefab;

    [SerializeField, Header(MasterDataModelConstants.Mission.ContentImage)]
    private Sprite contentImage;

    /// <summary>選択時に表示する内容の画像</summary>
    public Sprite ContentImage => contentImage;
}

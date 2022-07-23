using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMissionData", menuName = "マスターデータ/依頼")]
public class MasterMission : MasterDataBaseWithId
{
    [SerializeField]
    private MissionData[] missionDatas;

    /// <summary>ミッションデータ</summary>
    public IReadOnlyList<MissionData> MissionDatas => missionDatas;

    [System.Serializable]
    public sealed class MissionData
    {
        public string missionName;
    }
}

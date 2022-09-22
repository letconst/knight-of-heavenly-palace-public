using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneDataPack
{
    public abstract GameScene PreviousGameScene { get; protected set; }
}

public class ToResultSceneDataPack : SceneDataPack
{
    public override GameScene PreviousGameScene { get; protected set; }

    public float time;

    public MasterMission AcceptedMission { get; private set; }

    public ToResultSceneDataPack(GameScene gameScene, float time, MasterMission acceptedMission)
    {
        this.time = time;
        AcceptedMission = acceptedMission;
        PreviousGameScene = gameScene;
    }
}

public class ToMainGameSceneDataPack : SceneDataPack
{
    public override GameScene PreviousGameScene { get; protected set; }

    /// <summary>受けた依頼の情報</summary>
    public MasterMission MissionData { get; private set; }

    public ToMainGameSceneDataPack(GameScene prevScene, MasterMission missionData)
    {
        PreviousGameScene = prevScene;
        MissionData       = missionData;
    }
}

public class ToLoadingSceneDataPack : SceneDataPack
{
    public override GameScene PreviousGameScene { get; protected set ; }
    public GameScene NextScene
    {
        get;private set;
    }
    public SceneDataPack NextSceneDataPack
    {
        get; private set;
    }
    public ToLoadingSceneDataPack(GameScene PrevScene,GameScene NextScene,SceneDataPack NextSceneDataPack)
    {
        PreviousGameScene = PrevScene;
        this.NextScene = NextScene;
        this.NextSceneDataPack = NextSceneDataPack;
    }

}

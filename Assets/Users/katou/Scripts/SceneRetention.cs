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
    public ToResultSceneDataPack(GameScene gameScene,float time )
    {
        this.time = time;
        PreviousGameScene = gameScene;
    }
}

public class ToMainGameSceneDataPack : SceneDataPack
{
    public override GameScene PreviousGameScene { get; protected set; }

    /// <summary>受けた依頼の情報</summary>
    public Mission MissionData { get; private set; }

    public ToMainGameSceneDataPack(GameScene prevScene, Mission missionData)
    {
        PreviousGameScene = prevScene;
        MissionData       = missionData;
    }
}

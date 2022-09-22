using System;

public class Monster_02_StateController : StateControllerBase
{
    public enum StateType
    {
        Wait = 0, // Idle
        Move = 1, // 移動
        Interval = 2, // 少し待つとき用のステート
        Null = 3 // Null
    }
    public override void Initialize(int initializeStateType)
    {
        try
        {
            // 待機
            stateDic[(int)StateType.Wait] = gameObject.GetComponent<Monster_02_StateChild_Wait>();
            stateDic[(int)StateType.Wait].Initialize((int)StateType.Wait);
        }
        catch (NullReferenceException e)
        {
            // 待機
            stateDic[(int)StateType.Wait] = gameObject.AddComponent<Monster_02_StateChild_Wait>();
            stateDic[(int)StateType.Wait].Initialize((int)StateType.Wait);
        }
        try
        {
            // 移動
            stateDic[(int)StateType.Move] = gameObject.GetComponent<Monster_02_StateChild_Move>();
            stateDic[(int)StateType.Move].Initialize((int)StateType.Move);
        }
        catch (NullReferenceException e)
        {
            // 移動
            stateDic[(int)StateType.Move] = gameObject.AddComponent<Monster_02_StateChild_Move>();
            stateDic[(int)StateType.Move].Initialize((int)StateType.Move);
        }
        try
        {
            // 少し待つとき用のステート
            stateDic[(int)StateType.Interval] = gameObject.GetComponent<Monster_02_StateChild_Interval>();
            stateDic[(int)StateType.Interval].Initialize((int)StateType.Interval);
        }
        catch (NullReferenceException e)
        {
            // 少し待つとき用のステート
            stateDic[(int)StateType.Interval] = gameObject.AddComponent<Monster_02_StateChild_Interval>();
            stateDic[(int)StateType.Interval].Initialize((int)StateType.Interval);
        }
        try
        {
            // Null
            stateDic[(int)StateType.Null] = gameObject.GetComponent<Monster_02_StateChild_Null>();
            stateDic[(int)StateType.Null].Initialize((int)StateType.Null);
        }
        catch (NullReferenceException e)
        {
            // Null
            stateDic[(int)StateType.Null] = gameObject.AddComponent<Monster_02_StateChild_Null>();
            stateDic[(int)StateType.Null].Initialize((int)StateType.Null);
        }
        
        CurrentState = initializeStateType;
        stateDic[CurrentState].OnEnter();
    }

}

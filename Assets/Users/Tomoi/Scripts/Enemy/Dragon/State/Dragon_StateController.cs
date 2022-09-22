using System;

public class Dragon_StateController : StateControllerBase
{
    public enum StateType
    {
        Wait = 0, // Idle
        Move = 1, // 移動
        AttackMove = 2, // 攻撃のための移動
        AttackBreath = 3, // 炎の攻撃
        Interval = 4, // 少し待つとき用のステート
        Discover = 5, // プレイヤーを発見
        Lost = 6, // プレイヤーを見失う
        Null = 7 // Null
    }

    // 初期化処理
    public override void Initialize(int initializeStateType)
    {
        try
        {
            // 待機
            stateDic[(int)StateType.Wait] = gameObject.GetComponent<Dragon_StateChild_Wait>();
            stateDic[(int)StateType.Wait].Initialize((int)StateType.Wait);
        }
        catch (NullReferenceException e)
        {
            // 待機
            stateDic[(int)StateType.Wait] = gameObject.AddComponent<Dragon_StateChild_Wait>();
            stateDic[(int)StateType.Wait].Initialize((int)StateType.Wait);
        }

        try
        {
            // 移動
            stateDic[(int)StateType.Move] = gameObject.GetComponent<Dragon_StateChild_Move>();
            stateDic[(int)StateType.Move].Initialize((int)StateType.Move);
        }
        catch (NullReferenceException e)
        {
            // 移動
            stateDic[(int)StateType.Move] = gameObject.AddComponent<Dragon_StateChild_Move>();
            stateDic[(int)StateType.Move].Initialize((int)StateType.Move);
        }

        try
        {
            // 攻撃のための移動
            stateDic[(int)StateType.AttackMove] = gameObject.GetComponent<Dragon_StateChild_AttackMove>();
            stateDic[(int)StateType.AttackMove].Initialize((int)StateType.AttackMove);
        }
        catch (NullReferenceException e)
        {
            // 攻撃のための移動
            stateDic[(int)StateType.AttackMove] = gameObject.AddComponent<Dragon_StateChild_AttackMove>();
            stateDic[(int)StateType.AttackMove].Initialize((int)StateType.AttackMove);
        }

        try
        {
            // 炎の攻撃
            stateDic[(int)StateType.AttackBreath] = gameObject.GetComponent<Dragon_StateChild_AttackBreath>();
            stateDic[(int)StateType.AttackBreath].Initialize((int)StateType.AttackBreath);
        }
        catch (NullReferenceException e)
        {
            // 炎の攻撃
            stateDic[(int)StateType.AttackMove] = gameObject.AddComponent<Dragon_StateChild_AttackBreath>();
            stateDic[(int)StateType.AttackMove].Initialize((int)StateType.AttackMove);
        }

        try
        {
            // 少し待つとき用のステート
            stateDic[(int)StateType.Interval] = gameObject.GetComponent<Dragon_StateChild_Interval>();
            stateDic[(int)StateType.Interval].Initialize((int)StateType.Interval);
        }
        catch (NullReferenceException e)
        {
            // 少し待つとき用のステート
            stateDic[(int)StateType.Interval] = gameObject.AddComponent<Dragon_StateChild_Interval>();
            stateDic[(int)StateType.Interval].Initialize((int)StateType.Interval);
        }

        try
        {
            // プレイヤーを発見
            stateDic[(int)StateType.Discover] = gameObject.GetComponent<Dragon_StateChild_Discover>();
            stateDic[(int)StateType.Discover].Initialize((int)StateType.Discover);
        }
        catch (NullReferenceException e)
        {
            stateDic[(int)StateType.Discover] = gameObject.AddComponent<Dragon_StateChild_Discover>();
            stateDic[(int)StateType.Discover].Initialize((int)StateType.Discover);
        }

        try
        {
            // プレイヤーを見失う
            stateDic[(int)StateType.Lost] = gameObject.GetComponent<Dragon_StateChild_Lost>();
            stateDic[(int)StateType.Lost].Initialize((int)StateType.Lost);
        }
        catch (NullReferenceException e)
        {
            // プレイヤーを見失う
            stateDic[(int)StateType.Lost] = gameObject.AddComponent<Dragon_StateChild_Lost>();
            stateDic[(int)StateType.Lost].Initialize((int)StateType.Lost);
        }

        try
        {
            // Null
            stateDic[(int)StateType.Null] = gameObject.GetComponent<Dragon_StateChild_Null>();
            stateDic[(int)StateType.Null].Initialize((int)StateType.Null);
        }
        catch (NullReferenceException e)
        {
            // Null
            stateDic[(int)StateType.Null] = gameObject.AddComponent<Dragon_StateChild_Null>();
            stateDic[(int)StateType.Null].Initialize((int)StateType.Null);
        }


        CurrentState = initializeStateType;
        stateDic[CurrentState].OnEnter();
    }
}
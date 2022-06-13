namespace Endo.Sample.EventSystem
{
    // ① 発生させるイベント用のクラスを用意する
    // EventMessage<T>を継承させるが、イベント時に渡したい値がある場合はジェネリックを増やし、
    // EventSystem<T, P1>の形にする。これは3個まで追加で指定できる
    public sealed class OnEndPlayerMove : EventMessage<OnEndPlayerMove>
    {
    }

    public sealed class OnPlayerDamagedSample : EventMessage<OnPlayerDamagedSample, float>
    {
        // 渡したい値がある場合、プロパティとしてわかり易い名前で公開してあげる
        // 渡す変数は、ジェネリックで指定した順にparam1, param2, param3として格納される
        public float DamageValue => param1;
    }
}

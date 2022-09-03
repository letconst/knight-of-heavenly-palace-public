public interface IAsyncInitialize
{
    /// <summary>
    /// 非同期初期化処理の完了を待機する
    /// </summary>
    /// <returns></returns>
    Cysharp.Threading.Tasks.UniTask WaitForInitialize();
}

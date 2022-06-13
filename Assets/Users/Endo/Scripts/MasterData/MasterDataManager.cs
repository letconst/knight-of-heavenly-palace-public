using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public sealed class MasterDataManager : SingletonMonoBehaviour<MasterDataManager>
{
    private readonly List<AsyncOperationHandle> _dataHandles = new();

    /// <summary>
    /// 指定IDのマスターデータを非同期で取得する
    /// </summary>
    /// <param name="id">マスターデータID</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>マスターデータ</returns>
    public UniTask<T> GetMasterDataAsync<T>(string id) where T : MasterDataBaseWithId
    {
        // TODO: IDの存在確認
        return InternalGetMasterDataAsync<T>($"MasterData/{id}");
    }

    /// <summary>
    /// プレイヤーのマスターデータを非同期で取得する
    /// </summary>
    /// <returns>プレイヤーのマスターデータ</returns>
    public UniTask<MasterPlayer> GetPlayerMasterDataAsync()
    {
        return InternalGetMasterDataAsync<MasterPlayer>("MasterData/Player");
    }

    private UniTask<T> InternalGetMasterDataAsync<T>(string key) where T : MasterDataBase
    {
        AsyncOperationHandle<T> op = Addressables.LoadAssetAsync<T>(key);

        _dataHandles.Add(op);

        return op.Task.AsUniTask();
    }

    private void OnDestroy()
    {
        foreach (AsyncOperationHandle handle in _dataHandles)
        {
            Addressables.Release(handle);
        }

        _dataHandles.Clear();
    }
}

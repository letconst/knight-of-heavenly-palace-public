using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AsyncOperationHandleExtension
{
    public static Task<T> NotNullTask<T>(this AsyncOperationHandle<T> handle)
        => handle.Task ?? Task.FromResult(handle.Result);
}

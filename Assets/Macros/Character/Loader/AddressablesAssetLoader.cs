using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesAssetLoader : IAssetLoader, IDisposable
{
    //LoadAssetAsyncによってロードされたアセット
    private readonly Dictionary<string, AsyncOperationHandle> assetHandleDictionary = new Dictionary<string, AsyncOperationHandle>();
    //LoadAllAssetsAsyncによってロードされたアセット
    private readonly Dictionary<string, AsyncOperationHandle> assetsHandleInGroupDictionary = new Dictionary<string, AsyncOperationHandle>();
    //Dispose済みかどうか
    private bool isDisposed = false;

    //アセットを一つロード
    public async UniTask<T> LoadAssetAsync<T>(string assetPath, CancellationToken cancellationToken) where T : UnityEngine.Object
    {
        //Dispose済みかチェック
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(AddressablesAssetLoader));
        }
        //ロード済みのアセットかチェック
        if (assetHandleDictionary.ContainsKey(assetPath))
        {
            throw new ArgumentException($"Asset '{assetPath}' exists already. Unload it before the new one is loaded.");
        }
        //ロード
        var handle = Addressables.LoadAssetAsync<T>(assetPath);
        //登録
        assetHandleDictionary.Add(assetPath, handle);

        return await handle.ToUniTask(cancellationToken: cancellationToken);
    }

    //グループ内のアセットをすべてロード
    public async UniTask<IEnumerable<T>> LoadAllAssetsAsync<T>(string groupPath, CancellationToken cancellationToken) where T : UnityEngine.Object
    {
        //Dispose済みかチェック
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(AddressablesAssetLoader));
        }
        //ロード済みのアセットかチェック
        if (assetsHandleInGroupDictionary.ContainsKey(groupPath))
        {
            throw new ArgumentException($"Group '{groupPath}' exists already. Unload it before the new one is loaded");
        }
        //ロード
        var handle = Addressables.LoadAssetsAsync<T>(groupPath, null);
        //登録
        assetsHandleInGroupDictionary.Add(groupPath, handle);

        return await handle.ToUniTask(cancellationToken: cancellationToken);
    }

    //アセットを一つアンロード
    public void UnloadAsset(string assetPath)
    {
        //Dispose済みかチェック
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(AddressablesAssetLoader));
        }
        //存在するアセットかチェック
        if (!assetHandleDictionary.ContainsKey(assetPath))
        {
            throw new ArgumentException($"Asset '{assetPath}' does not exist.");
        }
        //アンロード
        Addressables.Release(assetHandleDictionary[assetPath]);
        //Dictionaryから抹消
        assetHandleDictionary.Remove(assetPath);
    }

    //グループ内のアセットを全てアンロード
    public void UnloadAllAssets(string groupPath)
    {
        //Dispose済みかチェック
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(AddressablesAssetLoader));
        }
        //存在するグループかチェック
        if (!assetsHandleInGroupDictionary.ContainsKey(groupPath))
        {
            throw new ArgumentException($"Group '{groupPath}' does not exist.");
        }
        //アンロード
        Addressables.Release(assetsHandleInGroupDictionary[groupPath]);
        //Dictionaryから抹消
        assetsHandleInGroupDictionary.Remove(groupPath);
    }

    //全てのリソースを解放
    public void Dispose()
    {
        if (!isDisposed)
        {
            foreach (var handle in assetHandleDictionary.Values)
            {
                Addressables.Release(handle);
            }
            foreach (var handle in assetsHandleInGroupDictionary.Values)
            {
                Addressables.Release(handle);
            }
            assetHandleDictionary.Clear();
            assetsHandleInGroupDictionary.Clear();
            isDisposed = true;
        }
    }
}
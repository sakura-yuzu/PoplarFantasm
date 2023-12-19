using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public interface IAssetLoader
{
    //一つのアセットをロード
    UniTask<T> LoadAssetAsync<T>(string assetPath, CancellationToken cancellationToken) where T : UnityEngine.Object;
    //グループ内の複数のアセットをロード
    UniTask<IEnumerable<T>> LoadAllAssetsAsync<T>(string groupPath, CancellationToken cancellationToken) where T : UnityEngine.Object;
    //一つのアセットをアンロード
    void UnloadAsset(string assetPath);
    //グループ内の複数のアセットをアンロード
    void UnloadAllAssets(string groupPath);
}

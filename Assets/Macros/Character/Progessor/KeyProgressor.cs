using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;
using UnityEngine;

public class KeyProgressor : INextProgressor, ICancellationProgressor
{
    private readonly KeyCode[] keyCodes;

    public KeyProgressor(Settings settings)
    {
        this.keyCodes = settings.KeyCodes ?? throw new ArgumentNullException(nameof(settings.KeyCodes));
    }

    public UniTask NotifyNextAsync(CancellationToken cancellationToken)
    {
        return AwaitAnyKeyDownAsync(cancellationToken);
    }

    public UniTask NotifyCancellationAsync(CancellationToken cancellationToken)
    {
        return AwaitAnyKeyDownAsync(cancellationToken);
    }

    private UniTask AwaitAnyKeyDownAsync(CancellationToken cancellationToken)
    {
        //キーのどれかが押されたら完了
        return keyCodes.Length > 0 ? UniTaskAsyncEnumerable.EveryUpdate()
            .SelectMany(_ => keyCodes.ToUniTaskAsyncEnumerable().Select(keyCode => Input.GetKeyDown(keyCode)))
            .Where(x => x)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) :
            UniTask.Never(cancellationToken);
    }

    [Serializable]
    public class Settings
    {
        public KeyCode[] KeyCodes;
    }
}
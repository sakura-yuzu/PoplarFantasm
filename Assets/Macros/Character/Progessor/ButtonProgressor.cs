using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;
using UnityEngine.UI;

public class ButtonProgressor : INextProgressor, ICancellationProgressor
{
    private readonly Button[] buttons;

    public ButtonProgressor(Settings settings)
    {
        this.buttons = settings.Buttons ?? throw new ArgumentNullException(nameof(settings.Buttons));
    }

    public UniTask NotifyNextAsync(CancellationToken cancellationToken)
    {
        return AwaitAnyButtonClikedAsync(cancellationToken);
    }

    public UniTask NotifyCancellationAsync(CancellationToken cancellationToken)
    {
        return AwaitAnyButtonClikedAsync(cancellationToken);
    }

    private UniTask AwaitAnyButtonClikedAsync(CancellationToken cancellationToken)
    {
        //ボタンのどれかが押されたら完了
        return buttons.Length > 0 ? buttons.ToUniTaskAsyncEnumerable()
            .SelectMany(button => button.OnClickAsAsyncEnumerable())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) :
            UniTask.Never(cancellationToken);
    }

    [Serializable]
    public class Settings
    {
        public Button[] Buttons;
    }
}
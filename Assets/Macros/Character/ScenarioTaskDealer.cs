using Cysharp.Threading.Tasks;
using ScenarioFlow;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;

[ScenarioMethod("task")]
public class ScenarioTaskDealer : IReflectable
{
    //ScenarioTaskを消化する
    private readonly IScenarioTaskAccepter scenarioTaskAccepter;
    //ScenarioTaskをキャンセルする
    private readonly ICancellationTokenCanceler cancellationTokenCanceler;

    public ScenarioTaskDealer(IScenarioTaskAccepter scenarioTaskAccepter, ICancellationTokenCanceler cancellationTokenCanceler)
    {
        this.scenarioTaskAccepter = scenarioTaskAccepter ?? throw new ArgumentNullException(nameof(scenarioTaskAccepter));

        this.cancellationTokenCanceler = cancellationTokenCanceler ?? throw new ArgumentNullException(nameof(cancellationTokenCanceler));
    }

    [ScenarioMethod("accept", "ScenarioTaskの終了を待機\nAwait the ScenarioTask completed.")]
    public async UniTask AcceptScenarioTaskAsync(string taskName, CancellationToken cancellationToken)
    {
        try
        {
            //ScenarioTaskの終了もしくはキャンセルを待機
            await UniTask.WhenAny(
                scenarioTaskAccepter.AcceptAsync(taskName),
                cancellationToken.ToUniTask().Item1);
        }
        finally
        {
            //ScenarioTaskの終了を保証
            cancellationTokenCanceler.Cancel(taskName);
        }
    }

    [ScenarioMethod("cancel", "ScenarioTaskをキャンセル\nCancel the ScenarioTask.")]
    public void CancelScenarioTaskAsync(string taskName)
    {
        //ScenarioTaskを消化
        scenarioTaskAccepter.AcceptAsync(taskName).Forget();
        //即座にキャンセル
        cancellationTokenCanceler.Cancel(taskName);
    }
}
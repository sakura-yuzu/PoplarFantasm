using Cysharp.Threading.Tasks;
using ScenarioFlow;
using System;
using System.Threading;
using UnityEngine;

[ScenarioMethod("delay")]
public class DelayGenerator : IReflectable
{
    [ScenarioMethod("sec", "指定の秒数待機\nWait for the seconds.")]
    public async UniTask DelaySeconds(float seconds, CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: cancellationToken);
    }

    [ScenarioMethod("milli", "指定のミリ秒待機\nWait for the milliseconds.")]
    public async UniTask DelayMilliseconds(float milliSeconds, CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(milliSeconds), cancellationToken: cancellationToken);
    }
}
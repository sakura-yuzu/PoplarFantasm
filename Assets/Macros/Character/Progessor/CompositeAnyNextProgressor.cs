using Cysharp.Threading.Tasks;
using ScenarioFlow.TaskFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class CompositeAnyNextProgressor : INextProgressor
{
    private readonly INextProgressor[] nextProgressors;

    public CompositeAnyNextProgressor(IEnumerable<INextProgressor> nextProgressors)
    {
        if (nextProgressors == null)
            throw new ArgumentNullException(nameof(nextProgressors));

        this.nextProgressors = nextProgressors.ToArray();
    }

    public UniTask NotifyNextAsync(CancellationToken cancellationToken)
    {
        return nextProgressors.Length > 0 ?
            UniTask.WhenAny(nextProgressors.Select(nextProgressor => nextProgressor.NotifyNextAsync(cancellationToken))) :
            UniTask.Never(cancellationToken);
    }
}
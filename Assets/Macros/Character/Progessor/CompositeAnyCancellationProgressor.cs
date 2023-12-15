using Cysharp.Threading.Tasks;
using ScenarioFlow.TaskFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class CompositeAnyCancellationProgressor : ICancellationProgressor
{
    private readonly ICancellationProgressor[] cancellationProgressors;

    public CompositeAnyCancellationProgressor(IEnumerable<ICancellationProgressor> cancellationProgressors)
    {
        if (cancellationProgressors == null)
            throw new ArgumentNullException(nameof(cancellationProgressors));

        this.cancellationProgressors = cancellationProgressors.ToArray();
    }

    public UniTask NotifyCancellationAsync(CancellationToken cancellationToken)
    {
        return cancellationProgressors.Length > 0 ?
            UniTask.WhenAny(cancellationProgressors.Select(cancellationProgressor => cancellationProgressor.NotifyCancellationAsync(cancellationToken))) :
            UniTask.Never(cancellationToken);
    }
}
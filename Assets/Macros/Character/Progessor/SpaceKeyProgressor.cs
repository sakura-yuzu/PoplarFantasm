using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using ScenarioFlow.TaskFlow;
using System.Threading;
using UnityEngine;

public class SpaceKeyProgressor : INextProgressor, ICancellationProgressor
{
   public UniTask NotifyNextAsync(CancellationToken cancellationToken)
   {
       return AwaitSpaceKeyDownAsync(cancellationToken);
   }

   public UniTask NotifyCancellationAsync(CancellationToken cancellationToken)
   {
       return AwaitSpaceKeyDownAsync(cancellationToken);
   }

   private UniTask AwaitSpaceKeyDownAsync(CancellationToken cancellationToken)
   {
       return UniTaskAsyncEnumerable.EveryUpdate()
           .Select(_ => Input.GetKeyDown(KeyCode.Space))
           .Where(x => x)
           .FirstOrDefaultAsync(cancellationToken: cancellationToken);
   }
}
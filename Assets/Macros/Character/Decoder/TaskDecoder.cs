using ScenarioFlow;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;

public class TaskDecoder : IReflectable
{
   private readonly ICancellationTokenDecoder cancellationTokenDecoder;

   public TaskDecoder(ICancellationTokenDecoder cancellationTokenDecoder)
   {
       this.cancellationTokenDecoder = cancellationTokenDecoder ??
           throw new ArgumentNullException(nameof(cancellationTokenDecoder));
   }

   [Decoder]
   public CancellationToken CancellationTokenDecoder(string source)
   {
       return cancellationTokenDecoder.Decode(source);
   }
}
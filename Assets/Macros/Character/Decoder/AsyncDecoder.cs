using ScenarioFlow;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;

public class AsyncDecoder : IReflectable
{
    private readonly ICancellationTokenDecoder cancellationTokenDecoder;

    public AsyncDecoder(ICancellationTokenDecoder cancellationTokenDecoder)
    {
        this.cancellationTokenDecoder = cancellationTokenDecoder ?? throw new ArgumentNullException(nameof(cancellationTokenDecoder));
    }

    [Decoder]
    public CancellationToken CancellationTokenDecoder(string source)
    {
        return cancellationTokenDecoder.Decode(source);
    }
}
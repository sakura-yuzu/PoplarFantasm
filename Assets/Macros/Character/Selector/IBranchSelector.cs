using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public interface IBranchSelector
{
	public UniTask<int> SelectBranchAsync(IEnumerable<string> choices, CancellationToken cancellationToken);
}
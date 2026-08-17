using System.Threading;
using UnityEngine;

namespace VContainer.Unity
{
	public interface IAsyncStartable
	{
		Awaitable StartAsync(CancellationToken cancellation = default(CancellationToken));
	}
}

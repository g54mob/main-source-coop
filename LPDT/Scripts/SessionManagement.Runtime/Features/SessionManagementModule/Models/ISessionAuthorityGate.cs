using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface ISessionAuthorityGate
	{
		bool IsStepOpen(int step, int epoch);

		UniTask PassAsync(int step, int epoch, Func<UniTask> work, CancellationToken cancellationToken = default(CancellationToken));
	}
}

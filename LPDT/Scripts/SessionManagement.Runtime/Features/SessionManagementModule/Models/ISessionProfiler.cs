using System;

namespace Features.SessionManagementModule.Models
{
	public interface ISessionProfiler
	{
		IAsyncDisposable Sample(SessionPhase phase);
	}
}

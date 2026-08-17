using System;
using VContainer.Diagnostics;

namespace VContainer
{
	public interface IObjectResolver : IDisposable
	{
		object ApplicationOrigin { get; }

		DiagnosticsCollector Diagnostics { get; set; }

		object Resolve(Type type);

		bool TryResolve(Type type, out object resolved);

		object Resolve(Registration registration);

		IScopedObjectResolver CreateScope(Action<IContainerBuilder> installation = null);

		void Inject(object instance);

		bool TryGetRegistration(Type type, out Registration registration);
	}
}

using System;
using VContainer.Diagnostics;

namespace VContainer
{
	public interface IContainerBuilder
	{
		object ApplicationOrigin { get; set; }

		DiagnosticsCollector Diagnostics { get; set; }

		int Count { get; }

		RegistrationBuilder this[int index] { get; set; }

		T Register<T>(T registrationBuilder) where T : RegistrationBuilder;

		void RegisterBuildCallback(Action<IObjectResolver> container);

		bool Exists(Type type, bool includeInterfaceTypes = false, bool findParentScopes = false);
	}
}

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using VContainer.Diagnostics;
using VContainer.Internal;

namespace VContainer
{
	public sealed class Container : IObjectResolver, IDisposable
	{
		private readonly Registry registry;

		private readonly IScopedObjectResolver rootScope;

		private readonly ConcurrentDictionary<Registration, Lazy<object>> sharedInstances = new ConcurrentDictionary<Registration, Lazy<object>>();

		private readonly CompositeDisposable disposables = new CompositeDisposable();

		private readonly Func<Registration, Lazy<object>> createInstance;

		public object ApplicationOrigin { get; }

		public DiagnosticsCollector Diagnostics { get; set; }

		internal Container(Registry registry, object applicationOrigin = null)
		{
			this.registry = registry;
			rootScope = new ScopedContainer(registry, this, null, applicationOrigin);
			createInstance = (Registration registration) => new Lazy<object>(() => registration.SpawnInstance(this));
			ApplicationOrigin = applicationOrigin;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Resolve(Type type)
		{
			if (TryGetRegistration(type, out var registration))
			{
				return Resolve(registration);
			}
			throw new VContainerException(type, $"No such registration of type: {type}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryResolve(Type type, out object resolved)
		{
			if (TryGetRegistration(type, out var registration))
			{
				resolved = Resolve(registration);
				return true;
			}
			resolved = null;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Resolve(Registration registration)
		{
			if (Diagnostics != null)
			{
				return Diagnostics.TraceResolve(registration, ResolveCore);
			}
			return ResolveCore(registration);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IScopedObjectResolver CreateScope(Action<IContainerBuilder> installation = null)
		{
			return rootScope.CreateScope(installation);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Inject(object instance)
		{
			InjectorCache.GetOrBuild(instance.GetType()).Inject(instance, this, null);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryGetRegistration(Type type, out Registration registration)
		{
			return registry.TryGet(type, out registration);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			if (Diagnostics != null)
			{
				Diagnostics.Clear();
			}
			rootScope.Dispose();
			disposables.Dispose();
			sharedInstances.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private object ResolveCore(Registration registration)
		{
			switch (registration.Lifetime)
			{
			case Lifetime.Singleton:
			{
				Lazy<object> orAdd = sharedInstances.GetOrAdd(registration, createInstance);
				if (!orAdd.IsValueCreated && orAdd.Value is IDisposable disposable && !(registration.Provider is ExistingInstanceProvider))
				{
					disposables.Add(disposable);
				}
				return orAdd.Value;
			}
			case Lifetime.Scoped:
				return rootScope.Resolve(registration);
			default:
				return registration.SpawnInstance(this);
			}
		}
	}
}

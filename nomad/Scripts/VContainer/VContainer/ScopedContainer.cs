using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using VContainer.Diagnostics;
using VContainer.Internal;

namespace VContainer
{
	public sealed class ScopedContainer : IScopedObjectResolver, IObjectResolver, IDisposable
	{
		private readonly Registry registry;

		private readonly ConcurrentDictionary<Registration, Lazy<object>> sharedInstances = new ConcurrentDictionary<Registration, Lazy<object>>();

		private readonly CompositeDisposable disposables = new CompositeDisposable();

		private readonly Func<Registration, Lazy<object>> createInstance;

		public IObjectResolver Root { get; }

		public IScopedObjectResolver Parent { get; }

		public object ApplicationOrigin { get; }

		public DiagnosticsCollector Diagnostics { get; set; }

		internal ScopedContainer(Registry registry, IObjectResolver root, IScopedObjectResolver parent = null, object applicationOrigin = null)
		{
			Root = root;
			Parent = parent;
			ApplicationOrigin = applicationOrigin;
			this.registry = registry;
			createInstance = (Registration registration) => new Lazy<object>(() => registration.SpawnInstance(this));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Resolve(Type type)
		{
			if (TryFindRegistration(type, out var registration))
			{
				return Resolve(registration);
			}
			throw new VContainerException(type, $"No such registration of type: {type}");
		}

		public bool TryResolve(Type type, out object resolved)
		{
			if (TryFindRegistration(type, out var registration))
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
			ScopedContainerBuilder scopedContainerBuilder = new ScopedContainerBuilder(Root, this)
			{
				ApplicationOrigin = ApplicationOrigin
			};
			installation?.Invoke(scopedContainerBuilder);
			return scopedContainerBuilder.BuildScope();
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
			disposables.Dispose();
			sharedInstances.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private object ResolveCore(Registration registration)
		{
			switch (registration.Lifetime)
			{
			case Lifetime.Singleton:
				if (Parent == null)
				{
					return Root.Resolve(registration);
				}
				if (!registry.Exists(registration.ImplementationType))
				{
					return Parent.Resolve(registration);
				}
				return CreateTrackedInstance(registration);
			case Lifetime.Scoped:
				return CreateTrackedInstance(registration);
			default:
				return registration.SpawnInstance(this);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private object CreateTrackedInstance(Registration registration)
		{
			Lazy<object> orAdd = sharedInstances.GetOrAdd(registration, createInstance);
			bool isValueCreated = orAdd.IsValueCreated;
			object value = orAdd.Value;
			if (!isValueCreated && value is IDisposable disposable && !(registration.Provider is ExistingInstanceProvider))
			{
				disposables.Add(disposable);
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool TryFindRegistration(Type type, out Registration registration)
		{
			for (IScopedObjectResolver scopedObjectResolver = this; scopedObjectResolver != null; scopedObjectResolver = scopedObjectResolver.Parent)
			{
				if (scopedObjectResolver.TryGetRegistration(type, out registration))
				{
					return true;
				}
			}
			registration = null;
			return false;
		}
	}
}

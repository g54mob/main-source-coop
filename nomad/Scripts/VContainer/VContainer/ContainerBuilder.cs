using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VContainer.Diagnostics;
using VContainer.Internal;

namespace VContainer
{
	public class ContainerBuilder : IContainerBuilder
	{
		private readonly List<RegistrationBuilder> registrationBuilders = new List<RegistrationBuilder>();

		private Action<IObjectResolver> buildCallback;

		private DiagnosticsCollector diagnostics;

		public object ApplicationOrigin { get; set; }

		public int Count => registrationBuilders.Count;

		public RegistrationBuilder this[int index]
		{
			get
			{
				return registrationBuilders[index];
			}
			set
			{
				registrationBuilders[index] = value;
			}
		}

		public DiagnosticsCollector Diagnostics
		{
			get
			{
				return diagnostics;
			}
			set
			{
				diagnostics = value;
				diagnostics?.Clear();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Register<T>(T registrationBuilder) where T : RegistrationBuilder
		{
			registrationBuilders.Add(registrationBuilder);
			Diagnostics?.TraceRegister(new RegisterInfo(registrationBuilder));
			return registrationBuilder;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterBuildCallback(Action<IObjectResolver> callback)
		{
			buildCallback = (Action<IObjectResolver>)Delegate.Combine(buildCallback, callback);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Exists(Type type, bool includeInterfaceTypes = false, bool findParentScopes = false)
		{
			foreach (RegistrationBuilder registrationBuilder in registrationBuilders)
			{
				if (!(registrationBuilder.ImplementationType == type))
				{
					if (!includeInterfaceTypes)
					{
						continue;
					}
					List<Type> interfaceTypes = registrationBuilder.InterfaceTypes;
					if (interfaceTypes == null || !interfaceTypes.Contains(type))
					{
						continue;
					}
				}
				return true;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual IObjectResolver Build()
		{
			Container container = new Container(BuildRegistry(), ApplicationOrigin);
			container.Diagnostics = Diagnostics;
			EmitCallbacks(container);
			return container;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected Registry BuildRegistry()
		{
			Registration[] array = new Registration[registrationBuilders.Count + 1];
			for (int i = 0; i < registrationBuilders.Count; i++)
			{
				RegistrationBuilder registrationBuilder = registrationBuilders[i];
				Registration registration = registrationBuilder.Build();
				Diagnostics?.TraceBuild(registrationBuilder, registration);
				array[i] = registration;
			}
			array[array.Length - 1] = new Registration(typeof(IObjectResolver), Lifetime.Transient, null, ContainerInstanceProvider.Default);
			Registry registry = Registry.Build(array);
			TypeAnalyzer.CheckCircularDependency(array, registry);
			return registry;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void EmitCallbacks(IObjectResolver container)
		{
			buildCallback?.Invoke(container);
			Diagnostics?.NotifyContainerBuilt(container);
		}
	}
}

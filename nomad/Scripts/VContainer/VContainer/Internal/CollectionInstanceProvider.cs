using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
	internal sealed class CollectionInstanceProvider : IInstanceProvider, IEnumerable<Registration>, IEnumerable
	{
		private readonly List<Type> interfaceTypes;

		private readonly List<Registration> registrations = new List<Registration>();

		public Type ImplementationType { get; }

		public IReadOnlyList<Type> InterfaceTypes => interfaceTypes;

		public Lifetime Lifetime => Lifetime.Transient;

		public Type ElementType { get; }

		public static bool Match(Type openGenericType)
		{
			if (!(openGenericType == typeof(IEnumerable<>)))
			{
				return openGenericType == typeof(IReadOnlyList<>);
			}
			return true;
		}

		public List<Registration>.Enumerator GetEnumerator()
		{
			return registrations.GetEnumerator();
		}

		IEnumerator<Registration> IEnumerable<Registration>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public CollectionInstanceProvider(Type elementType)
		{
			ElementType = elementType;
			ImplementationType = elementType.MakeArrayType();
			interfaceTypes = new List<Type>
			{
				RuntimeTypeCache.EnumerableTypeOf(elementType),
				RuntimeTypeCache.ReadOnlyListTypeOf(elementType)
			};
		}

		public override string ToString()
		{
			string arg = ((InterfaceTypes != null) ? string.Join(", ", InterfaceTypes) : "");
			return $"CollectionRegistration {ImplementationType} ContractTypes=[{arg}] {Lifetime}";
		}

		public void Add(Registration registration)
		{
			foreach (Registration registration2 in registrations)
			{
				if (registration2.Lifetime == Lifetime.Singleton && registration2.ImplementationType == registration.ImplementationType)
				{
					throw new VContainerException(registration.ImplementationType, $"Conflict implementation type : {registration}");
				}
			}
			registrations.Add(registration);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object SpawnInstance(IObjectResolver resolver)
		{
			if (resolver is IScopedObjectResolver scope)
			{
				List<RegistrationElement> buffer;
				using (ListPool<RegistrationElement>.Get(out buffer))
				{
					CollectFromParentScopes(scope, buffer);
					return SpawnInstance(resolver, buffer);
				}
			}
			Array array = Array.CreateInstance(ElementType, registrations.Count);
			for (int i = 0; i < registrations.Count; i++)
			{
				array.SetValue(resolver.Resolve(registrations[i]), i);
			}
			return array;
		}

		internal object SpawnInstance(IObjectResolver currentScope, IReadOnlyList<RegistrationElement> entirelyRegistrations)
		{
			Array array = Array.CreateInstance(ElementType, entirelyRegistrations.Count);
			for (int i = 0; i < entirelyRegistrations.Count; i++)
			{
				RegistrationElement registrationElement = entirelyRegistrations[i];
				IObjectResolver objectResolver = ((registrationElement.Registration.Lifetime == Lifetime.Singleton) ? registrationElement.RegisteredContainer : currentScope);
				array.SetValue(objectResolver.Resolve(registrationElement.Registration), i);
			}
			return array;
		}

		internal void CollectFromParentScopes(IScopedObjectResolver scope, List<RegistrationElement> registrationsBuffer, bool localScopeOnly = false)
		{
			foreach (Registration registration2 in registrations)
			{
				registrationsBuffer.Add(new RegistrationElement(registration2, scope));
			}
			Type type = InterfaceTypes[0];
			for (scope = scope.Parent; scope != null; scope = scope.Parent)
			{
				if (scope.TryGetRegistration(type, out var registration) && registration.Provider is CollectionInstanceProvider collectionInstanceProvider)
				{
					foreach (Registration registration3 in collectionInstanceProvider.registrations)
					{
						if (!localScopeOnly || registration3.Lifetime != Lifetime.Singleton)
						{
							registrationsBuffer.Add(new RegistrationElement(registration3, scope));
						}
					}
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace VContainer.Internal
{
	public sealed class Registry
	{
		[ThreadStatic]
		private static IDictionary<Type, Registration> buildBuffer = new Dictionary<Type, Registration>(128);

		private readonly FixedTypeKeyHashtable<Registration> hashTable;

		public static Registry Build(Registration[] registrations)
		{
			if (buildBuffer == null)
			{
				buildBuffer = new Dictionary<Type, Registration>(128);
			}
			buildBuffer.Clear();
			foreach (Registration registration in registrations)
			{
				IReadOnlyList<Type> interfaceTypes = registration.InterfaceTypes;
				if (interfaceTypes != null)
				{
					for (int j = 0; j < interfaceTypes.Count; j++)
					{
						AddToBuildBuffer(buildBuffer, interfaceTypes[j], registration);
					}
					if (!buildBuffer.ContainsKey(registration.ImplementationType))
					{
						buildBuffer.Add(registration.ImplementationType, null);
					}
				}
				else
				{
					AddToBuildBuffer(buildBuffer, registration.ImplementationType, registration);
				}
			}
			return new Registry(new FixedTypeKeyHashtable<Registration>(buildBuffer.ToArray()));
		}

		private static void AddToBuildBuffer(IDictionary<Type, Registration> buf, Type service, Registration registration)
		{
			if (buf.TryGetValue(service, out var value) && value != null)
			{
				CollectionInstanceProvider collectionInstanceProvider2;
				if (buf.TryGetValue(RuntimeTypeCache.EnumerableTypeOf(service), out var value2) && value2.Provider is CollectionInstanceProvider collectionInstanceProvider)
				{
					collectionInstanceProvider2 = collectionInstanceProvider;
				}
				else
				{
					collectionInstanceProvider2 = new CollectionInstanceProvider(service) { value };
					Registration collectionRegistration = new Registration(RuntimeTypeCache.ArrayTypeOf(service), Lifetime.Transient, new List<Type>
					{
						RuntimeTypeCache.EnumerableTypeOf(service),
						RuntimeTypeCache.ReadOnlyListTypeOf(service)
					}, collectionInstanceProvider2);
					AddCollectionToBuildBuffer(buf, collectionRegistration);
				}
				collectionInstanceProvider2.Add(registration);
				buf[service] = registration;
			}
			else
			{
				buf.Add(service, registration);
			}
		}

		private static void AddCollectionToBuildBuffer(IDictionary<Type, Registration> buf, Registration collectionRegistration)
		{
			for (int i = 0; i < collectionRegistration.InterfaceTypes.Count; i++)
			{
				Type type = collectionRegistration.InterfaceTypes[i];
				try
				{
					buf.Add(type, collectionRegistration);
				}
				catch (ArgumentException)
				{
					throw new VContainerException(type, $"Registration with the same key already exists: {collectionRegistration}");
				}
			}
		}

		private Registry(FixedTypeKeyHashtable<Registration> hashTable)
		{
			this.hashTable = hashTable;
		}

		public bool TryGet(Type interfaceType, out Registration registration)
		{
			if (hashTable.TryGet(interfaceType, out registration))
			{
				return registration != null;
			}
			if (interfaceType.IsConstructedGenericType)
			{
				Type openGenericType = RuntimeTypeCache.OpenGenericTypeOf(interfaceType);
				Type[] typeParameters = RuntimeTypeCache.GenericTypeParametersOf(interfaceType);
				if (!TryGetClosedGenericRegistration(interfaceType, openGenericType, typeParameters, out registration) && !TryFallbackToSingleElementCollection(interfaceType, openGenericType, typeParameters, out registration))
				{
					return TryFallbackToContainerLocal(interfaceType, openGenericType, typeParameters, out registration);
				}
				return true;
			}
			return false;
		}

		private bool TryGetClosedGenericRegistration(Type interfaceType, Type openGenericType, Type[] typeParameters, out Registration registration)
		{
			if (hashTable.TryGet(openGenericType, out var value) && value.Provider is OpenGenericInstanceProvider openGenericInstanceProvider)
			{
				registration = openGenericInstanceProvider.GetClosedRegistration(interfaceType, typeParameters);
				return true;
			}
			registration = null;
			return false;
		}

		public bool Exists(Type type)
		{
			if (hashTable.TryGet(type, out var value))
			{
				return true;
			}
			if (type.IsConstructedGenericType)
			{
				type = RuntimeTypeCache.OpenGenericTypeOf(type);
			}
			return hashTable.TryGet(type, out value);
		}

		private bool TryFallbackToContainerLocal(Type closedGenericType, Type openGenericType, IReadOnlyList<Type> typeParameters, out Registration newRegistration)
		{
			if (openGenericType == typeof(ContainerLocal<>))
			{
				Type interfaceType = typeParameters[0];
				if (TryGet(interfaceType, out var registration))
				{
					ContainerLocalInstanceProvider provider = new ContainerLocalInstanceProvider(closedGenericType, registration);
					newRegistration = new Registration(closedGenericType, Lifetime.Scoped, null, provider);
					return true;
				}
			}
			newRegistration = null;
			return false;
		}

		private bool TryFallbackToSingleElementCollection(Type closedGenericType, Type openGenericType, IReadOnlyList<Type> typeParameters, out Registration newRegistration)
		{
			if (CollectionInstanceProvider.Match(openGenericType))
			{
				Type type = typeParameters[0];
				CollectionInstanceProvider collectionInstanceProvider = new CollectionInstanceProvider(type);
				if (hashTable.TryGet(type, out var value) && value != null)
				{
					collectionInstanceProvider.Add(value);
				}
				newRegistration = new Registration(RuntimeTypeCache.ArrayTypeOf(type), Lifetime.Transient, new List<Type>
				{
					RuntimeTypeCache.EnumerableTypeOf(type),
					RuntimeTypeCache.ReadOnlyListTypeOf(type)
				}, collectionInstanceProvider);
				return true;
			}
			newRegistration = null;
			return false;
		}
	}
}

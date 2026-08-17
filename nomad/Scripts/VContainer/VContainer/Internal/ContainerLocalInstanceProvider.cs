using System;
using System.Collections.Generic;

namespace VContainer.Internal
{
	internal sealed class ContainerLocalInstanceProvider : IInstanceProvider
	{
		private readonly Type wrappedType;

		private readonly Registration valueRegistration;

		public ContainerLocalInstanceProvider(Type wrappedType, Registration valueRegistration)
		{
			this.wrappedType = wrappedType;
			this.valueRegistration = valueRegistration;
		}

		public object SpawnInstance(IObjectResolver resolver)
		{
			object obj;
			if (resolver is ScopedContainer scopedContainer && valueRegistration.Provider is CollectionInstanceProvider collectionInstanceProvider)
			{
				List<RegistrationElement> buffer;
				using (ListPool<RegistrationElement>.Get(out buffer))
				{
					collectionInstanceProvider.CollectFromParentScopes(scopedContainer, buffer, localScopeOnly: true);
					obj = collectionInstanceProvider.SpawnInstance(scopedContainer, buffer);
				}
			}
			else
			{
				obj = resolver.Resolve(valueRegistration);
			}
			object[] array = CappedArrayPool<object>.Shared8Limit.Rent(1);
			try
			{
				array[0] = obj;
				return Activator.CreateInstance(wrappedType, array);
			}
			finally
			{
				CappedArrayPool<object>.Shared8Limit.Return(array);
			}
		}
	}
}

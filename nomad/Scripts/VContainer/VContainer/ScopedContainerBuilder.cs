using System;
using System.Runtime.CompilerServices;

namespace VContainer
{
	public sealed class ScopedContainerBuilder : ContainerBuilder
	{
		private readonly IObjectResolver root;

		private readonly IScopedObjectResolver parent;

		internal ScopedContainerBuilder(IObjectResolver root, IScopedObjectResolver parent)
		{
			this.root = root;
			this.parent = parent;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IScopedObjectResolver BuildScope()
		{
			ScopedContainer scopedContainer = new ScopedContainer(BuildRegistry(), root, parent, base.ApplicationOrigin);
			scopedContainer.Diagnostics = base.Diagnostics;
			EmitCallbacks(scopedContainer);
			return scopedContainer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override IObjectResolver Build()
		{
			return BuildScope();
		}

		public override bool Exists(Type type, bool includeInterfaceTypes = false, bool findParentScopes = false)
		{
			if (base.Exists(type, includeInterfaceTypes, findParentScopes))
			{
				return true;
			}
			if (findParentScopes)
			{
				for (IScopedObjectResolver scopedObjectResolver = parent; scopedObjectResolver != null; scopedObjectResolver = scopedObjectResolver.Parent)
				{
					if (scopedObjectResolver.TryGetRegistration(type, out var registration) && (includeInterfaceTypes || registration.ImplementationType == type))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

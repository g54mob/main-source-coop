using System;

namespace VContainer.Unity
{
	public sealed class VContainerParentTypeReferenceNotFound : Exception
	{
		public readonly Type ParentType;

		public VContainerParentTypeReferenceNotFound(Type parentType, string message)
			: base(message)
		{
			ParentType = parentType;
		}
	}
}

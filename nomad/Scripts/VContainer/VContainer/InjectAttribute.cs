using System;
using JetBrains.Annotations;

namespace VContainer
{
	[MeansImplicitUse(ImplicitUseKindFlags.Access | ImplicitUseKindFlags.Assign | ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class InjectAttribute : PreserveAttribute
	{
	}
}

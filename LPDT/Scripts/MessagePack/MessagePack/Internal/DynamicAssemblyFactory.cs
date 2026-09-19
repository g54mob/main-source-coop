using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MessagePack.Internal
{
	internal class DynamicAssemblyFactory
	{
		private readonly string moduleName;

		private DynamicAssembly? singletonAssembly;

		private ImmutableHashSet<AssemblyName> lastCreatedDynamicAssemblySkipVisibilityChecks = SkipClrVisibilityChecks.EmptySet.Add(Assembly.GetExecutingAssembly().GetName());

		public DynamicAssemblyFactory(string moduleName)
		{
			this.moduleName = moduleName;
		}

		[return: NotNullIfNotNull("type")]
		public DynamicAssembly? GetDynamicAssembly(Type? type, bool allowPrivate)
		{
			if ((object)type == null)
			{
				return singletonAssembly;
			}
			DynamicAssembly dynamicAssembly2;
			if (allowPrivate)
			{
				ImmutableHashSet<AssemblyName>.Builder builder = lastCreatedDynamicAssemblySkipVisibilityChecks.ToBuilder();
				int count = builder.Count;
				SkipClrVisibilityChecks.GetSkipVisibilityChecksRequirements(type.GetTypeInfo(), builder);
				lock (this)
				{
					if (builder.Count > count)
					{
						builder.UnionWith(lastCreatedDynamicAssemblySkipVisibilityChecks);
						lastCreatedDynamicAssemblySkipVisibilityChecks = builder.ToImmutable();
						DynamicAssembly dynamicAssembly = NewAssembly();
						dynamicAssembly2 = (singletonAssembly = dynamicAssembly);
						dynamicAssembly2 = dynamicAssembly2;
						goto IL_00b3;
					}
				}
			}
			lock (this)
			{
				dynamicAssembly2 = singletonAssembly ?? (singletonAssembly = NewAssembly());
			}
			goto IL_00b3;
			IL_00b3:
			return dynamicAssembly2;
			DynamicAssembly NewAssembly()
			{
				return new DynamicAssembly(moduleName, lastCreatedDynamicAssemblySkipVisibilityChecks);
			}
		}
	}
}

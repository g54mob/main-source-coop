using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MessagePack.Internal
{
	internal class DynamicAssembly
	{
		internal static readonly bool AvoidDynamicCode = !RuntimeFeature.IsDynamicCodeSupported;

		private readonly AssemblyBuilder assemblyBuilder;

		private readonly ModuleBuilder moduleBuilder;

		public DynamicAssembly(string moduleName, ImmutableHashSet<AssemblyName> skipVisibilityChecksTo)
		{
			AssemblyBuilderAccess access = AssemblyBuilderAccess.RunAndCollect;
			assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(moduleName), access);
			moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName + ".dll");
			new SkipClrVisibilityChecks(assemblyBuilder, moduleBuilder).SkipVisibilityChecksFor(skipVisibilityChecksTo);
		}

		public TypeBuilder DefineType(string name, TypeAttributes attr)
		{
			return moduleBuilder.DefineType(name, attr);
		}

		public TypeBuilder DefineType(string name, TypeAttributes attr, Type? parent)
		{
			return moduleBuilder.DefineType(name, attr, parent);
		}

		public TypeBuilder DefineType(string name, TypeAttributes attr, Type? parent, Type[]? interfaces)
		{
			return moduleBuilder.DefineType(name, attr, parent, interfaces);
		}
	}
}

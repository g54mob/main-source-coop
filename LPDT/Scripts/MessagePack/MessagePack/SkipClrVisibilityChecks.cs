using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MessagePack
{
	internal class SkipClrVisibilityChecks
	{
		private class AssemblyNameEqualityComparer : IEqualityComparer<AssemblyName>
		{
			internal static readonly AssemblyNameEqualityComparer Instance = new AssemblyNameEqualityComparer();

			private AssemblyNameEqualityComparer()
			{
			}

			public bool Equals(AssemblyName? x, AssemblyName? y)
			{
				if (x == null || y == null)
				{
					return x == y;
				}
				return string.Equals(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
			}

			public int GetHashCode([DisallowNull] AssemblyName obj)
			{
				return obj.FullName?.GetHashCode() ?? 0;
			}
		}

		private static readonly ConstructorInfo AttributeBaseClassCtor = typeof(Attribute).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single((ConstructorInfo ctor) => ctor.GetParameters().Length == 0);

		private static readonly ConstructorInfo AttributeUsageCtor = typeof(AttributeUsageAttribute).GetConstructor(new Type[1] { typeof(AttributeTargets) });

		private static readonly PropertyInfo AttributeUsageAllowMultipleProperty = typeof(AttributeUsageAttribute).GetProperty("AllowMultiple");

		private readonly AssemblyBuilder assemblyBuilder;

		private readonly ModuleBuilder moduleBuilder;

		private readonly HashSet<string> attributedAssemblyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private ConstructorInfo? magicAttributeCtor;

		internal static readonly ImmutableHashSet<AssemblyName> EmptySet = ImmutableHashSet.Create((IEqualityComparer<AssemblyName>?)AssemblyNameEqualityComparer.Instance);

		internal SkipClrVisibilityChecks(AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
		{
			this.assemblyBuilder = assemblyBuilder;
			this.moduleBuilder = moduleBuilder;
		}

		internal static void GetSkipVisibilityChecksRequirements(TypeInfo typeInfo, ImmutableHashSet<AssemblyName>.Builder referencedAssemblies)
		{
			if (typeInfo.IsArray)
			{
				GetSkipVisibilityChecksRequirements(typeInfo.GetElementType().GetTypeInfo(), referencedAssemblies);
			}
			AddTypeIfNonPublic(typeInfo);
			Type[] genericTypeArguments = typeInfo.GenericTypeArguments;
			for (int i = 0; i < genericTypeArguments.Length; i++)
			{
				AddTypeIfNonPublic(genericTypeArguments[i]);
			}
			TypeInfo typeInfo2 = typeInfo;
			while ((object)typeInfo2 != null)
			{
				ScanDirectType(typeInfo2);
				typeInfo2 = typeInfo2.BaseType?.GetTypeInfo();
			}
			void AddTypeIfNonPublic(Type type)
			{
				if (type.IsNotPublic || (!type.IsPublic && !type.IsNestedPublic))
				{
					referencedAssemblies.Add(type.Assembly.GetName());
				}
				Type[] genericTypeArguments2 = type.GenericTypeArguments;
				for (int j = 0; j < genericTypeArguments2.Length; j++)
				{
					AddTypeIfNonPublic(genericTypeArguments2[j]);
				}
			}
			void ScanDirectType(TypeInfo typeInfo3)
			{
				MemberInfo[] members = typeInfo3.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MemberInfo memberInfo in members)
				{
					if (!(memberInfo is FieldInfo fieldInfo))
					{
						if (!(memberInfo is PropertyInfo propertyInfo))
						{
							if (memberInfo is ConstructorInfo constructorInfo)
							{
								if (!constructorInfo.IsPublic)
								{
									referencedAssemblies.Add(typeInfo3.Assembly.GetName());
								}
								ParameterInfo[] parameters = constructorInfo.GetParameters();
								for (int k = 0; k < parameters.Length; k++)
								{
									AddTypeIfNonPublic(parameters[k].ParameterType);
								}
							}
						}
						else
						{
							if (((!(propertyInfo.SetMethod?.IsPublic)) ?? false) || ((!(propertyInfo.GetMethod?.IsPublic)) ?? false))
							{
								referencedAssemblies.Add(typeInfo3.Assembly.GetName());
							}
							AddTypeIfNonPublic(propertyInfo.PropertyType);
						}
					}
					else
					{
						if (!fieldInfo.IsPublic)
						{
							referencedAssemblies.Add(typeInfo3.Assembly.GetName());
						}
						AddTypeIfNonPublic(fieldInfo.FieldType);
					}
				}
			}
		}

		internal void SkipVisibilityChecksFor(IEnumerable<AssemblyName> assemblyNames)
		{
			foreach (AssemblyName assemblyName in assemblyNames)
			{
				SkipVisibilityChecksFor(assemblyName);
			}
		}

		internal void SkipVisibilityChecksFor(AssemblyName assemblyName)
		{
			string name = assemblyName.Name;
			if (name != null && attributedAssemblyNames.Add(name))
			{
				CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(GetMagicAttributeCtor(), new object[1] { name });
				assemblyBuilder.SetCustomAttribute(customAttribute);
			}
		}

		private ConstructorInfo GetMagicAttributeCtor()
		{
			if (magicAttributeCtor == null)
			{
				TypeInfo typeInfo = EmitMagicAttribute();
				magicAttributeCtor = typeInfo.GetConstructor(new Type[1] { typeof(string) });
			}
			return magicAttributeCtor;
		}

		private TypeInfo EmitMagicAttribute()
		{
			TypeBuilder typeBuilder = moduleBuilder.DefineType("System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute", TypeAttributes.NotPublic, typeof(Attribute));
			CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(AttributeUsageCtor, new object[1] { AttributeTargets.Assembly }, new PropertyInfo[1] { AttributeUsageAllowMultipleProperty }, new object[1] { false });
			typeBuilder.SetCustomAttribute(customAttribute);
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[1] { typeof(string) });
			constructorBuilder.DefineParameter(1, ParameterAttributes.None, "assemblyName");
			ILGenerator iLGenerator = constructorBuilder.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Call, AttributeBaseClassCtor);
			iLGenerator.Emit(OpCodes.Ret);
			return typeBuilder.CreateTypeInfo();
		}
	}
}

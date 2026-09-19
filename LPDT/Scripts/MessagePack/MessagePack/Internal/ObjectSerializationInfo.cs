using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MessagePack.Internal
{
	internal class ObjectSerializationInfo
	{
		internal class EmittableMemberAndConstructorParameter
		{
			internal EmittableMember MemberInfo { get; }

			internal ParameterInfo ConstructorParameter { get; }

			internal EmittableMemberAndConstructorParameter(EmittableMember memberInfo, ParameterInfo constructorParameter)
			{
				MemberInfo = memberInfo;
				ConstructorParameter = constructorParameter;
			}
		}

		internal class EmittableMember
		{
			private delegate void PropertySetterHelperForStructs<T, TValue>(ref T target, TValue value) where T : struct;

			private Delegate? setterHelperDelegate;

			private FieldBuilder? setterHelperField;

			internal bool IsProperty => (object)PropertyInfo != null;

			internal bool IsField => (object)FieldInfo != null;

			internal bool IsWritable { get; set; }

			internal bool IsWrittenByConstructor { get; set; }

			internal bool IsInitOnly => PropertyInfo?.GetSetMethod(nonPublic: true)?.ReturnParameter.GetRequiredCustomModifiers().Any((Type modifierType) => modifierType.FullName == "System.Runtime.CompilerServices.IsExternalInit") == true;

			internal bool IsReadable { get; set; }

			internal int IntKey { get; set; }

			internal string? StringKey { get; set; }

			internal Type Type => FieldInfo?.FieldType ?? PropertyInfo.PropertyType;

			internal MemberInfo MemberInfo { get; }

			internal FieldInfo? FieldInfo => MemberInfo as FieldInfo;

			internal string Name => PropertyInfo?.Name ?? FieldInfo.Name;

			internal PropertyInfo? PropertyInfo => MemberInfo as PropertyInfo;

			internal bool IsValueType => (PropertyInfo?.PropertyType ?? FieldInfo.FieldType).IsValueType;

			internal bool IsExplicitContract { get; set; }

			internal bool IsProblematicInitProperty { get; set; }

			internal EmittableMember(MemberInfo memberInfo)
			{
				MemberInfo = memberInfo;
			}

			internal MessagePackFormatterAttribute? GetMessagePackFormatterAttribute()
			{
				if ((object)PropertyInfo == null)
				{
					return FieldInfo.GetCustomAttribute<MessagePackFormatterAttribute>(inherit: true);
				}
				return PropertyInfo.GetCustomAttribute<MessagePackFormatterAttribute>(inherit: true);
			}

			internal DataMemberAttribute? GetDataMemberAttribute()
			{
				if ((object)PropertyInfo == null)
				{
					return FieldInfo.GetCustomAttribute<DataMemberAttribute>(inherit: true);
				}
				return PropertyInfo.GetCustomAttribute<DataMemberAttribute>(inherit: true);
			}

			internal void EmitLoadValue(ILGenerator il)
			{
				if ((object)PropertyInfo != null)
				{
					il.EmitCall(PropertyInfo.GetGetMethod(nonPublic: true) ?? throw new Exception("No get accessor"));
				}
				else
				{
					il.Emit(OpCodes.Ldfld, FieldInfo);
				}
			}

			internal void OnTypeCreated(TypeInfo formatterType)
			{
				if ((object)setterHelperDelegate != null && (object)setterHelperField != null)
				{
					formatterType.GetField(setterHelperField.Name, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, setterHelperDelegate);
				}
			}

			internal void EmitPreStoreValue(TypeBuilder typeBuilder, ILGenerator il, LocalBuilder localResult)
			{
				if ((object)PropertyInfo != null && IsProblematicInitProperty)
				{
					Type[] parameterTypes = new Type[2]
					{
						localResult.LocalType.IsClass ? MemberInfo.DeclaringType : MemberInfo.DeclaringType.MakeByRefType(),
						Type
					};
					DynamicMethod dynamicMethod = new DynamicMethod("Set" + Name + "Helper", null, parameterTypes);
					ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
					iLGenerator.Emit(OpCodes.Ldarg_0);
					iLGenerator.Emit(OpCodes.Ldarg_1);
					iLGenerator.EmitCall(PropertyInfo.GetSetMethod(nonPublic: true) ?? throw new Exception("No set accessor"));
					iLGenerator.Emit(OpCodes.Ret);
					Type type = (localResult.LocalType.IsClass ? typeof(Action<, >) : typeof(PropertySetterHelperForStructs<, >)).MakeGenericType(MemberInfo.DeclaringType, Type);
					setterHelperDelegate = dynamicMethod.CreateDelegate(type);
					setterHelperField = typeBuilder.DefineField(Name + "Setter", type, FieldAttributes.Private | FieldAttributes.Static);
					il.Emit(OpCodes.Ldsfld, setterHelperField);
				}
				if (localResult.LocalType.IsClass)
				{
					il.EmitLdloc(localResult);
				}
				else
				{
					il.EmitLdloca(localResult);
				}
			}

			internal void EmitStoreValue(ILGenerator il, TypeBuilder typeBuilder)
			{
				if ((object)PropertyInfo != null)
				{
					if (IsProblematicInitProperty)
					{
						if ((object)setterHelperDelegate == null)
						{
							throw new Exception();
						}
						il.Emit(OpCodes.Callvirt, setterHelperDelegate.GetType().GetMethod("Invoke") ?? throw new Exception("Unable to find Invoke method"));
					}
					else
					{
						il.EmitCall(PropertyInfo.GetSetMethod(nonPublic: true) ?? throw new Exception("No set accessor"));
					}
				}
				else
				{
					il.Emit(OpCodes.Stfld, FieldInfo);
				}
			}
		}

		private class OrderBaseTypesBeforeDerivedTypes : IComparer<Type?>
		{
			internal static readonly OrderBaseTypesBeforeDerivedTypes Instance = new OrderBaseTypesBeforeDerivedTypes();

			private OrderBaseTypesBeforeDerivedTypes()
			{
			}

			public int Compare(Type? x, Type? y)
			{
				if ((object)x == null || (object)y == null)
				{
					throw new NotSupportedException();
				}
				if (!(x == y) && !x.IsEquivalentTo(y))
				{
					if (!x.IsAssignableFrom(y))
					{
						return y.IsAssignableFrom(x) ? 1 : 0;
					}
					return -1;
				}
				return 0;
			}
		}

		internal Type Type { get; }

		internal bool IsIntKey { get; }

		internal bool IsStringKey => !IsIntKey;

		internal bool IsClass { get; }

		internal bool IsStruct => !IsClass;

		internal bool ShouldUseFormatterResolver { get; private set; }

		internal ConstructorInfo? BestmatchConstructor { get; }

		internal EmittableMemberAndConstructorParameter[] ConstructorParameters { get; }

		internal EmittableMember[] Members { get; }

		private ObjectSerializationInfo(Type type, EmittableMemberAndConstructorParameter[] constructorParameters, EmittableMember[] members, bool isClass, ConstructorInfo? bestmatchConstructor, bool isIntKey)
		{
			Type = type;
			ConstructorParameters = constructorParameters;
			Members = members;
			IsClass = isClass;
			BestmatchConstructor = bestmatchConstructor;
			IsIntKey = isIntKey;
			foreach (EmittableMember member in members)
			{
				if (member == null || !member.IsInitOnly)
				{
					continue;
				}
				PropertyInfo propertyInfo = member.PropertyInfo;
				if ((object)propertyInfo != null)
				{
					Type declaringType = propertyInfo.DeclaringType;
					if ((object)declaringType != null && declaringType.IsGenericType && !ConstructorParameters.Any((EmittableMemberAndConstructorParameter cp) => cp.MemberInfo == member))
					{
						member.IsProblematicInitProperty = true;
					}
				}
			}
		}

		internal static ObjectSerializationInfo? CreateOrNull(Type type, bool forceStringKey, bool contractless, bool allowPrivate)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			bool flag = typeInfo.IsClass || typeInfo.IsInterface || typeInfo.IsAbstract;
			bool isClassRecord = flag && IsClassRecord(typeInfo);
			bool isValueType = typeInfo.IsValueType;
			MessagePackObjectAttribute messagePackObjectAttribute = typeInfo.GetCustomAttributes<MessagePackObjectAttribute>().FirstOrDefault();
			DataContractAttribute customAttribute = typeInfo.GetCustomAttribute<DataContractAttribute>();
			if (messagePackObjectAttribute == null && customAttribute == null && !forceStringKey && !contractless)
			{
				return null;
			}
			bool flag2 = true;
			Dictionary<int, EmittableMember> intMembers = new Dictionary<int, EmittableMember>();
			Dictionary<string, EmittableMember> stringMembers = new Dictionary<string, EmittableMember>();
			checked
			{
				if (unchecked(forceStringKey || contractless) || (messagePackObjectAttribute != null && messagePackObjectAttribute.KeyAsPropertyName))
				{
					flag2 = !forceStringKey && (messagePackObjectAttribute == null || !messagePackObjectAttribute.KeyAsPropertyName);
					int num = 0;
					foreach (IGrouping<string, MemberInfo> item in from m in type.GetRuntimeProperties().Concat(type.GetRuntimeFields().Cast<MemberInfo>()).OrderBy<MemberInfo, Type>((MemberInfo m) => m.DeclaringType, OrderBaseTypesBeforeDerivedTypes.Instance)
						group m by m.Name)
					{
						bool flag3 = true;
						foreach (EmittableMember item2 in item.Select(CreateEmittableMember))
						{
							if (item2 != null)
							{
								MemberInfo memberInfo = (MemberInfo)(((object)item2.PropertyInfo) ?? ((object)item2.FieldInfo));
								if (flag3)
								{
									flag3 = false;
									item2.StringKey = memberInfo.Name;
								}
								else
								{
									item2.StringKey = memberInfo.DeclaringType.FullName + "." + memberInfo.Name;
								}
								item2.IntKey = num++;
								AddEmittableMemberOrIgnore(flag2, item2, checkConflicting: false);
							}
						}
					}
				}
				else
				{
					bool flag4 = true;
					int num2 = 0;
					foreach (EmittableMember item3 in GetAllProperties(type).Cast<MemberInfo>().Concat(GetAllFields(type)).Select(CreateEmittableMember))
					{
						if (item3 == null)
						{
							continue;
						}
						MemberInfo memberInfo2 = (MemberInfo)(((object)item3.PropertyInfo) ?? ((object)item3.FieldInfo));
						KeyAttribute keyAttribute;
						if (messagePackObjectAttribute != null)
						{
							keyAttribute = memberInfo2.GetCustomAttribute<KeyAttribute>(inherit: true) ?? throw new MessagePackDynamicObjectResolverException("all public members must mark KeyAttribute or IgnoreMemberAttribute. type:" + type.FullName + " member:" + memberInfo2.Name);
							if (!keyAttribute.IntKey.HasValue && keyAttribute.StringKey == null)
							{
								throw new MessagePackDynamicObjectResolverException("both IntKey and StringKey are null. type: " + type.FullName + " member:" + memberInfo2.Name);
							}
						}
						else
						{
							DataMemberAttribute customAttribute2 = memberInfo2.GetCustomAttribute<DataMemberAttribute>(inherit: true);
							if (customAttribute2 == null)
							{
								continue;
							}
							keyAttribute = ((customAttribute2.Order != -1) ? new KeyAttribute(customAttribute2.Order) : ((customAttribute2.Name != null) ? new KeyAttribute(customAttribute2.Name) : new KeyAttribute(memberInfo2.Name)));
						}
						item3.IsExplicitContract = true;
						if (flag4)
						{
							flag4 = false;
							flag2 = keyAttribute.IntKey.HasValue;
						}
						else if ((flag2 && !keyAttribute.IntKey.HasValue) || (!flag2 && keyAttribute.StringKey == null))
						{
							throw new MessagePackDynamicObjectResolverException("all members key type must be same. type: " + type.FullName + " member:" + memberInfo2.Name);
						}
						if (flag2)
						{
							item3.IntKey = keyAttribute.IntKey.Value;
						}
						else
						{
							item3.StringKey = keyAttribute.StringKey;
							item3.IntKey = num2++;
						}
						AddEmittableMemberOrIgnore(flag2, item3, checkConflicting: true);
					}
				}
				IEnumerator<ConstructorInfo> enumerator3 = null;
				ConstructorInfo ctor = typeInfo.DeclaredConstructors.SingleOrDefault((ConstructorInfo x) => x.GetCustomAttribute<SerializationConstructorAttribute>(inherit: false) != null);
				if (ctor == null)
				{
					enumerator3 = (from x in typeInfo.DeclaredConstructors
						where !x.IsStatic && (allowPrivate || x.IsPublic)
						orderby x.GetParameters().Length descending
						select x).GetEnumerator();
					if (enumerator3.MoveNext())
					{
						ctor = enumerator3.Current;
					}
				}
				if (ctor == null && !isValueType)
				{
					throw new MessagePackDynamicObjectResolverException("can't find public constructor. type:" + type.FullName);
				}
				List<EmittableMemberAndConstructorParameter> list = new List<EmittableMemberAndConstructorParameter>();
				if ((object)ctor != null)
				{
					IReadOnlyDictionary<int, EmittableMember> readOnlyDictionary = intMembers.OrderBy<KeyValuePair<int, EmittableMember>, int>((KeyValuePair<int, EmittableMember> x) => x.Key).Select((KeyValuePair<int, EmittableMember> x, int i) => (Key: x.Value, Index: i)).ToDictionary(((EmittableMember Key, int Index) x) => x.Index, ((EmittableMember Key, int Index) x) => x.Key);
					ILookup<string, KeyValuePair<string, EmittableMember>> lookup = stringMembers.ToLookup<KeyValuePair<string, EmittableMember>, string, KeyValuePair<string, EmittableMember>>((KeyValuePair<string, EmittableMember> x) => x.Key, (KeyValuePair<string, EmittableMember> x) => x, StringComparer.OrdinalIgnoreCase);
					ILookup<string, KeyValuePair<string, EmittableMember>> lookup2 = stringMembers.ToLookup<KeyValuePair<string, EmittableMember>, string, KeyValuePair<string, EmittableMember>>((KeyValuePair<string, EmittableMember> x) => x.Value.Name, (KeyValuePair<string, EmittableMember> x) => x, StringComparer.OrdinalIgnoreCase);
					do
					{
						list.Clear();
						int num3 = 0;
						ParameterInfo[] parameters = ctor.GetParameters();
						foreach (ParameterInfo parameterInfo in parameters)
						{
							EmittableMember value;
							if (flag2)
							{
								if (!readOnlyDictionary.TryGetValue(num3, out value))
								{
									if (enumerator3 != null)
									{
										ctor = null;
										break;
									}
									throw new MessagePackDynamicObjectResolverException("can't find matched constructor parameter, index not found. type:" + type.FullName + " parameterIndex:" + num3);
								}
								if ((!(parameterInfo.ParameterType == value.Type) && !parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(value.Type)) || !value.IsReadable)
								{
									if (enumerator3 != null)
									{
										ctor = null;
										break;
									}
									throw new MessagePackDynamicObjectResolverException("can't find matched constructor parameter, parameterType mismatch. type:" + type.FullName + " parameterIndex:" + num3 + " parameterType:" + parameterInfo.ParameterType.Name);
								}
								list.Add(new EmittableMemberAndConstructorParameter(value, parameterInfo));
							}
							else
							{
								IEnumerable<KeyValuePair<string, EmittableMember>> source = lookup[parameterInfo.Name];
								IEnumerable<KeyValuePair<string, EmittableMember>> enumerable = lookup2[parameterInfo.Name];
								int num5 = source.Count();
								int num6 = enumerable.Count();
								int num7 = num5;
								if (num5 == 0 && num6 != 0)
								{
									num7 = num6;
									source = enumerable;
								}
								if (num7 == 0)
								{
									if (enumerator3 != null)
									{
										ctor = null;
										break;
									}
									throw new MessagePackDynamicObjectResolverException("can't find matched constructor parameter, index not found. type:" + type.FullName + " parameterName:" + parameterInfo.Name);
								}
								value = source.First().Value;
								if (!parameterInfo.ParameterType.IsAssignableFrom(value.Type) || !value.IsReadable)
								{
									if (enumerator3 != null)
									{
										ctor = null;
										break;
									}
									throw new MessagePackDynamicObjectResolverException("can't find matched constructor parameter, parameterType mismatch. type:" + type.FullName + " parameterName:" + parameterInfo.Name + " parameterType:" + parameterInfo.ParameterType.Name);
								}
								list.Add(new EmittableMemberAndConstructorParameter(value, parameterInfo));
							}
							num3++;
						}
					}
					while (TryGetNextConstructor(enumerator3, ref ctor));
					if (ctor == null)
					{
						throw new MessagePackDynamicObjectResolverException("can't find matched constructor. type:" + type.FullName);
					}
				}
				EmittableMember[] source2 = ((!flag2) ? stringMembers.Values.OrderBy((EmittableMember x) => x.GetDataMemberAttribute()?.Order ?? int.MaxValue).ToArray() : intMembers.Values.OrderBy((EmittableMember x) => x.IntKey).ToArray());
				bool shouldUseFormatterResolver = false;
				foreach (EmittableMemberAndConstructorParameter item4 in list)
				{
					item4.MemberInfo.IsWrittenByConstructor = true;
				}
				EmittableMember[] array = source2.Where((EmittableMember m) => m.IsExplicitContract || m.IsWrittenByConstructor || m.IsWritable).ToArray();
				EmittableMember[] array2 = array;
				foreach (EmittableMember emittableMember in array2)
				{
					if (!IsOptimizeTargetType(emittableMember.Type) && emittableMember.GetMessagePackFormatterAttribute() == null)
					{
						shouldUseFormatterResolver = true;
						break;
					}
				}
				return new ObjectSerializationInfo(type, list.ToArray(), array, flag, ctor, flag2)
				{
					ShouldUseFormatterResolver = shouldUseFormatterResolver
				};
			}
			bool AddEmittableMemberOrIgnore(bool isIntKeyMode, EmittableMember member, bool checkConflicting)
			{
				if (checkConflicting && (isIntKeyMode ? intMembers.TryGetValue(member.IntKey, out EmittableMember value2) : stringMembers.TryGetValue(member.StringKey, out value2)))
				{
					if ((object)member.PropertyInfo != null && (object)value2.PropertyInfo != null && member.PropertyInfo.Name == value2.PropertyInfo.Name)
					{
						MethodInfo getMethod = value2.PropertyInfo.GetMethod;
						bool num8 = (object)getMethod != null && getMethod.IsVirtual && !(value2.PropertyInfo.GetMethod?.IsFinal ?? false);
						MethodInfo setMethod = value2.PropertyInfo.SetMethod;
						bool flag5 = (object)setMethod != null && setMethod.IsVirtual && !(value2.PropertyInfo.SetMethod?.IsFinal ?? false);
						if (num8 || flag5)
						{
							return false;
						}
					}
					throw new MessagePackDynamicObjectResolverException(string.Concat(str3: ((MemberInfo)(((object)member.PropertyInfo) ?? ((object)member.FieldInfo)))?.Name, str0: "key is duplicated, all members key must be unique. type:", str1: type.FullName, str2: " member:"));
				}
				if (isIntKeyMode)
				{
					intMembers.Add(member.IntKey, member);
				}
				else
				{
					stringMembers.Add(member.StringKey, member);
				}
				return true;
			}
			EmittableMember? CreateEmittableMember(MemberInfo m)
			{
				if (m.IsDefined(typeof(IgnoreMemberAttribute), inherit: true) || m.IsDefined(typeof(IgnoreDataMemberAttribute), inherit: true) || m.IsDefined(typeof(NonSerializedAttribute), inherit: true))
				{
					return null;
				}
				EmittableMember emittableMember2;
				if (!(m is PropertyInfo propertyInfo))
				{
					if (!(m is FieldInfo fieldInfo))
					{
						throw new MessagePackSerializationException("unexpected member type");
					}
					if (fieldInfo.GetCustomAttribute<CompilerGeneratedAttribute>(inherit: true) != null)
					{
						return null;
					}
					if (fieldInfo.IsStatic)
					{
						return null;
					}
					emittableMember2 = new EmittableMember(fieldInfo)
					{
						IsReadable = (allowPrivate || fieldInfo.IsPublic),
						IsWritable = (allowPrivate || (fieldInfo.IsPublic && !fieldInfo.IsInitOnly))
					};
				}
				else
				{
					if (propertyInfo.IsIndexer())
					{
						return null;
					}
					if (isClassRecord && propertyInfo.Name == "EqualityContract")
					{
						return null;
					}
					MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
					MethodInfo setMethod = propertyInfo.GetSetMethod(nonPublic: true);
					emittableMember2 = new EmittableMember(propertyInfo)
					{
						IsReadable = ((object)getMethod != null && (allowPrivate || getMethod.IsPublic) && !getMethod.IsStatic),
						IsWritable = ((object)setMethod != null && (allowPrivate || setMethod.IsPublic) && !setMethod.IsStatic)
					};
				}
				if (!emittableMember2.IsReadable && !emittableMember2.IsWritable)
				{
					return null;
				}
				return emittableMember2;
			}
		}

		internal static bool IsOptimizeTargetType(Type type)
		{
			if (!(type == typeof(short)) && !(type == typeof(int)) && !(type == typeof(long)) && !(type == typeof(ushort)) && !(type == typeof(uint)) && !(type == typeof(ulong)) && !(type == typeof(float)) && !(type == typeof(double)) && !(type == typeof(bool)) && !(type == typeof(byte)) && !(type == typeof(sbyte)) && !(type == typeof(char)))
			{
				return type == typeof(byte[]);
			}
			return true;
		}

		private static IEnumerable<FieldInfo> GetAllFields(Type type)
		{
			if ((object)type.BaseType != null)
			{
				foreach (FieldInfo allField in GetAllFields(type.BaseType))
				{
					yield return allField;
				}
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++)
			{
				yield return fields[i];
			}
		}

		private static IEnumerable<PropertyInfo> GetAllProperties(Type type)
		{
			if ((object)type.BaseType != null)
			{
				foreach (PropertyInfo allProperty in GetAllProperties(type.BaseType))
				{
					yield return allProperty;
				}
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < properties.Length; i++)
			{
				yield return properties[i];
			}
		}

		private static bool IsClassRecord(TypeInfo type)
		{
			if (type.IsClass)
			{
				return (object)type.GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public) != null;
			}
			return false;
		}

		private static bool TryGetNextConstructor(IEnumerator<ConstructorInfo>? ctorEnumerator, [NotNullWhen(true)] ref ConstructorInfo? ctor)
		{
			if (ctorEnumerator == null || (object)ctor != null)
			{
				return false;
			}
			if (ctorEnumerator.MoveNext())
			{
				ctor = ctorEnumerator.Current;
				return true;
			}
			ctor = null;
			return false;
		}
	}
}

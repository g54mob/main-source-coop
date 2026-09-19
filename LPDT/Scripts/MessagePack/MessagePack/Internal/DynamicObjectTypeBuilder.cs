using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading;
using MessagePack.Formatters;

namespace MessagePack.Internal
{
	internal static class DynamicObjectTypeBuilder
	{
		internal static class MessagePackWriterTypeInfo
		{
			internal static readonly TypeInfo TypeInfo = typeof(MessagePackWriter).GetTypeInfo();

			internal static readonly MethodInfo WriteMapHeader = typeof(MessagePackWriter).GetRuntimeMethod("WriteMapHeader", new Type[1] { typeof(int) });

			internal static readonly MethodInfo WriteArrayHeader = typeof(MessagePackWriter).GetRuntimeMethod("WriteArrayHeader", new Type[1] { typeof(int) });

			internal static readonly MethodInfo WriteBytes = typeof(MessagePackWriter).GetRuntimeMethod("Write", new Type[1] { typeof(ReadOnlySpan<byte>) });

			internal static readonly MethodInfo WriteNil = typeof(MessagePackWriter).GetRuntimeMethod("WriteNil", Type.EmptyTypes);

			internal static readonly MethodInfo WriteRaw = typeof(MessagePackWriter).GetRuntimeMethod("WriteRaw", new Type[1] { typeof(ReadOnlySpan<byte>) });
		}

		internal static class MessagePackReaderTypeInfo
		{
			internal static readonly TypeInfo TypeInfo = typeof(MessagePackReader).GetTypeInfo();

			internal static readonly MethodInfo ReadArrayHeader = typeof(MessagePackReader).GetRuntimeMethod("ReadArrayHeader", Type.EmptyTypes);

			internal static readonly MethodInfo ReadMapHeader = typeof(MessagePackReader).GetRuntimeMethod("ReadMapHeader", Type.EmptyTypes);

			internal static readonly MethodInfo ReadBytes = typeof(MessagePackReader).GetRuntimeMethod("ReadBytes", Type.EmptyTypes);

			internal static readonly MethodInfo TryReadNil = typeof(MessagePackReader).GetRuntimeMethod("TryReadNil", Type.EmptyTypes);

			internal static readonly MethodInfo Skip = typeof(MessagePackReader).GetRuntimeMethod("Skip", Type.EmptyTypes);
		}

		internal static class CodeGenHelpersTypeInfo
		{
			internal static readonly MethodInfo GetEncodedStringBytes = typeof(CodeGenHelpers).GetRuntimeMethod("GetEncodedStringBytes", new Type[1] { typeof(string) });
		}

		internal static class EmitInfo
		{
			internal static class MessagePackFormatterAttr
			{
				internal static readonly MethodInfo FormatterType = ExpressionUtility.GetPropertyInfo((MessagePackFormatterAttribute attr) => attr.FormatterType).GetGetMethod();

				internal static readonly MethodInfo Arguments = ExpressionUtility.GetPropertyInfo((MessagePackFormatterAttribute attr) => attr.Arguments).GetGetMethod();
			}

			internal static readonly MethodInfo GetTypeFromHandle = ExpressionUtility.GetMethodInfo(() => Type.GetTypeFromHandle(default(RuntimeTypeHandle)));

			internal static readonly MethodInfo TypeGetProperty = ExpressionUtility.GetMethodInfo((Type t) => t.GetTypeInfo().GetProperty(null, BindingFlags.Default));

			internal static readonly MethodInfo TypeGetField = ExpressionUtility.GetMethodInfo((Type t) => t.GetTypeInfo().GetField(null, BindingFlags.Default));

			internal static readonly MethodInfo GetCustomAttributeMessagePackFormatterAttribute = ExpressionUtility.GetMethodInfo(() => ((MemberInfo)null).GetCustomAttribute<MessagePackFormatterAttribute>(false));

			internal static readonly MethodInfo ActivatorCreateInstance = ExpressionUtility.GetMethodInfo(() => Activator.CreateInstance((Type)null, (object[])null));
		}

		private class DeserializeInfo
		{
			internal ObjectSerializationInfo.EmittableMember? MemberInfo { get; set; }

			internal LocalBuilder? LocalVariable { get; set; }

			internal LocalBuilder? IsInitializedLocalVariable { get; set; }

			internal Label SwitchLabel { get; set; }
		}

		internal static readonly Regex SubtractFullNameRegex = new Regex(", Version=\\d+.\\d+.\\d+.\\d+, Culture=\\w+, PublicKeyToken=\\w+", (!DynamicAssembly.AvoidDynamicCode) ? RegexOptions.Compiled : RegexOptions.None);

		private static int nameSequence = 0;

		private static HashSet<Type> ignoreTypes = new HashSet<Type>
		{
			typeof(object),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(bool),
			typeof(byte),
			typeof(sbyte),
			typeof(decimal),
			typeof(char),
			typeof(string),
			typeof(Guid),
			typeof(TimeSpan),
			typeof(DateTime),
			typeof(DateTimeOffset),
			typeof(Nil)
		};

		private static readonly Type refMessagePackReader = typeof(MessagePackReader).MakeByRefType();

		private static readonly MethodInfo ReadOnlySpanFromByteArray = typeof(ReadOnlySpan<byte>).GetRuntimeMethod("op_Implicit", new Type[1] { typeof(byte[]) });

		private static readonly MethodInfo ReadStringSpan = typeof(CodeGenHelpers).GetRuntimeMethod("ReadStringSpan", new Type[1] { typeof(MessagePackReader).MakeByRefType() });

		private static readonly MethodInfo ArrayFromNullableReadOnlySequence = typeof(CodeGenHelpers).GetRuntimeMethod("GetArrayFromNullableSequence", new Type[1] { typeof(ReadOnlySequence<byte>?).MakeByRefType() });

		private static readonly MethodInfo getFormatterWithVerify = typeof(FormatterResolverExtensions).GetRuntimeMethods().First((MethodInfo x) => x.Name == "GetFormatterWithVerify");

		private static readonly MethodInfo getResolverFromOptions = typeof(MessagePackSerializerOptions).GetRuntimeProperty("Resolver").GetMethod;

		private static readonly MethodInfo getSecurityFromOptions = typeof(MessagePackSerializerOptions).GetRuntimeProperty("Security").GetMethod;

		private static readonly MethodInfo securityDepthStep = typeof(MessagePackSecurity).GetRuntimeMethod("DepthStep", new Type[1] { typeof(MessagePackReader).MakeByRefType() });

		private static readonly MethodInfo readerDepthGet = typeof(MessagePackReader).GetRuntimeProperty("Depth").GetMethod;

		private static readonly MethodInfo readerDepthSet = typeof(MessagePackReader).GetRuntimeProperty("Depth").SetMethod;

		private static readonly Func<Type, MethodInfo> getSerialize = (Type t) => typeof(IMessagePackFormatter<>).MakeGenericType(t).GetRuntimeMethod("Serialize", new Type[3]
		{
			typeof(MessagePackWriter).MakeByRefType(),
			t,
			typeof(MessagePackSerializerOptions)
		});

		private static readonly Func<Type, MethodInfo> getDeserialize = (Type t) => typeof(IMessagePackFormatter<>).MakeGenericType(t).GetRuntimeMethod("Deserialize", new Type[2]
		{
			refMessagePackReader,
			typeof(MessagePackSerializerOptions)
		});

		private static readonly ConstructorInfo messagePackSerializationExceptionMessageOnlyConstructor = typeof(MessagePackSerializationException).GetTypeInfo().DeclaredConstructors.First(delegate(ConstructorInfo x)
		{
			ParameterInfo[] parameters = x.GetParameters();
			return parameters.Length == 1 && parameters[0].ParameterType == typeof(string);
		});

		private static readonly MethodInfo onBeforeSerialize = typeof(IMessagePackSerializationCallbackReceiver).GetRuntimeMethod("OnBeforeSerialize", Type.EmptyTypes);

		private static readonly MethodInfo onAfterDeserialize = typeof(IMessagePackSerializationCallbackReceiver).GetRuntimeMethod("OnAfterDeserialize", Type.EmptyTypes);

		private static readonly ConstructorInfo objectCtor = typeof(object).GetTypeInfo().DeclaredConstructors.First((ConstructorInfo x) => x.GetParameters().Length == 0);

		internal static TypeInfo? BuildType(DynamicAssembly assembly, Type type, bool forceStringKey, bool contractless, bool allowPrivate)
		{
			if (ignoreTypes.Contains(type))
			{
				return null;
			}
			ObjectSerializationInfo objectSerializationInfo = ObjectSerializationInfo.CreateOrNull(type, forceStringKey, contractless, allowPrivate);
			if (objectSerializationInfo == null)
			{
				return null;
			}
			if (!allowPrivate && !type.IsPublic && !type.IsNestedPublic && !type.GetTypeInfo().IsAnonymous())
			{
				throw new MessagePackSerializationException("Building dynamic formatter only allows public type. Type: " + type.FullName);
			}
			MessagePackEventSource.Instance.FormatterDynamicallyGeneratedStart();
			try
			{
				using (MonoProtection.EnterRefEmitLock())
				{
					Type type2 = typeof(IMessagePackFormatter<>).MakeGenericType(type);
					TypeBuilder typeBuilder = assembly.DefineType("MessagePack.Formatters." + SubtractFullNameRegex.Replace(type.FullName, string.Empty).Replace(".", "_") + "Formatter" + Interlocked.Increment(ref nameSequence), TypeAttributes.Public | TypeAttributes.Sealed, null, new Type[1] { type2 });
					FieldBuilder stringByteKeysField = null;
					Dictionary<ObjectSerializationInfo.EmittableMember, FieldInfo> customFormatterLookup = null;
					if (objectSerializationInfo.IsStringKey)
					{
						ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
						stringByteKeysField = typeBuilder.DefineField("stringByteKeys", typeof(byte[][]), FieldAttributes.Private | FieldAttributes.InitOnly);
						ILGenerator iLGenerator = constructorBuilder.GetILGenerator();
						BuildConstructor(type, objectSerializationInfo, constructorBuilder, stringByteKeysField, iLGenerator);
						customFormatterLookup = BuildCustomFormatterField(typeBuilder, objectSerializationInfo, iLGenerator);
						iLGenerator.Emit(OpCodes.Ret);
					}
					else
					{
						ILGenerator iLGenerator2 = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes).GetILGenerator();
						iLGenerator2.EmitLoadThis();
						iLGenerator2.Emit(OpCodes.Call, objectCtor);
						customFormatterLookup = BuildCustomFormatterField(typeBuilder, objectSerializationInfo, iLGenerator2);
						iLGenerator2.Emit(OpCodes.Ret);
					}
					MethodBuilder methodBuilder = typeBuilder.DefineMethod("Serialize", MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual, null, new Type[3]
					{
						typeof(MessagePackWriter).MakeByRefType(),
						type,
						typeof(MessagePackSerializerOptions)
					});
					methodBuilder.DefineParameter(1, ParameterAttributes.None, "writer");
					methodBuilder.DefineParameter(2, ParameterAttributes.None, "value");
					methodBuilder.DefineParameter(3, ParameterAttributes.None, "options");
					ILGenerator il = methodBuilder.GetILGenerator();
					BuildSerialize(type, objectSerializationInfo, il, delegate
					{
						il.EmitLoadThis();
						il.EmitLdfld(stringByteKeysField);
					}, (int index, ObjectSerializationInfo.EmittableMember member) => (!customFormatterLookup.TryGetValue(member, out FieldInfo fi)) ? null : ((Action)delegate
					{
						il.EmitLoadThis();
						il.EmitLdfld(fi);
					}), 1);
					MethodBuilder methodBuilder2 = typeBuilder.DefineMethod("Deserialize", MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual, type, new Type[2]
					{
						refMessagePackReader,
						typeof(MessagePackSerializerOptions)
					});
					methodBuilder2.DefineParameter(1, ParameterAttributes.None, "reader");
					methodBuilder2.DefineParameter(2, ParameterAttributes.None, "options");
					ILGenerator il2 = methodBuilder2.GetILGenerator();
					BuildDeserialize(type, objectSerializationInfo, typeBuilder, il2, (int index, ObjectSerializationInfo.EmittableMember member) => (!customFormatterLookup.TryGetValue(member, out FieldInfo fi)) ? null : ((Action)delegate
					{
						il2.EmitLoadThis();
						il2.EmitLdfld(fi);
					}), 1);
					TypeInfo typeInfo = typeBuilder.CreateTypeInfo();
					if ((object)typeInfo != null)
					{
						ObjectSerializationInfo.EmittableMember[] members = objectSerializationInfo.Members;
						for (int num = 0; num < members.Length; num++)
						{
							members[num].OnTypeCreated(typeInfo);
						}
					}
					return typeInfo;
				}
			}
			finally
			{
				MessagePackEventSource.Instance.FormatterDynamicallyGeneratedStop(type);
			}
		}

		private static void BuildConstructor(Type type, ObjectSerializationInfo info, ConstructorInfo method, FieldBuilder stringByteKeysField, ILGenerator il)
		{
			il.EmitLoadThis();
			il.Emit(OpCodes.Call, objectCtor);
			int value = info.Members.Count((ObjectSerializationInfo.EmittableMember x) => x.IsReadable);
			il.EmitLoadThis();
			il.EmitLdc_I4(value);
			il.Emit(OpCodes.Newarr, typeof(byte[]));
			int num = 0;
			foreach (ObjectSerializationInfo.EmittableMember item in info.Members.Where((ObjectSerializationInfo.EmittableMember x) => x.IsReadable))
			{
				il.Emit(OpCodes.Dup);
				il.EmitLdc_I4(num);
				il.Emit(OpCodes.Ldstr, item.StringKey);
				il.EmitCall(CodeGenHelpersTypeInfo.GetEncodedStringBytes);
				il.Emit(OpCodes.Stelem_Ref);
				num = checked(num + 1);
			}
			il.Emit(OpCodes.Stfld, stringByteKeysField);
		}

		private static Dictionary<ObjectSerializationInfo.EmittableMember, FieldInfo> BuildCustomFormatterField(TypeBuilder builder, ObjectSerializationInfo info, ILGenerator il)
		{
			Dictionary<ObjectSerializationInfo.EmittableMember, FieldInfo> dictionary = new Dictionary<ObjectSerializationInfo.EmittableMember, FieldInfo>();
			foreach (ObjectSerializationInfo.EmittableMember item in info.Members.Where((ObjectSerializationInfo.EmittableMember x) => x.IsReadable || x.IsWritable))
			{
				MessagePackFormatterAttribute messagePackFormatterAttribute = item.GetMessagePackFormatterAttribute();
				if (messagePackFormatterAttribute == null)
				{
					continue;
				}
				if (!messagePackFormatterAttribute.FormatterType.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessagePackFormatter<>) && i.GenericTypeArguments[0].IsEquivalentTo(item.Type)))
				{
					throw new MessagePackSerializationException(info.Type.FullName + "." + item.Name + " is declared as type " + item.Type.FullName + ", but the prescribed " + messagePackFormatterAttribute.FormatterType.FullName + " does not implement IMessagePackFormatter<" + item.Type.Name + ">.");
				}
				FieldBuilder fieldBuilder = builder.DefineField(item.Name + "_formatter", messagePackFormatterAttribute.FormatterType, FieldAttributes.Private | FieldAttributes.InitOnly);
				if (messagePackFormatterAttribute.Arguments == null || messagePackFormatterAttribute.Arguments.Length == 0)
				{
					FieldInfo fieldInfo = ResolverUtilities.FetchSingletonField(messagePackFormatterAttribute.FormatterType);
					if ((object)fieldInfo != null)
					{
						il.EmitLoadThis();
						il.EmitLdsfld(fieldInfo);
						goto IL_023b;
					}
				}
				int value = 52;
				LocalBuilder local = il.DeclareLocal(typeof(MessagePackFormatterAttribute));
				il.Emit(OpCodes.Ldtoken, info.Type);
				il.EmitCall(EmitInfo.GetTypeFromHandle);
				il.Emit(OpCodes.Ldstr, item.Name);
				il.EmitLdc_I4(value);
				if (item.IsProperty)
				{
					il.EmitCall(EmitInfo.TypeGetProperty);
				}
				else
				{
					il.EmitCall(EmitInfo.TypeGetField);
				}
				il.EmitTrue();
				il.EmitCall(EmitInfo.GetCustomAttributeMessagePackFormatterAttribute);
				il.EmitStloc(local);
				il.EmitLoadThis();
				il.EmitLdloc(local);
				il.EmitCall(EmitInfo.MessagePackFormatterAttr.FormatterType);
				il.EmitLdloc(local);
				il.EmitCall(EmitInfo.MessagePackFormatterAttr.Arguments);
				il.EmitCall(EmitInfo.ActivatorCreateInstance);
				il.Emit(OpCodes.Castclass, messagePackFormatterAttribute.FormatterType);
				goto IL_023b;
				IL_023b:
				il.Emit(OpCodes.Stfld, fieldBuilder);
				dictionary.Add(item, fieldBuilder);
			}
			return dictionary;
		}

		private static void BuildSerialize(Type type, ObjectSerializationInfo info, ILGenerator il, Action emitStringByteKeys, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, int firstArgIndex)
		{
			ArgumentField argWriter = new ArgumentField(il, firstArgIndex);
			checked
			{
				ArgumentField argValue = new ArgumentField(il, firstArgIndex + 1, type);
				ArgumentField argOptions = new ArgumentField(il, firstArgIndex + 2);
				if (type.GetTypeInfo().IsClass)
				{
					Label label = il.DefineLabel();
					argValue.EmitLoad();
					il.Emit(OpCodes.Brtrue_S, label);
					argWriter.EmitLoad();
					il.EmitCall(MessagePackWriterTypeInfo.WriteNil);
					il.Emit(OpCodes.Ret);
					il.MarkLabel(label);
				}
				if (type.GetTypeInfo().ImplementedInterfaces.Any((Type x) => x == typeof(IMessagePackSerializationCallbackReceiver)))
				{
					MethodInfo[] array = (from x in type.GetRuntimeMethods()
						where x.Name == "OnBeforeSerialize"
						select x).ToArray();
					if (array.Length == 1)
					{
						argValue.EmitLoad();
						il.Emit(OpCodes.Call, array[0]);
					}
					else
					{
						argValue.EmitLdarg();
						il.EmitBoxOrDoNothing(type);
						il.EmitCall(onBeforeSerialize);
					}
				}
				LocalBuilder localBuilder = il.DeclareLocal(typeof(IFormatterResolver));
				argOptions.EmitLoad();
				il.EmitCall(getResolverFromOptions);
				il.EmitStloc(localBuilder);
				if (info.IsIntKey)
				{
					int num = (from x in info.Members
						where x.IsReadable
						select x.IntKey).DefaultIfEmpty(-1).Max();
					Dictionary<int, ObjectSerializationInfo.EmittableMember> dictionary = info.Members.Where((ObjectSerializationInfo.EmittableMember x) => x.IsReadable).ToDictionary((ObjectSerializationInfo.EmittableMember x) => x.IntKey);
					int value = num + 1;
					argWriter.EmitLoad();
					il.EmitLdc_I4(value);
					il.EmitCall(MessagePackWriterTypeInfo.WriteArrayHeader);
					int num2 = 0;
					for (int num3 = 0; num3 <= num; num3++)
					{
						if (dictionary.TryGetValue(num3, out var value2))
						{
							EmitSerializeValue(il, type.GetTypeInfo(), value2, num2++, tryEmitLoadCustomFormatter, argWriter, argValue, argOptions, localBuilder);
							continue;
						}
						argWriter.EmitLoad();
						il.EmitCall(MessagePackWriterTypeInfo.WriteNil);
					}
				}
				else
				{
					int value3 = info.Members.Count((ObjectSerializationInfo.EmittableMember x) => x.IsReadable);
					argWriter.EmitLoad();
					il.EmitLdc_I4(value3);
					il.EmitCall(MessagePackWriterTypeInfo.WriteMapHeader);
					int num4 = 0;
					foreach (ObjectSerializationInfo.EmittableMember item in info.Members.Where((ObjectSerializationInfo.EmittableMember x) => x.IsReadable))
					{
						argWriter.EmitLoad();
						emitStringByteKeys();
						il.EmitLdc_I4(num4);
						il.Emit(OpCodes.Ldelem_Ref);
						il.Emit(OpCodes.Call, ReadOnlySpanFromByteArray);
						int num5 = CodeGenHelpers.GetEncodedStringBytes(item.StringKey).Length;
						if (num5 <= 31)
						{
							if (UnsafeMemory.Is32Bit)
							{
								il.EmitCall(typeof(UnsafeMemory32).GetRuntimeMethod("WriteRaw" + num5, new Type[2]
								{
									typeof(MessagePackWriter).MakeByRefType(),
									typeof(ReadOnlySpan<byte>)
								}));
							}
							else
							{
								il.EmitCall(typeof(UnsafeMemory64).GetRuntimeMethod("WriteRaw" + num5, new Type[2]
								{
									typeof(MessagePackWriter).MakeByRefType(),
									typeof(ReadOnlySpan<byte>)
								}));
							}
						}
						else
						{
							il.EmitCall(MessagePackWriterTypeInfo.WriteRaw);
						}
						EmitSerializeValue(il, type.GetTypeInfo(), item, num4, tryEmitLoadCustomFormatter, argWriter, argValue, argOptions, localBuilder);
						num4++;
					}
				}
				il.Emit(OpCodes.Ret);
			}
		}

		private static void EmitSerializeValue(ILGenerator il, TypeInfo type, ObjectSerializationInfo.EmittableMember member, int index, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, ArgumentField argWriter, ArgumentField argValue, ArgumentField argOptions, LocalBuilder localResolver)
		{
			Label label = il.DefineLabel();
			Type type2 = member.Type;
			Action action = tryEmitLoadCustomFormatter(index, member);
			if (action != null)
			{
				action();
				argWriter.EmitLoad();
				argValue.EmitLoad();
				member.EmitLoadValue(il);
				argOptions.EmitLoad();
				il.EmitCall(getSerialize(type2));
			}
			else if (ObjectSerializationInfo.IsOptimizeTargetType(type2))
			{
				if (!type2.GetTypeInfo().IsValueType)
				{
					Label label2 = il.DefineLabel();
					LocalBuilder local = il.DeclareLocal(type2);
					argValue.EmitLoad();
					member.EmitLoadValue(il);
					il.Emit(OpCodes.Dup);
					il.EmitStloc(local);
					il.Emit(OpCodes.Brtrue, label2);
					argWriter.EmitLoad();
					il.EmitCall(MessagePackWriterTypeInfo.WriteNil);
					il.Emit(OpCodes.Br, label);
					il.MarkLabel(label2);
					argWriter.EmitLoad();
					il.EmitLdloc(local);
				}
				else
				{
					argWriter.EmitLoad();
					argValue.EmitLoad();
					member.EmitLoadValue(il);
				}
				if (type2 == typeof(byte[]))
				{
					il.EmitCall(ReadOnlySpanFromByteArray);
					il.EmitCall(MessagePackWriterTypeInfo.WriteBytes);
				}
				else
				{
					il.EmitCall(typeof(MessagePackWriter).GetRuntimeMethod("Write", new Type[1] { type2 }));
				}
			}
			else
			{
				il.EmitLdloc(localResolver);
				il.Emit(OpCodes.Call, getFormatterWithVerify.MakeGenericMethod(type2));
				argWriter.EmitLoad();
				argValue.EmitLoad();
				member.EmitLoadValue(il);
				argOptions.EmitLoad();
				il.EmitCall(getSerialize(type2));
			}
			il.MarkLabel(label);
		}

		private static void BuildDeserialize(Type type, ObjectSerializationInfo info, TypeBuilder typeBuilder, ILGenerator il, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, int firstArgIndex)
		{
			ArgumentField argReader = new ArgumentField(il, firstArgIndex, @ref: true);
			ArgumentField argOptions = new ArgumentField(il, checked(firstArgIndex + 1));
			BuildDeserializeInternalTryReadNil(type, il, ref argReader);
			LocalBuilder localBuilder = il.DeclareLocal(type);
			bool flag = info.ConstructorParameters.Length == 0;
			if (flag)
			{
				BuildDeserializeInternalCreateInstance(type, info, il, localBuilder);
			}
			BuildDeserializeInternalDepthStep(il, ref argReader, ref argOptions);
			LocalBuilder localLength = BuildDeserializeInternalReadHeaderLength(info, il, ref argReader);
			LocalBuilder localResolver = BuildDeserializeInternalResolver(info, il, ref argOptions);
			if (info.IsIntKey)
			{
				BuildDeserializeInternalDeserializeEachPropertyIntKey(info, typeBuilder, il, tryEmitLoadCustomFormatter, flag, ref argReader, ref argOptions, localResolver, localBuilder, localLength);
			}
			else
			{
				BuildDeserializeInternalDeserializeEachPropertyStringKey(info, typeBuilder, il, tryEmitLoadCustomFormatter, flag, ref argReader, argOptions, localResolver, localBuilder, localLength);
			}
			BuildDeserializeInternalOnAfterDeserialize(type, info, il, localBuilder);
			BuildDeserializeInternalDepthUnStep(il, ref argReader);
			il.Emit(OpCodes.Ldloc, localBuilder);
			il.Emit(OpCodes.Ret);
		}

		private static void BuildDeserializeInternalDeserializeEachPropertyStringKey(ObjectSerializationInfo info, TypeBuilder typeBuilder, ILGenerator il, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, bool canOverwrite, ref ArgumentField argReader, ArgumentField argOptions, LocalBuilder localResolver, LocalBuilder localResult, LocalBuilder localLength)
		{
			DeserializeInfo[] infoList = BuildDeserializeInternalDeserializationInfoArrayStringKey(info, il, canOverwrite);
			BuildDeserializeInternalDeserializeLoopStringKey(typeBuilder, il, tryEmitLoadCustomFormatter, ref argReader, ref argOptions, infoList, localResolver, localResult, localLength, canOverwrite, info);
			if (!canOverwrite)
			{
				BuildDeserializeInternalCreateInstanceWithArguments(info, il, infoList, localResult);
				BuildDeserializeInternalAssignFieldFromLocalVariableStringKey(info, typeBuilder, il, infoList, localResult);
			}
		}

		private static void BuildDeserializeInternalDeserializeEachPropertyIntKey(ObjectSerializationInfo info, TypeBuilder typeBuilder, ILGenerator il, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, bool canOverwrite, ref ArgumentField argReader, ref ArgumentField argOptions, LocalBuilder localResolver, LocalBuilder localResult, LocalBuilder localLength)
		{
			Label? gotoDefault;
			int maxKey;
			DeserializeInfo[] infoList = BuildDeserializeInternalDeserializationInfoArrayIntKey(info, il, canOverwrite, out gotoDefault, out maxKey);
			BuildDeserializeInternalDeserializeLoopIntKey(typeBuilder, il, tryEmitLoadCustomFormatter, ref argReader, ref argOptions, infoList, localResolver, localResult, localLength, canOverwrite, gotoDefault);
			if (!canOverwrite)
			{
				BuildDeserializeInternalCreateInstanceWithArguments(info, il, infoList, localResult);
				BuildDeserializeInternalAssignFieldFromLocalVariableIntKey(typeBuilder, info, il, infoList, localResult, localLength, maxKey);
			}
		}

		private static void BuildDeserializeInternalAssignFieldFromLocalVariableStringKey(ObjectSerializationInfo info, TypeBuilder typeBuilder, ILGenerator il, DeserializeInfo[] infoList, LocalBuilder localResult)
		{
			foreach (DeserializeInfo deserializeInfo in infoList)
			{
				if (deserializeInfo.MemberInfo != null && deserializeInfo.IsInitializedLocalVariable != null && !deserializeInfo.MemberInfo.IsWrittenByConstructor)
				{
					Label label = il.DefineLabel();
					il.EmitLdloc(deserializeInfo.IsInitializedLocalVariable);
					il.Emit(OpCodes.Brfalse_S, label);
					if (info.IsClass)
					{
						il.EmitLdloc(localResult);
					}
					else
					{
						il.EmitLdloca(localResult);
					}
					il.EmitLdloc(deserializeInfo.LocalVariable);
					deserializeInfo.MemberInfo.EmitStoreValue(il, typeBuilder);
					il.MarkLabel(label);
				}
			}
		}

		private static void BuildDeserializeInternalAssignFieldFromLocalVariableIntKey(TypeBuilder typeBuilder, ObjectSerializationInfo info, ILGenerator il, DeserializeInfo[] infoList, LocalBuilder localResult, LocalBuilder localLength, int maxKey)
		{
			if (maxKey == -1)
			{
				return;
			}
			Label? label = null;
			Dictionary<int, DeserializeInfo> dictionary = infoList.Where((DeserializeInfo x) => x.MemberInfo != null && x.MemberInfo.IsWritable).ToDictionary((DeserializeInfo x) => x.MemberInfo.IntKey);
			for (int num = 0; num <= maxKey; num = checked(num + 1))
			{
				if (dictionary.TryGetValue(num, out var value) && !value.MemberInfo.IsWrittenByConstructor)
				{
					il.EmitLdloc(localLength);
					il.EmitLdc_I4(num);
					if (!label.HasValue)
					{
						label = il.DefineLabel();
					}
					il.Emit(OpCodes.Ble, label.Value);
					if (info.IsClass)
					{
						il.EmitLdloc(localResult);
					}
					else
					{
						il.EmitLdloca(localResult);
					}
					il.EmitLdloc(value.LocalVariable);
					value.MemberInfo.EmitStoreValue(il, typeBuilder);
				}
			}
			if (label.HasValue)
			{
				il.MarkLabel(label.Value);
			}
		}

		private static void BuildDeserializeInternalCreateInstanceWithArguments(ObjectSerializationInfo info, ILGenerator il, DeserializeInfo[] infoList, LocalBuilder localResult)
		{
			ObjectSerializationInfo.EmittableMemberAndConstructorParameter[] constructorParameters = info.ConstructorParameters;
			foreach (ObjectSerializationInfo.EmittableMemberAndConstructorParameter item in constructorParameters)
			{
				DeserializeInfo deserializeInfo = infoList.First((DeserializeInfo x) => x.MemberInfo == item.MemberInfo);
				il.EmitLdloc(deserializeInfo.LocalVariable);
				if (!item.ConstructorParameter.ParameterType.IsValueType && (deserializeInfo.MemberInfo?.IsValueType ?? false))
				{
					il.Emit(OpCodes.Box, deserializeInfo.MemberInfo.Type);
				}
			}
			il.Emit(OpCodes.Newobj, info.BestmatchConstructor);
			il.Emit(OpCodes.Stloc, localResult);
		}

		private static DeserializeInfo[] BuildDeserializeInternalDeserializationInfoArrayStringKey(ObjectSerializationInfo info, ILGenerator il, bool canOverwrite)
		{
			DeserializeInfo[] array = new DeserializeInfo[info.Members.Length];
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				ObjectSerializationInfo.EmittableMember item = info.Members[i];
				if (canOverwrite && item.IsWritable)
				{
					array[i] = new DeserializeInfo
					{
						MemberInfo = item
					};
					continue;
				}
				bool flag = info.ConstructorParameters.Any((ObjectSerializationInfo.EmittableMemberAndConstructorParameter p) => p.MemberInfo.Equals(item));
				array[i] = new DeserializeInfo
				{
					MemberInfo = item,
					LocalVariable = il.DeclareLocal(item.Type),
					IsInitializedLocalVariable = (flag ? null : il.DeclareLocal(typeof(bool)))
				};
			}
			return array;
		}

		private static DeserializeInfo[] BuildDeserializeInternalDeserializationInfoArrayIntKey(ObjectSerializationInfo info, ILGenerator il, bool canOverwrite, out Label? gotoDefault, out int maxKey)
		{
			maxKey = info.Members.Select((ObjectSerializationInfo.EmittableMember x) => x.IntKey).DefaultIfEmpty(-1).Max();
			checked
			{
				int num = maxKey + 1;
				Dictionary<int, ObjectSerializationInfo.EmittableMember> dictionary = info.Members.ToDictionary((ObjectSerializationInfo.EmittableMember x) => x.IntKey);
				gotoDefault = null;
				DeserializeInfo[] array = new DeserializeInfo[num];
				for (int num2 = 0; num2 < array.Length; num2++)
				{
					if (dictionary.TryGetValue(num2, out var value))
					{
						if (canOverwrite && value.IsWritable)
						{
							array[num2] = new DeserializeInfo
							{
								MemberInfo = value,
								SwitchLabel = il.DefineLabel()
							};
						}
						else
						{
							array[num2] = new DeserializeInfo
							{
								MemberInfo = value,
								LocalVariable = il.DeclareLocal(value.Type),
								SwitchLabel = il.DefineLabel()
							};
						}
					}
					else
					{
						if (!gotoDefault.HasValue)
						{
							gotoDefault = il.DefineLabel();
						}
						array[num2] = new DeserializeInfo
						{
							SwitchLabel = gotoDefault.Value
						};
					}
				}
				return array;
			}
		}

		private static void BuildDeserializeInternalDeserializeLoopIntKey(TypeBuilder typeBuilder, ILGenerator il, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, ref ArgumentField argReader, ref ArgumentField argOptions, DeserializeInfo[] infoList, LocalBuilder localResolver, LocalBuilder localResult, LocalBuilder localLength, bool canOverwrite, Label? gotoDefault)
		{
			LocalBuilder key = il.DeclareLocal(typeof(int));
			Label switchDefault = il.DefineLabel();
			ArgumentField reader = argReader;
			ArgumentField options = argOptions;
			il.EmitIncrementFor(localLength, ForBody);
			void ForBody(LocalBuilder forILocal)
			{
				Label label = il.DefineLabel();
				il.EmitLdloc(forILocal);
				il.EmitStloc(key);
				il.EmitLdloc(key);
				il.Emit(OpCodes.Switch, infoList.Select((DeserializeInfo x) => x.SwitchLabel).ToArray());
				il.MarkLabel(switchDefault);
				reader.EmitLdarg();
				il.EmitCall(MessagePackReaderTypeInfo.Skip);
				il.Emit(OpCodes.Br, label);
				if (gotoDefault.HasValue)
				{
					il.MarkLabel(gotoDefault.Value);
					il.Emit(OpCodes.Br, switchDefault);
				}
				int num = 0;
				DeserializeInfo[] array = infoList;
				checked
				{
					foreach (DeserializeInfo deserializeInfo in array)
					{
						if (deserializeInfo.MemberInfo != null)
						{
							il.MarkLabel(deserializeInfo.SwitchLabel);
							if (canOverwrite)
							{
								BuildDeserializeInternalDeserializeValueAssignDirectly(typeBuilder, il, deserializeInfo, num++, tryEmitLoadCustomFormatter, ref reader, ref options, localResolver, localResult);
							}
							else
							{
								BuildDeserializeInternalDeserializeValueAssignLocalVariable(il, deserializeInfo, num++, tryEmitLoadCustomFormatter, ref reader, ref options, localResolver, localResult);
							}
							il.Emit(OpCodes.Br, label);
						}
					}
					il.MarkLabel(label);
				}
			}
		}

		private static void BuildDeserializeInternalDeserializeLoopStringKey(TypeBuilder typeBuilder, ILGenerator il, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, ref ArgumentField argReader, ref ArgumentField argOptions, DeserializeInfo[] infoList, LocalBuilder localResolver, LocalBuilder localResult, LocalBuilder localLength, bool canOverwrite, ObjectSerializationInfo info)
		{
			AutomataDictionary automata = new AutomataDictionary();
			for (int i = 0; i < info.Members.Length; i = checked(i + 1))
			{
				automata.Add(info.Members[i].StringKey, i);
			}
			LocalBuilder buffer = il.DeclareLocal(typeof(ReadOnlySpan<byte>));
			LocalBuilder longKey = il.DeclareLocal(typeof(ulong));
			ArgumentField reader = argReader;
			ArgumentField options = argOptions;
			il.EmitIncrementFor(localLength, ForBody);
			void ForBody(LocalBuilder forILocal)
			{
				Label readNext = il.DefineLabel();
				Label loopEnd = il.DefineLabel();
				reader.EmitLdarg();
				il.EmitCall(ReadStringSpan);
				il.EmitStloc(buffer);
				if (canOverwrite)
				{
					automata.EmitMatch(il, buffer, longKey, OnFoundAssignDirect, OnNotFound);
				}
				else
				{
					automata.EmitMatch(il, buffer, longKey, OnFoundAssignLocalVariable, OnNotFound);
				}
				il.MarkLabel(readNext);
				reader.EmitLdarg();
				il.EmitCall(MessagePackReaderTypeInfo.Skip);
				il.MarkLabel(loopEnd);
				void OnFoundAssignDirect(KeyValuePair<string?, int> x)
				{
					int value = x.Value;
					DeserializeInfo deserializeInfo = infoList[value];
					if (deserializeInfo.MemberInfo != null)
					{
						BuildDeserializeInternalDeserializeValueAssignDirectly(typeBuilder, il, deserializeInfo, value, tryEmitLoadCustomFormatter, ref reader, ref options, localResolver, localResult);
						il.Emit(OpCodes.Br, loopEnd);
					}
					else
					{
						il.Emit(OpCodes.Br, readNext);
					}
				}
				void OnFoundAssignLocalVariable(KeyValuePair<string?, int> x)
				{
					int value = x.Value;
					DeserializeInfo deserializeInfo = infoList[value];
					if (deserializeInfo.MemberInfo != null)
					{
						BuildDeserializeInternalDeserializeValueAssignLocalVariable(il, deserializeInfo, value, tryEmitLoadCustomFormatter, ref reader, ref options, localResolver, localResult);
						il.Emit(OpCodes.Br, loopEnd);
					}
					else
					{
						il.Emit(OpCodes.Br, readNext);
					}
				}
				void OnNotFound()
				{
					il.Emit(OpCodes.Br, readNext);
				}
			}
		}

		private static void BuildDeserializeInternalTryReadNil(Type type, ILGenerator il, ref ArgumentField argReader)
		{
			Label label = il.DefineLabel();
			argReader.EmitLdarg();
			il.EmitCall(MessagePackReaderTypeInfo.TryReadNil);
			il.Emit(OpCodes.Brfalse_S, label);
			if (type.GetTypeInfo().IsClass)
			{
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Ret);
			}
			else
			{
				il.Emit(OpCodes.Ldstr, "typecode is null, struct not supported");
				il.Emit(OpCodes.Newobj, messagePackSerializationExceptionMessageOnlyConstructor);
				il.Emit(OpCodes.Throw);
			}
			il.MarkLabel(label);
		}

		private static void BuildDeserializeInternalDepthUnStep(ILGenerator il, ref ArgumentField argReader)
		{
			argReader.EmitLdarg();
			il.Emit(OpCodes.Dup);
			il.EmitCall(readerDepthGet);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Sub_Ovf);
			il.EmitCall(readerDepthSet);
		}

		private static void BuildDeserializeInternalOnAfterDeserialize(Type type, ObjectSerializationInfo info, ILGenerator il, LocalBuilder localResult)
		{
			if (type.GetTypeInfo().ImplementedInterfaces.All((Type x) => x != typeof(IMessagePackSerializationCallbackReceiver)))
			{
				return;
			}
			if (info.IsClass)
			{
				il.EmitLdloc(localResult);
			}
			MethodInfo methodInfo = type.GetRuntimeMethods().SingleOrDefault((MethodInfo x) => x.Name == "OnAfterDeserialize");
			if ((object)methodInfo != null)
			{
				if (info.IsStruct)
				{
					il.EmitLdloca(localResult);
				}
				il.Emit(OpCodes.Call, methodInfo);
				return;
			}
			if (info.IsStruct)
			{
				il.EmitLdloc(localResult);
				il.Emit(OpCodes.Box, type);
			}
			il.EmitCall(onAfterDeserialize);
		}

		private static LocalBuilder? BuildDeserializeInternalResolver(ObjectSerializationInfo info, ILGenerator il, ref ArgumentField argOptions)
		{
			if (!info.ShouldUseFormatterResolver)
			{
				return null;
			}
			LocalBuilder localBuilder = il.DeclareLocal(typeof(IFormatterResolver));
			argOptions.EmitLoad();
			il.EmitCall(getResolverFromOptions);
			il.EmitStloc(localBuilder);
			return localBuilder;
		}

		private static LocalBuilder BuildDeserializeInternalReadHeaderLength(ObjectSerializationInfo info, ILGenerator il, ref ArgumentField argReader)
		{
			LocalBuilder localBuilder = il.DeclareLocal(typeof(int));
			argReader.EmitLdarg();
			il.EmitCall(info.IsIntKey ? MessagePackReaderTypeInfo.ReadArrayHeader : MessagePackReaderTypeInfo.ReadMapHeader);
			il.EmitStloc(localBuilder);
			return localBuilder;
		}

		private static void BuildDeserializeInternalDepthStep(ILGenerator il, ref ArgumentField argReader, ref ArgumentField argOptions)
		{
			argOptions.EmitLoad();
			il.EmitCall(getSecurityFromOptions);
			argReader.EmitLdarg();
			il.EmitCall(securityDepthStep);
		}

		private static void BuildDeserializeInternalCreateInstance(Type type, ObjectSerializationInfo info, ILGenerator il, LocalBuilder localResult)
		{
			if (info.IsClass)
			{
				il.Emit(OpCodes.Newobj, info.BestmatchConstructor);
				il.EmitStloc(localResult);
			}
			else
			{
				il.Emit(OpCodes.Ldloca, localResult);
				il.Emit(OpCodes.Initobj, type);
			}
		}

		private static void BuildDeserializeInternalDeserializeValueAssignDirectly(TypeBuilder typeBuilder, ILGenerator il, DeserializeInfo info, int index, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, ref ArgumentField argReader, ref ArgumentField argOptions, LocalBuilder localResolver, LocalBuilder localResult)
		{
			Label label = il.DefineLabel();
			ObjectSerializationInfo.EmittableMember memberInfo = info.MemberInfo;
			Type type = memberInfo.Type;
			Action action = tryEmitLoadCustomFormatter(index, memberInfo);
			if (memberInfo.IsWritable)
			{
				memberInfo.EmitPreStoreValue(typeBuilder, il, localResult);
			}
			else if (info.IsInitializedLocalVariable != null)
			{
				il.EmitLdc_I4(1);
				il.EmitStloc(info.IsInitializedLocalVariable);
			}
			if (action != null)
			{
				action();
				argReader.EmitLdarg();
				argOptions.EmitLoad();
				il.EmitCall(getDeserialize(type));
			}
			else if (ObjectSerializationInfo.IsOptimizeTargetType(type))
			{
				if (!type.GetTypeInfo().IsValueType)
				{
					Label label2 = il.DefineLabel();
					argReader.EmitLdarg();
					il.EmitCall(MessagePackReaderTypeInfo.TryReadNil);
					il.Emit(OpCodes.Brfalse_S, label2);
					il.Emit(OpCodes.Ldnull);
					il.Emit(OpCodes.Br, label);
					il.MarkLabel(label2);
				}
				argReader.EmitLdarg();
				if (type == typeof(byte[]))
				{
					LocalBuilder local = il.DeclareLocal(typeof(ReadOnlySequence<byte>?));
					il.EmitCall(MessagePackReaderTypeInfo.ReadBytes);
					il.EmitStloc(local);
					il.EmitLdloca(local);
					il.EmitCall(ArrayFromNullableReadOnlySequence);
				}
				else
				{
					il.EmitCall(MessagePackReaderTypeInfo.TypeInfo.GetDeclaredMethods("Read" + type.Name).First((MethodInfo x) => x.GetParameters().Length == 0));
				}
			}
			else
			{
				il.EmitLdloc(localResolver);
				il.EmitCall(getFormatterWithVerify.MakeGenericMethod(type));
				argReader.EmitLdarg();
				argOptions.EmitLoad();
				il.EmitCall(getDeserialize(type));
			}
			il.MarkLabel(label);
			if (memberInfo.IsWritable)
			{
				memberInfo.EmitStoreValue(il, typeBuilder);
			}
			else
			{
				il.Emit(OpCodes.Pop);
			}
		}

		private static void BuildDeserializeInternalDeserializeValueAssignLocalVariable(ILGenerator il, DeserializeInfo info, int index, Func<int, ObjectSerializationInfo.EmittableMember, Action?> tryEmitLoadCustomFormatter, ref ArgumentField argReader, ref ArgumentField argOptions, LocalBuilder localResolver, LocalBuilder localResult)
		{
			Label label = il.DefineLabel();
			ObjectSerializationInfo.EmittableMember memberInfo = info.MemberInfo;
			Type type = memberInfo.Type;
			Action action = tryEmitLoadCustomFormatter(index, memberInfo);
			if (info.IsInitializedLocalVariable != null)
			{
				il.EmitLdc_I4(1);
				il.EmitStloc(info.IsInitializedLocalVariable);
			}
			if (action != null)
			{
				action();
				argReader.EmitLdarg();
				argOptions.EmitLoad();
				il.EmitCall(getDeserialize(type));
			}
			else if (ObjectSerializationInfo.IsOptimizeTargetType(type))
			{
				if (!type.GetTypeInfo().IsValueType)
				{
					Label label2 = il.DefineLabel();
					argReader.EmitLdarg();
					il.EmitCall(MessagePackReaderTypeInfo.TryReadNil);
					il.Emit(OpCodes.Brfalse_S, label2);
					il.Emit(OpCodes.Ldnull);
					il.Emit(OpCodes.Br, label);
					il.MarkLabel(label2);
				}
				argReader.EmitLdarg();
				if (type == typeof(byte[]))
				{
					LocalBuilder local = il.DeclareLocal(typeof(ReadOnlySequence<byte>?));
					il.EmitCall(MessagePackReaderTypeInfo.ReadBytes);
					il.EmitStloc(local);
					il.EmitLdloca(local);
					il.EmitCall(ArrayFromNullableReadOnlySequence);
				}
				else
				{
					il.EmitCall(MessagePackReaderTypeInfo.TypeInfo.GetDeclaredMethods("Read" + type.Name).First((MethodInfo x) => x.GetParameters().Length == 0));
				}
			}
			else
			{
				il.EmitLdloc(localResolver);
				il.EmitCall(getFormatterWithVerify.MakeGenericMethod(type));
				argReader.EmitLdarg();
				argOptions.EmitLoad();
				il.EmitCall(getDeserialize(type));
			}
			il.MarkLabel(label);
			il.EmitStloc(info.LocalVariable);
		}

		private static bool Matches(MethodInfo m, int parameterIndex, Type desiredType)
		{
			ParameterInfo[] parameters = m.GetParameters();
			if (parameters.Length > parameterIndex && parameters[parameterIndex].ParameterType.Name == desiredType.Name)
			{
				return parameters[parameterIndex].ParameterType.Namespace == desiredType.Namespace;
			}
			return false;
		}
	}
}

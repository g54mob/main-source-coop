using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Utils.Cil;

namespace MonoMod.Utils
{
	public sealed class DynamicMethodDefinition : IDisposable
	{
		private enum TokenResolutionMode
		{
			Any = 0,
			Type = 1,
			Method = 2,
			Field = 3
		}

		private static Mono.Cecil.Cil.OpCode[] _CecilOpCodes1X;

		private static Mono.Cecil.Cil.OpCode[] _CecilOpCodes2X;

		internal static readonly bool _IsMono;

		internal static readonly bool _IsNewMonoSRE;

		internal static readonly bool _IsOldMonoSRE;

		private static bool _PreferCecil;

		internal static readonly ConstructorInfo c_DebuggableAttribute;

		internal static readonly ConstructorInfo c_UnverifiableCodeAttribute;

		internal static readonly ConstructorInfo c_IgnoresAccessChecksToAttribute;

		internal static readonly Type t__IDMDGenerator;

		internal static readonly Dictionary<string, _IDMDGenerator> _DMDGeneratorCache;

		private MethodDefinition _Definition;

		private ModuleDefinition _Module;

		public string Name;

		public Type OwnerType;

		public bool Debug;

		private Guid GUID = Guid.NewGuid();

		private bool _IsDisposed;

		public static bool IsDynamicILAvailable => !_PreferCecil;

		[Obsolete("Use OriginalMethod instead.")]
		public MethodBase Method => OriginalMethod;

		public MethodBase OriginalMethod { get; private set; }

		public MethodDefinition Definition => _Definition;

		public ModuleDefinition Module => _Module;

		private static void _InitCopier()
		{
			_CecilOpCodes1X = new Mono.Cecil.Cil.OpCode[225];
			_CecilOpCodes2X = new Mono.Cecil.Cil.OpCode[31];
			FieldInfo[] fields = typeof(Mono.Cecil.Cil.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				Mono.Cecil.Cil.OpCode opCode = (Mono.Cecil.Cil.OpCode)fields[i].GetValue(null);
				if (opCode.OpCodeType != Mono.Cecil.Cil.OpCodeType.Nternal)
				{
					if (opCode.Size == 1)
					{
						_CecilOpCodes1X[opCode.Value] = opCode;
					}
					else
					{
						_CecilOpCodes2X[opCode.Value & 0xFF] = opCode;
					}
				}
			}
		}

		private void _CopyMethodToDefinition()
		{
			MethodBase originalMethod = OriginalMethod;
			Module moduleFrom = originalMethod.Module;
			System.Reflection.MethodBody methodBody = originalMethod.GetMethodBody();
			byte[] array = methodBody?.GetILAsByteArray();
			if (array == null)
			{
				throw new NotSupportedException("Body-less method");
			}
			MethodDefinition def = Definition;
			ModuleDefinition moduleTo = def.Module;
			Mono.Cecil.Cil.MethodBody bodyTo = def.Body;
			bodyTo.GetILProcessor();
			Type[] typeArguments = null;
			if (originalMethod.DeclaringType.IsGenericType)
			{
				typeArguments = originalMethod.DeclaringType.GetGenericArguments();
			}
			Type[] methodArguments = null;
			if (originalMethod.IsGenericMethod)
			{
				methodArguments = originalMethod.GetGenericArguments();
			}
			foreach (LocalVariableInfo localVariable in methodBody.LocalVariables)
			{
				TypeReference typeReference = moduleTo.ImportReference(localVariable.LocalType);
				if (localVariable.IsPinned)
				{
					typeReference = new PinnedType(typeReference);
				}
				bodyTo.Variables.Add(new VariableDefinition(typeReference));
			}
			using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(array)))
			{
				Instruction instruction = null;
				Instruction instruction2 = null;
				while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
				{
					int offset = (int)binaryReader.BaseStream.Position;
					instruction = Instruction.Create(Mono.Cecil.Cil.OpCodes.Nop);
					byte b = binaryReader.ReadByte();
					instruction.OpCode = ((b != 254) ? _CecilOpCodes1X[b] : _CecilOpCodes2X[binaryReader.ReadByte()]);
					instruction.Offset = offset;
					if (instruction2 != null)
					{
						instruction2.Next = instruction;
					}
					instruction.Previous = instruction2;
					ReadOperand(binaryReader, instruction);
					bodyTo.Instructions.Add(instruction);
					instruction2 = instruction;
				}
			}
			foreach (Instruction instruction4 in bodyTo.Instructions)
			{
				switch (instruction4.OpCode.OperandType)
				{
				case Mono.Cecil.Cil.OperandType.InlineBrTarget:
				case Mono.Cecil.Cil.OperandType.ShortInlineBrTarget:
					instruction4.Operand = GetInstruction((int)instruction4.Operand);
					break;
				case Mono.Cecil.Cil.OperandType.InlineSwitch:
				{
					int[] array2 = (int[])instruction4.Operand;
					Instruction[] array3 = new Instruction[array2.Length];
					for (int i = 0; i < array2.Length; i++)
					{
						array3[i] = GetInstruction(array2[i]);
					}
					instruction4.Operand = array3;
					break;
				}
				}
			}
			foreach (ExceptionHandlingClause exceptionHandlingClause in methodBody.ExceptionHandlingClauses)
			{
				Mono.Cecil.Cil.ExceptionHandler exceptionHandler = new Mono.Cecil.Cil.ExceptionHandler((ExceptionHandlerType)exceptionHandlingClause.Flags);
				bodyTo.ExceptionHandlers.Add(exceptionHandler);
				exceptionHandler.TryStart = GetInstruction(exceptionHandlingClause.TryOffset);
				exceptionHandler.TryEnd = GetInstruction(exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength);
				exceptionHandler.FilterStart = ((exceptionHandler.HandlerType != ExceptionHandlerType.Filter) ? null : GetInstruction(exceptionHandlingClause.FilterOffset));
				exceptionHandler.HandlerStart = GetInstruction(exceptionHandlingClause.HandlerOffset);
				exceptionHandler.HandlerEnd = GetInstruction(exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength);
				exceptionHandler.CatchType = ((exceptionHandler.HandlerType != ExceptionHandlerType.Catch) ? null : (((object)exceptionHandlingClause.CatchType == null) ? null : moduleTo.ImportReference(exceptionHandlingClause.CatchType)));
			}
			Instruction GetInstruction(int num2)
			{
				int num = bodyTo.Instructions.Count - 1;
				if (num2 < 0 || num2 > bodyTo.Instructions[num].Offset)
				{
					return null;
				}
				int num3 = 0;
				int num4 = num;
				while (num3 <= num4)
				{
					int num5 = num3 + (num4 - num3) / 2;
					Instruction instruction3 = bodyTo.Instructions[num5];
					if (num2 == instruction3.Offset)
					{
						return instruction3;
					}
					if (num2 < instruction3.Offset)
					{
						num4 = num5 - 1;
					}
					else
					{
						num3 = num5 + 1;
					}
				}
				return null;
			}
			void ReadOperand(BinaryReader reader, Instruction instr)
			{
				switch (instr.OpCode.OperandType)
				{
				case Mono.Cecil.Cil.OperandType.InlineNone:
					instr.Operand = null;
					break;
				case Mono.Cecil.Cil.OperandType.InlineSwitch:
				{
					int num = reader.ReadInt32();
					int num2 = (int)reader.BaseStream.Position + 4 * num;
					int[] array4 = new int[num];
					for (int j = 0; j < num; j++)
					{
						array4[j] = reader.ReadInt32() + num2;
					}
					instr.Operand = array4;
					break;
				}
				case Mono.Cecil.Cil.OperandType.ShortInlineBrTarget:
				{
					int num2 = reader.ReadSByte();
					instr.Operand = (int)reader.BaseStream.Position + num2;
					break;
				}
				case Mono.Cecil.Cil.OperandType.InlineBrTarget:
				{
					int num2 = reader.ReadInt32();
					instr.Operand = (int)reader.BaseStream.Position + num2;
					break;
				}
				case Mono.Cecil.Cil.OperandType.ShortInlineI:
					instr.Operand = ((instr.OpCode == Mono.Cecil.Cil.OpCodes.Ldc_I4_S) ? ((object)reader.ReadSByte()) : ((object)reader.ReadByte()));
					break;
				case Mono.Cecil.Cil.OperandType.InlineI:
					instr.Operand = reader.ReadInt32();
					break;
				case Mono.Cecil.Cil.OperandType.ShortInlineR:
					instr.Operand = reader.ReadSingle();
					break;
				case Mono.Cecil.Cil.OperandType.InlineR:
					instr.Operand = reader.ReadDouble();
					break;
				case Mono.Cecil.Cil.OperandType.InlineI8:
					instr.Operand = reader.ReadInt64();
					break;
				case Mono.Cecil.Cil.OperandType.InlineSig:
					instr.Operand = moduleTo.ImportCallSite(moduleFrom, moduleFrom.ResolveSignature(reader.ReadInt32()));
					break;
				case Mono.Cecil.Cil.OperandType.InlineString:
					instr.Operand = moduleFrom.ResolveString(reader.ReadInt32());
					break;
				case Mono.Cecil.Cil.OperandType.InlineTok:
					instr.Operand = ResolveTokenAs(reader.ReadInt32(), TokenResolutionMode.Any);
					break;
				case Mono.Cecil.Cil.OperandType.InlineType:
					instr.Operand = ResolveTokenAs(reader.ReadInt32(), TokenResolutionMode.Type);
					break;
				case Mono.Cecil.Cil.OperandType.InlineMethod:
					instr.Operand = ResolveTokenAs(reader.ReadInt32(), TokenResolutionMode.Method);
					break;
				case Mono.Cecil.Cil.OperandType.InlineField:
					instr.Operand = ResolveTokenAs(reader.ReadInt32(), TokenResolutionMode.Field);
					break;
				case Mono.Cecil.Cil.OperandType.InlineVar:
				case Mono.Cecil.Cil.OperandType.ShortInlineVar:
				{
					int index = ((instr.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineVar) ? reader.ReadByte() : reader.ReadInt16());
					instr.Operand = bodyTo.Variables[index];
					break;
				}
				case Mono.Cecil.Cil.OperandType.InlineArg:
				case Mono.Cecil.Cil.OperandType.ShortInlineArg:
				{
					int index = ((instr.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineArg) ? reader.ReadByte() : reader.ReadInt16());
					instr.Operand = def.Parameters[index];
					break;
				}
				default:
					throw new NotSupportedException("Unsupported opcode $" + instr.OpCode.Name);
				}
			}
			MemberReference ResolveTokenAs(int token, TokenResolutionMode resolveMode)
			{
				try
				{
					switch (resolveMode)
					{
					case TokenResolutionMode.Type:
					{
						Type type2 = moduleFrom.ResolveType(token, typeArguments, methodArguments);
						type2.FixReflectionCacheAuto();
						return moduleTo.ImportReference(type2);
					}
					case TokenResolutionMode.Method:
					{
						MethodBase methodBase2 = moduleFrom.ResolveMethod(token, typeArguments, methodArguments);
						methodBase2.GetRealDeclaringType()?.FixReflectionCacheAuto();
						return moduleTo.ImportReference(methodBase2);
					}
					case TokenResolutionMode.Field:
					{
						FieldInfo fieldInfo2 = moduleFrom.ResolveField(token, typeArguments, methodArguments);
						fieldInfo2.GetRealDeclaringType()?.FixReflectionCacheAuto();
						return moduleTo.ImportReference(fieldInfo2);
					}
					case TokenResolutionMode.Any:
					{
						MemberInfo memberInfo = moduleFrom.ResolveMember(token, typeArguments, methodArguments);
						if (memberInfo is Type type)
						{
							type.FixReflectionCacheAuto();
							return moduleTo.ImportReference(type);
						}
						if (memberInfo is MethodBase methodBase)
						{
							methodBase.GetRealDeclaringType()?.FixReflectionCacheAuto();
							return moduleTo.ImportReference(methodBase);
						}
						if (!(memberInfo is FieldInfo fieldInfo))
						{
							throw new NotSupportedException($"Invalid resolved member type {memberInfo.GetType()}");
						}
						fieldInfo.GetRealDeclaringType()?.FixReflectionCacheAuto();
						return moduleTo.ImportReference(fieldInfo);
					}
					default:
						throw new NotSupportedException($"Invalid TokenResolutionMode {resolveMode}");
					}
				}
				catch (MissingMemberException)
				{
					string location = moduleFrom.Assembly.Location;
					if (!File.Exists(location))
					{
						throw;
					}
					using AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(location, new ReaderParameters
					{
						ReadingMode = ReadingMode.Deferred
					});
					MemberReference memberReference = (MemberReference)assemblyDefinition.Modules.First((ModuleDefinition m) => m.Name == moduleFrom.Name).LookupToken(token);
					return resolveMode switch
					{
						TokenResolutionMode.Type => (TypeReference)memberReference, 
						TokenResolutionMode.Method => (MethodReference)memberReference, 
						TokenResolutionMode.Field => (FieldReference)memberReference, 
						TokenResolutionMode.Any => memberReference, 
						_ => throw new NotSupportedException($"Invalid TokenResolutionMode {resolveMode}"), 
					};
				}
			}
		}

		static DynamicMethodDefinition()
		{
			_IsMono = (object)Type.GetType("Mono.Runtime") != null;
			_IsNewMonoSRE = _IsMono && (object)typeof(DynamicMethod).GetField("il_info", BindingFlags.Instance | BindingFlags.NonPublic) != null;
			_IsOldMonoSRE = _IsMono && !_IsNewMonoSRE && (object)typeof(DynamicMethod).GetField("ilgen", BindingFlags.Instance | BindingFlags.NonPublic) != null;
			_PreferCecil = (_IsMono && !_IsNewMonoSRE && !_IsOldMonoSRE) || (!_IsMono && (object)typeof(ILGenerator).Assembly.GetType("System.Reflection.Emit.DynamicILGenerator")?.GetField("m_scope", BindingFlags.Instance | BindingFlags.NonPublic) == null);
			c_DebuggableAttribute = typeof(DebuggableAttribute).GetConstructor(new Type[1] { typeof(DebuggableAttribute.DebuggingModes) });
			c_UnverifiableCodeAttribute = typeof(UnverifiableCodeAttribute).GetConstructor(new Type[0]);
			c_IgnoresAccessChecksToAttribute = typeof(IgnoresAccessChecksToAttribute).GetConstructor(new Type[1] { typeof(string) });
			t__IDMDGenerator = typeof(_IDMDGenerator);
			_DMDGeneratorCache = new Dictionary<string, _IDMDGenerator>();
			_InitCopier();
		}

		internal DynamicMethodDefinition()
		{
			Debug = Environment.GetEnvironmentVariable("MONOMOD_DMD_DEBUG") == "1";
		}

		public DynamicMethodDefinition(MethodBase method)
			: this()
		{
			OriginalMethod = method ?? throw new ArgumentNullException("method");
			Reload();
		}

		public DynamicMethodDefinition(string name, Type returnType, Type[] parameterTypes)
			: this()
		{
			Name = name;
			OriginalMethod = null;
			_CreateDynModule(name, returnType, parameterTypes);
		}

		public ILProcessor GetILProcessor()
		{
			return Definition.Body.GetILProcessor();
		}

		public ILGenerator GetILGenerator()
		{
			return new CecilILGenerator(Definition.Body.GetILProcessor()).GetProxy();
		}

		private ModuleDefinition _CreateDynModule(string name, Type returnType, Type[] parameterTypes)
		{
			ModuleDefinition moduleDefinition = (_Module = ModuleDefinition.CreateModule($"DMD:DynModule<{name}>?{GetHashCode()}", new ModuleParameters
			{
				Kind = ModuleKind.Dll,
				ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault
			}));
			TypeDefinition typeDefinition = new TypeDefinition("", $"DMD<{name}>?{GetHashCode()}", Mono.Cecil.TypeAttributes.Public);
			moduleDefinition.Types.Add(typeDefinition);
			MethodDefinition methodDefinition = (_Definition = new MethodDefinition(name, Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.HideBySig, ((object)returnType != null) ? moduleDefinition.ImportReference(returnType) : moduleDefinition.TypeSystem.Void));
			foreach (Type type in parameterTypes)
			{
				methodDefinition.Parameters.Add(new ParameterDefinition(moduleDefinition.ImportReference(type)));
			}
			typeDefinition.Methods.Add(methodDefinition);
			return moduleDefinition;
		}

		public void Reload()
		{
			MethodBase originalMethod = OriginalMethod;
			if ((object)originalMethod == null)
			{
				throw new InvalidOperationException();
			}
			ModuleDefinition moduleDefinition = null;
			try
			{
				_Definition = null;
				_Module?.Dispose();
				_Module = null;
				ParameterInfo[] parameters = originalMethod.GetParameters();
				int num = 0;
				Type[] array;
				if (!originalMethod.IsStatic)
				{
					num++;
					array = new Type[parameters.Length + 1];
					array[0] = originalMethod.GetThisParamType();
				}
				else
				{
					array = new Type[parameters.Length];
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i + num] = parameters[i].ParameterType;
				}
				moduleDefinition = _CreateDynModule(originalMethod.GetID(null, null, withType: true, proxyMethod: false, simple: true), (originalMethod as MethodInfo)?.ReturnType, array);
				_CopyMethodToDefinition();
				MethodDefinition definition = Definition;
				if (!originalMethod.IsStatic)
				{
					definition.Parameters[0].Name = "this";
				}
				for (int j = 0; j < parameters.Length; j++)
				{
					definition.Parameters[j + num].Name = parameters[j].Name;
				}
				_Module = moduleDefinition;
				moduleDefinition = null;
			}
			catch
			{
				moduleDefinition?.Dispose();
				throw;
			}
		}

		public MethodInfo Generate()
		{
			return Generate(null);
		}

		public MethodInfo Generate(object context)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("MONOMOD_DMD_TYPE");
			switch (environmentVariable?.ToLower(CultureInfo.InvariantCulture))
			{
			case "dynamicmethod":
			case "dm":
				return DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(this, context);
			case "methodbuilder":
			case "mb":
				return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
			case "cecil":
			case "md":
				return DMDGenerator<DMDCecilGenerator>.Generate(this, context);
			default:
			{
				Type type = ReflectionHelper.GetType(environmentVariable);
				if ((object)type != null)
				{
					if (!t__IDMDGenerator.IsCompatible(type))
					{
						throw new ArgumentException("Invalid DMDGenerator type: " + environmentVariable);
					}
					if (!_DMDGeneratorCache.TryGetValue(environmentVariable, out var value))
					{
						value = (_DMDGeneratorCache[environmentVariable] = Activator.CreateInstance(type) as _IDMDGenerator);
					}
					return value.Generate(this, context);
				}
				if (_PreferCecil)
				{
					return DMDGenerator<DMDCecilGenerator>.Generate(this, context);
				}
				if (Debug)
				{
					return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
				}
				if (Definition.Body.ExceptionHandlers.Any((Mono.Cecil.Cil.ExceptionHandler eh) => eh.HandlerType == ExceptionHandlerType.Fault || eh.HandlerType == ExceptionHandlerType.Filter))
				{
					return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
				}
				return DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(this, context);
			}
			}
		}

		public void Dispose()
		{
			if (!_IsDisposed)
			{
				_IsDisposed = true;
				_Module.Dispose();
			}
		}

		public string GetDumpName(string type)
		{
			return string.Format("DMDASM.{0:X8}{1}", GUID.GetHashCode(), string.IsNullOrEmpty(type) ? "" : ("." + type));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoMod.Utils
{
	public sealed class DMDCecilGenerator : DMDGenerator<DMDCecilGenerator>
	{
		protected override MethodInfo _Generate(DynamicMethodDefinition dmd, object context)
		{
			MethodDefinition def = dmd.Definition;
			TypeDefinition typeDefinition = context as TypeDefinition;
			bool flag = false;
			ModuleDefinition module = typeDefinition?.Module;
			HashSet<string> hashSet = null;
			if (typeDefinition == null)
			{
				flag = true;
				hashSet = new HashSet<string>();
				string dumpName = dmd.GetDumpName("Cecil");
				module = ModuleDefinition.CreateModule(dumpName, new ModuleParameters
				{
					Kind = ModuleKind.Dll,
					ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault
				});
				module.Assembly.CustomAttributes.Add(new CustomAttribute(module.ImportReference(DynamicMethodDefinition.c_UnverifiableCodeAttribute)));
				if (dmd.Debug)
				{
					CustomAttribute customAttribute = new CustomAttribute(module.ImportReference(DynamicMethodDefinition.c_DebuggableAttribute));
					customAttribute.ConstructorArguments.Add(new CustomAttributeArgument(module.ImportReference(typeof(DebuggableAttribute.DebuggingModes)), DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations));
					module.Assembly.CustomAttributes.Add(customAttribute);
				}
				typeDefinition = new TypeDefinition("", $"DMD<{dmd.OriginalMethod?.Name?.Replace('.', '_')}>?{GetHashCode()}", Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed)
				{
					BaseType = module.TypeSystem.Object
				};
				module.Types.Add(typeDefinition);
			}
			try
			{
				MethodDefinition clone = null;
				TypeReference typeReference = new TypeReference("System.Runtime.CompilerServices", "IsVolatile", module, module.TypeSystem.CoreLibrary);
				Relinker relinker = (IMetadataTokenProvider metadataTokenProvider, IGenericParameterProvider ctx) => (metadataTokenProvider == def) ? clone : module.ImportReference(metadataTokenProvider);
				clone = new MethodDefinition(dmd.Name ?? ("_" + def.Name.Replace('.', '_')), def.Attributes, module.TypeSystem.Void)
				{
					MethodReturnType = def.MethodReturnType,
					Attributes = (Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.HideBySig),
					ImplAttributes = Mono.Cecil.MethodImplAttributes.IL,
					DeclaringType = typeDefinition,
					NoInlining = true
				};
				foreach (ParameterDefinition parameter in def.Parameters)
				{
					clone.Parameters.Add(parameter.Clone().Relink(relinker, clone));
				}
				clone.ReturnType = def.ReturnType.Relink(relinker, clone);
				typeDefinition.Methods.Add(clone);
				clone.HasThis = def.HasThis;
				Mono.Cecil.Cil.MethodBody methodBody = (clone.Body = def.Body.Clone(clone));
				Mono.Cecil.Cil.MethodBody methodBody3 = methodBody;
				foreach (VariableDefinition variable in clone.Body.Variables)
				{
					variable.VariableType = variable.VariableType.Relink(relinker, clone);
				}
				foreach (ExceptionHandler exceptionHandler in clone.Body.ExceptionHandlers)
				{
					if (exceptionHandler.CatchType != null)
					{
						exceptionHandler.CatchType = exceptionHandler.CatchType.Relink(relinker, clone);
					}
				}
				for (int num = 0; num < methodBody3.Instructions.Count; num++)
				{
					Instruction instruction = methodBody3.Instructions[num];
					object obj = instruction.Operand;
					if (obj is ParameterDefinition parameterDefinition)
					{
						obj = clone.Parameters[parameterDefinition.Index];
					}
					else if (obj is IMetadataTokenProvider mtp)
					{
						obj = mtp.Relink(relinker, clone);
					}
					OpCode? opCode = instruction.Previous?.OpCode;
					OpCode opCode2 = OpCodes.Volatile;
					if (opCode.HasValue && (!opCode.HasValue || opCode.GetValueOrDefault() == opCode2) && obj is FieldReference fieldReference && (fieldReference.FieldType as RequiredModifierType)?.ModifierType != typeReference)
					{
						fieldReference.FieldType = new RequiredModifierType(typeReference, fieldReference.FieldType);
					}
					_ = obj is DynamicMethodReference;
					if (hashSet != null && obj is MemberReference memberReference)
					{
						IMetadataScope metadataScope = (memberReference as TypeReference)?.Scope ?? memberReference.DeclaringType.Scope;
						if (!hashSet.Contains(metadataScope.Name))
						{
							CustomAttribute item = new CustomAttribute(module.ImportReference(DynamicMethodDefinition.c_IgnoresAccessChecksToAttribute))
							{
								ConstructorArguments = 
								{
									new CustomAttributeArgument(module.ImportReference(typeof(DebuggableAttribute.DebuggingModes)), metadataScope.Name)
								}
							};
							module.Assembly.CustomAttributes.Add(item);
							hashSet.Add(metadataScope.Name);
						}
					}
					instruction.Operand = obj;
				}
				clone.HasThis = false;
				if (def.HasThis)
				{
					TypeReference typeReference2 = def.DeclaringType;
					if (typeReference2.IsValueType)
					{
						typeReference2 = new ByReferenceType(typeReference2);
					}
					clone.Parameters.Insert(0, new ParameterDefinition("<>_this", Mono.Cecil.ParameterAttributes.None, typeReference2.Relink(relinker, clone)));
				}
				if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONOMOD_DMD_DUMP")))
				{
					string fullPath = Path.GetFullPath(Environment.GetEnvironmentVariable("MONOMOD_DMD_DUMP"));
					string path = module.Name + ".dll";
					string path2 = Path.Combine(fullPath, path);
					fullPath = Path.GetDirectoryName(path2);
					if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
					{
						Directory.CreateDirectory(fullPath);
					}
					if (File.Exists(path2))
					{
						File.Delete(path2);
					}
					using Stream stream = File.OpenWrite(path2);
					module.Write(stream);
				}
				return ReflectionHelper.Load(module).GetType(MultiTargetShims.Replace(typeDefinition.FullName, "+", "\\+", StringComparison.Ordinal), throwOnError: false, ignoreCase: false).GetMethod(clone.Name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			finally
			{
				if (flag)
				{
					module.Dispose();
				}
			}
		}
	}
}

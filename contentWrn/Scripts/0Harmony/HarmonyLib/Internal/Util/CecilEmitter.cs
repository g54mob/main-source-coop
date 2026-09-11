using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using HarmonyLib.Tools;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Utils;

namespace HarmonyLib.Internal.Util
{
	internal static class CecilEmitter
	{
		private static readonly ConstructorInfo UnverifiableCodeAttributeConstructor = typeof(UnverifiableCodeAttribute).GetConstructor(Type.EmptyTypes);

		public static void Dump(MethodDefinition md, IEnumerable<string> dumpPaths, MethodBase original = null)
		{
			string name = string.Format("HarmonyDump.{0}.{1:X8}", md.GetID(null, null, withType: false, simple: true).Replace(":", "_").Replace(" ", "_"), Guid.NewGuid().GetHashCode());
			string arg = (original?.Name ?? md.Name).Replace('.', '_');
			ModuleDefinition module = ModuleDefinition.CreateModule(name, new ModuleParameters
			{
				Kind = ModuleKind.Dll,
				ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault
			});
			try
			{
				module.Assembly.CustomAttributes.Add(new CustomAttribute(module.ImportReference(UnverifiableCodeAttributeConstructor)));
				int hashCode = Guid.NewGuid().GetHashCode();
				TypeDefinition typeDefinition = new TypeDefinition("", $"HarmonyDump<{arg}>?{hashCode}", Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed)
				{
					BaseType = module.TypeSystem.Object
				};
				module.Types.Add(typeDefinition);
				MethodDefinition clone = null;
				TypeReference typeReference = new TypeReference("System.Runtime.CompilerServices", "IsVolatile", module, module.TypeSystem.CoreLibrary);
				Relinker relinker = (IMetadataTokenProvider metadataTokenProvider, IGenericParameterProvider _) => (metadataTokenProvider != md) ? module.ImportReference(metadataTokenProvider) : clone;
				clone = new MethodDefinition(original?.Name ?? ("_" + md.Name.Replace(".", "_")), md.Attributes, module.TypeSystem.Void)
				{
					MethodReturnType = md.MethodReturnType,
					Attributes = (Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.HideBySig),
					ImplAttributes = Mono.Cecil.MethodImplAttributes.IL,
					DeclaringType = typeDefinition,
					HasThis = false
				};
				typeDefinition.Methods.Add(clone);
				foreach (ParameterDefinition parameter in md.Parameters)
				{
					clone.Parameters.Add(parameter.Clone().Relink(relinker, clone));
				}
				clone.ReturnType = md.ReturnType.Relink(relinker, clone);
				Mono.Cecil.Cil.MethodBody methodBody = (clone.Body = md.Body.Clone(clone));
				Mono.Cecil.Cil.MethodBody methodBody3 = methodBody;
				foreach (VariableDefinition variable in clone.Body.Variables)
				{
					variable.VariableType = variable.VariableType.Relink(relinker, clone);
				}
				foreach (ExceptionHandler item in clone.Body.ExceptionHandlers.Where((ExceptionHandler handler) => handler.CatchType != null))
				{
					item.CatchType = item.CatchType.Relink(relinker, clone);
				}
				foreach (Instruction instruction in methodBody3.Instructions)
				{
					object operand = instruction.Operand;
					object obj = ((operand is ParameterDefinition parameterDefinition) ? clone.Parameters[parameterDefinition.Index] : ((!(operand is IMetadataTokenProvider mtp)) ? operand : mtp.Relink(relinker, clone)));
					operand = obj;
					OpCode? opCode = instruction.Previous?.OpCode;
					OpCode opCode2 = OpCodes.Volatile;
					if (opCode.HasValue && (!opCode.HasValue || opCode.GetValueOrDefault() == opCode2) && operand is FieldReference fieldReference && (fieldReference.FieldType as RequiredModifierType)?.ModifierType != typeReference)
					{
						fieldReference.FieldType = new RequiredModifierType(typeReference, fieldReference.FieldType);
					}
					instruction.Operand = operand;
				}
				if (md.HasThis)
				{
					TypeReference typeReference2 = md.DeclaringType;
					if (typeReference2.IsValueType)
					{
						typeReference2 = new ByReferenceType(typeReference2);
					}
					clone.Parameters.Insert(0, new ParameterDefinition("<>_this", Mono.Cecil.ParameterAttributes.None, typeReference2.Relink(relinker, clone)));
				}
				foreach (string dumpPath in dumpPaths)
				{
					string fullPath = Path.GetFullPath(dumpPath);
					try
					{
						Directory.CreateDirectory(fullPath);
						using FileStream stream = File.OpenWrite(Path.Combine(fullPath, module.Name + ".dll"));
						module.Write(stream);
					}
					catch (Exception ex)
					{
						Exception e = ex;
						Logger.Log(Logger.LogChannel.Error, () => $"Failed to dump {md.GetID(null, null, withType: true, simple: true)} to {fullPath}: {e}");
					}
				}
			}
			finally
			{
				if (module != null)
				{
					((IDisposable)module).Dispose();
				}
			}
		}
	}
}

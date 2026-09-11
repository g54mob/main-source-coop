using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib.Internal.Util;
using HarmonyLib.Tools;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using MonoMod.Utils.Cil;

namespace HarmonyLib.Internal.Patching
{
	internal class ILManipulator
	{
		private class RawInstruction
		{
			public CodeInstruction Instruction { get; set; }

			public object Operand { get; set; }

			public Instruction CILInstruction { get; set; }
		}

		private static readonly Dictionary<short, System.Reflection.Emit.OpCode> SREOpCodes;

		private static readonly Dictionary<short, Mono.Cecil.Cil.OpCode> CecilOpCodes;

		private static readonly Dictionary<System.Reflection.Emit.OpCode, System.Reflection.Emit.OpCode> ShortToLongMap;

		private readonly IEnumerable<RawInstruction> codeInstructions;

		private readonly bool debug;

		private readonly Dictionary<VariableDefinition, LocalBuilder> localsCache = new Dictionary<VariableDefinition, LocalBuilder>();

		private readonly List<MethodInfo> transpilers = new List<MethodInfo>();

		public Mono.Cecil.Cil.MethodBody Body { get; }

		static ILManipulator()
		{
			SREOpCodes = new Dictionary<short, System.Reflection.Emit.OpCode>();
			CecilOpCodes = new Dictionary<short, Mono.Cecil.Cil.OpCode>();
			ShortToLongMap = new Dictionary<System.Reflection.Emit.OpCode, System.Reflection.Emit.OpCode>
			{
				[System.Reflection.Emit.OpCodes.Beq_S] = System.Reflection.Emit.OpCodes.Beq,
				[System.Reflection.Emit.OpCodes.Bge_S] = System.Reflection.Emit.OpCodes.Bge,
				[System.Reflection.Emit.OpCodes.Bge_Un_S] = System.Reflection.Emit.OpCodes.Bge_Un,
				[System.Reflection.Emit.OpCodes.Bgt_S] = System.Reflection.Emit.OpCodes.Bgt,
				[System.Reflection.Emit.OpCodes.Bgt_Un_S] = System.Reflection.Emit.OpCodes.Bgt_Un,
				[System.Reflection.Emit.OpCodes.Ble_S] = System.Reflection.Emit.OpCodes.Ble,
				[System.Reflection.Emit.OpCodes.Ble_Un_S] = System.Reflection.Emit.OpCodes.Ble_Un,
				[System.Reflection.Emit.OpCodes.Blt_S] = System.Reflection.Emit.OpCodes.Blt,
				[System.Reflection.Emit.OpCodes.Blt_Un_S] = System.Reflection.Emit.OpCodes.Blt_Un,
				[System.Reflection.Emit.OpCodes.Bne_Un_S] = System.Reflection.Emit.OpCodes.Bne_Un,
				[System.Reflection.Emit.OpCodes.Brfalse_S] = System.Reflection.Emit.OpCodes.Brfalse,
				[System.Reflection.Emit.OpCodes.Brtrue_S] = System.Reflection.Emit.OpCodes.Brtrue,
				[System.Reflection.Emit.OpCodes.Br_S] = System.Reflection.Emit.OpCodes.Br,
				[System.Reflection.Emit.OpCodes.Leave_S] = System.Reflection.Emit.OpCodes.Leave
			};
			FieldInfo[] fields = typeof(System.Reflection.Emit.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				System.Reflection.Emit.OpCode value = (System.Reflection.Emit.OpCode)fields[i].GetValue(null);
				SREOpCodes[value.Value] = value;
			}
			fields = typeof(Mono.Cecil.Cil.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				Mono.Cecil.Cil.OpCode value2 = (Mono.Cecil.Cil.OpCode)fields[i].GetValue(null);
				CecilOpCodes[value2.Value] = value2;
			}
		}

		public ILManipulator(Mono.Cecil.Cil.MethodBody body, bool debug)
		{
			Body = body;
			this.debug = debug;
			codeInstructions = ReadBody(Body);
		}

		private int GetTarget(Mono.Cecil.Cil.MethodBody body, object insOp)
		{
			if (insOp is ILLabel iLLabel)
			{
				return body.Instructions.IndexOf(iLLabel.Target);
			}
			if (insOp is Instruction item)
			{
				return body.Instructions.IndexOf(item);
			}
			return -1;
		}

		private int[] GetTargets(Mono.Cecil.Cil.MethodBody body, object insOp)
		{
			if (insOp is ILLabel[] arr)
			{
				return Result<ILLabel>(arr, (ILLabel l) => l.Target);
			}
			if (insOp is Instruction[] arr2)
			{
				return Result<Instruction>(arr2, (Instruction i) => i);
			}
			return new int[0];
			int[] Result<T>(IEnumerable<T> source, Func<T, Instruction> insGetter)
			{
				return source.Select((T i) => body.Instructions.IndexOf(insGetter(i))).ToArray();
			}
		}

		private IEnumerable<RawInstruction> ReadBody(Mono.Cecil.Cil.MethodBody body)
		{
			List<RawInstruction> instructions = new List<RawInstruction>(body.Instructions.Count);
			instructions.AddRange(body.Instructions.Select(ReadInstruction));
			foreach (RawInstruction item in instructions)
			{
				RawInstruction rawInstruction = item;
				rawInstruction.Operand = item.Instruction.opcode.OperandType switch
				{
					System.Reflection.Emit.OperandType.ShortInlineBrTarget => instructions[(int)item.Operand].Instruction, 
					System.Reflection.Emit.OperandType.InlineBrTarget => instructions[(int)item.Operand].Instruction, 
					System.Reflection.Emit.OperandType.InlineSwitch => ((int[])item.Operand).Select((int i) => instructions[i].Instruction).ToArray(), 
					_ => item.Operand, 
				};
			}
			foreach (Mono.Cecil.Cil.ExceptionHandler exceptionHandler in body.ExceptionHandlers)
			{
				CodeInstruction instruction = instructions[body.Instructions.IndexOf(exceptionHandler.TryStart)].Instruction;
				_ = instructions[body.Instructions.IndexOf(exceptionHandler.TryEnd)].Instruction;
				CodeInstruction instruction2 = instructions[body.Instructions.IndexOf(exceptionHandler.HandlerStart)].Instruction;
				int index = ((exceptionHandler.HandlerEnd == null) ? (instructions.Count - 1) : body.Instructions.IndexOf(exceptionHandler.HandlerEnd.Previous));
				CodeInstruction instruction3 = instructions[index].Instruction;
				instruction.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));
				instruction3.blocks.Add(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock));
				switch (exceptionHandler.HandlerType)
				{
				case ExceptionHandlerType.Catch:
					instruction2.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, exceptionHandler.CatchType.ResolveReflection()));
					break;
				case ExceptionHandlerType.Filter:
					instructions[body.Instructions.IndexOf(exceptionHandler.FilterStart)].Instruction.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptFilterBlock));
					break;
				case ExceptionHandlerType.Finally:
					instruction2.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFinallyBlock));
					break;
				case ExceptionHandlerType.Fault:
					instruction2.blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFaultBlock));
					break;
				}
			}
			return instructions;
			RawInstruction ReadInstruction(Instruction ins)
			{
				RawInstruction rawInstruction2 = new RawInstruction
				{
					Instruction = new CodeInstruction(SREOpCodes[ins.OpCode.Value])
				};
				RawInstruction rawInstruction3 = rawInstruction2;
				rawInstruction3.Operand = ins.OpCode.OperandType switch
				{
					Mono.Cecil.Cil.OperandType.InlineField => ((MemberReference)ins.Operand).ResolveReflection(), 
					Mono.Cecil.Cil.OperandType.InlineMethod => ((MemberReference)ins.Operand).ResolveReflection(), 
					Mono.Cecil.Cil.OperandType.InlineType => ((MemberReference)ins.Operand).ResolveReflection(), 
					Mono.Cecil.Cil.OperandType.InlineTok => ((MemberReference)ins.Operand).ResolveReflection(), 
					Mono.Cecil.Cil.OperandType.InlineVar => (VariableDefinition)ins.Operand, 
					Mono.Cecil.Cil.OperandType.ShortInlineVar => (VariableDefinition)ins.Operand, 
					Mono.Cecil.Cil.OperandType.InlineArg => (short)((ParameterDefinition)ins.Operand).Index, 
					Mono.Cecil.Cil.OperandType.ShortInlineArg => (byte)((ParameterDefinition)ins.Operand).Index, 
					Mono.Cecil.Cil.OperandType.InlineBrTarget => GetTarget(body, ins.Operand), 
					Mono.Cecil.Cil.OperandType.ShortInlineBrTarget => GetTarget(body, ins.Operand), 
					Mono.Cecil.Cil.OperandType.InlineSwitch => GetTargets(body, ins.Operand), 
					_ => ins.Operand, 
				};
				rawInstruction2.CILInstruction = ins;
				return rawInstruction2;
			}
		}

		public void AddTranspiler(MethodInfo transpiler)
		{
			transpilers.Add(transpiler);
		}

		private object[] GetTranspilerArguments(ILGenerator il, MethodInfo transpiler, IEnumerable<CodeInstruction> instructions, MethodBase orignal = null)
		{
			List<object> list = new List<object>();
			foreach (Type item in from p in transpiler.GetParameters()
				select p.ParameterType)
			{
				if (item.IsAssignableFrom(typeof(ILGenerator)))
				{
					list.Add(il);
				}
				else if (item.IsAssignableFrom(typeof(MethodBase)) && (object)orignal != null)
				{
					list.Add(orignal);
				}
				else if (item.IsAssignableFrom(typeof(IEnumerable<CodeInstruction>)))
				{
					list.Add(instructions);
				}
			}
			return list.ToArray();
		}

		public IEnumerable<KeyValuePair<System.Reflection.Emit.OpCode, object>> GetRawInstructions()
		{
			return codeInstructions.Select((RawInstruction i) => new KeyValuePair<System.Reflection.Emit.OpCode, object>(i.Instruction.opcode, i.Operand));
		}

		public List<CodeInstruction> GetInstructions(ILGenerator il, MethodBase original = null)
		{
			return NormalizeInstructions(ApplyTranspilers(il, original, (VariableDefinition vDef) => il.DeclareLocal(vDef.VariableType.ResolveReflection()), il.DefineLabel)).ToList();
		}

		private IEnumerable<CodeInstruction> ApplyTranspilers(ILGenerator il, MethodBase original, Func<VariableDefinition, LocalBuilder> getLocal, Func<Label> defineLabel)
		{
			List<CodeInstruction> list = (from i in Prepare(getLocal, defineLabel)
				select i.Instruction).ToList();
			if (transpilers.Count == 0)
			{
				return list;
			}
			IEnumerable<CodeInstruction> enumerable = NormalizeInstructions(list);
			foreach (MethodInfo transpiler in transpilers)
			{
				object[] transpilerArguments = GetTranspilerArguments(il, transpiler, enumerable, original);
				Logger.Log(Logger.LogChannel.Info, () => "Running transpiler " + transpiler.FullDescription(), debug);
				enumerable = NormalizeInstructions(transpiler.Invoke(null, transpilerArguments) as IEnumerable<CodeInstruction>).ToList();
			}
			return enumerable;
		}

		public Dictionary<int, CodeInstruction> GetIndexedInstructions(ILGenerator il)
		{
			int size = 0;
			return Prepare((VariableDefinition vDef) => il.DeclareLocal(vDef.VariableType.ResolveReflection()), il.DefineLabel).ToDictionary((RawInstruction i) => Grow(ref size, i.CILInstruction.GetSize()), (RawInstruction i) => i.Instruction);
			static int Grow(ref int i, int s)
			{
				int result = i;
				i += s;
				return result;
			}
		}

		private IEnumerable<RawInstruction> Prepare(Func<VariableDefinition, LocalBuilder> getLocal, Func<Label> defineLabel)
		{
			localsCache.Clear();
			foreach (VariableDefinition variable in Body.Variables)
			{
				localsCache[variable] = getLocal(variable);
			}
			foreach (RawInstruction codeInstruction2 in codeInstructions)
			{
				codeInstruction2.Instruction.operand = codeInstruction2.Operand;
				switch (codeInstruction2.Instruction.opcode.OperandType)
				{
				case System.Reflection.Emit.OperandType.InlineVar:
				case System.Reflection.Emit.OperandType.ShortInlineVar:
					if (codeInstruction2.Operand is VariableDefinition key)
					{
						codeInstruction2.Instruction.operand = localsCache[key];
					}
					break;
				case System.Reflection.Emit.OperandType.InlineSwitch:
					if (codeInstruction2.Operand is CodeInstruction[] array)
					{
						List<Label> list = new List<Label>();
						CodeInstruction[] array2 = array;
						foreach (CodeInstruction obj in array2)
						{
							Label item = defineLabel();
							obj.labels.Add(item);
							list.Add(item);
						}
						codeInstruction2.Instruction.operand = list.ToArray();
					}
					break;
				case System.Reflection.Emit.OperandType.InlineBrTarget:
				case System.Reflection.Emit.OperandType.ShortInlineBrTarget:
					if (codeInstruction2.Instruction.operand is CodeInstruction codeInstruction)
					{
						Label label = defineLabel();
						codeInstruction.labels.Add(label);
						codeInstruction2.Instruction.operand = label;
					}
					break;
				}
			}
			return codeInstructions;
		}

		public void WriteTo(Mono.Cecil.Cil.MethodBody body, MethodBase original = null)
		{
			body.Instructions.Clear();
			body.ExceptionHandlers.Clear();
			CecilILGenerator il = new CecilILGenerator(body.GetILProcessor());
			ILGenerator proxy = il.GetProxy();
			il.DefineLabel();
			foreach (var (codeInstruction, codeInstruction2) in ApplyTranspilers(proxy, original, (VariableDefinition vDef) => il.GetLocal(vDef), il.DefineLabel).Pairwise())
			{
				codeInstruction.labels.ForEach(delegate(Label l)
				{
					il.MarkLabel(l);
				});
				codeInstruction.blocks.ForEach(delegate(ExceptionBlock b)
				{
					il.MarkBlockBefore(b);
				});
				if (((!(codeInstruction.opcode == System.Reflection.Emit.OpCodes.Leave) && !(codeInstruction.opcode == System.Reflection.Emit.OpCodes.Leave_S)) || (codeInstruction.blocks.Count <= 0 && (codeInstruction2 == null || codeInstruction2.blocks.Count <= 0))) && ((!(codeInstruction.opcode == System.Reflection.Emit.OpCodes.Endfilter) && !(codeInstruction.opcode == System.Reflection.Emit.OpCodes.Endfinally)) || codeInstruction.blocks.Count <= 0))
				{
					switch (codeInstruction.opcode.OperandType)
					{
					case System.Reflection.Emit.OperandType.InlineNone:
						il.Emit(codeInstruction.opcode);
						break;
					case System.Reflection.Emit.OperandType.InlineSig:
						throw new NotSupportedException("Emitting opcodes with CallSites is currently not fully implemented");
					default:
						if (codeInstruction.operand == null)
						{
							throw new ArgumentNullException("operand", $"Invalid argument for {codeInstruction}");
						}
						il.Emit(codeInstruction.opcode, codeInstruction.operand);
						break;
					}
				}
				codeInstruction.blocks.ForEach(delegate(ExceptionBlock b)
				{
					il.MarkBlockAfter(b);
				});
			}
			if (body.Instructions.Count == 0)
			{
				il.Emit(System.Reflection.Emit.OpCodes.Ret);
			}
		}

		private static IEnumerable<CodeInstruction> NormalizeInstructions(IEnumerable<CodeInstruction> instrs)
		{
			foreach (CodeInstruction instr in instrs)
			{
				CodeInstruction codeInstruction = instr;
				if (codeInstruction.labels == null)
				{
					codeInstruction.labels = new List<Label>();
				}
				codeInstruction = instr;
				if (codeInstruction.blocks == null)
				{
					codeInstruction.blocks = new List<ExceptionBlock>();
				}
				if (ShortToLongMap.TryGetValue(instr.opcode, out var value))
				{
					instr.opcode = value;
				}
				yield return instr;
			}
		}

		public static Dictionary<int, CodeInstruction> GetInstructions(Mono.Cecil.Cil.MethodBody body)
		{
			if (body == null)
			{
				return null;
			}
			try
			{
				return new ILManipulator(body, debug: false).GetIndexedInstructions(PatchProcessor.CreateILGenerator());
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Logger.Log(Logger.LogChannel.Warn, () => "Could not read instructions of " + body.Method.GetID() + ": " + e.Message);
				return null;
			}
		}
	}
}

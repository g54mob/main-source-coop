using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Mono.Cecil;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	public class DetourRuntimeNETCore30Platform : DetourRuntimeNETCorePlatform
	{
		private enum CorJitResult
		{
			CORJIT_OK = 0
		}

		private struct CORINFO_SIG_INST
		{
			public uint classInstCount;

			public unsafe IntPtr* classInst;

			public uint methInstCount;

			public unsafe IntPtr* methInst;
		}

		private struct CORINFO_SIG_INFO
		{
			public int callConv;

			public IntPtr retTypeClass;

			public IntPtr retTypeSigClass;

			public byte retType;

			public byte flags;

			public ushort numArgs;

			public CORINFO_SIG_INST sigInst;

			public IntPtr args;

			public IntPtr pSig;

			public uint sbSig;

			public IntPtr scope;

			public uint token;
		}

		private struct CORINFO_METHOD_INFO
		{
			public IntPtr ftn;

			public IntPtr scope;

			public unsafe byte* ILCode;

			public uint ILCodeSize;

			public uint maxStack;

			public uint EHcount;

			public int options;

			public int regionKind;

			public CORINFO_SIG_INFO args;

			public CORINFO_SIG_INFO locals;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate CorJitResult d_compileMethod(IntPtr thisPtr, IntPtr corJitInfo, in CORINFO_METHOD_INFO methodInfo, uint flags, out byte* nativeEntry, out ulong nativeSizeOfCode);

		protected delegate object d_MethodHandle_GetLoaderAllocator(IntPtr methodHandle);

		protected delegate object d_CreateRuntimeMethodInfoStub(IntPtr methodHandle, object loaderAllocator);

		protected delegate RuntimeMethodHandle d_CreateRuntimeMethodHandle(object runtimeMethodInfo);

		protected delegate Type d_GetDeclaringTypeOfMethodHandle(IntPtr methodHandle);

		protected delegate Type d_GetTypeFromNativeHandle(IntPtr handle);

		public static readonly Guid JitVersionGuid = new Guid("d609bed1-7831-49fc-bd49-b6f054dd4d46");

		private d_compileMethod our_compileMethod;

		private IntPtr real_compileMethodPtr;

		private d_compileMethod real_compileMethod;

		[ThreadStatic]
		private static int hookEntrancy = 0;

		protected d_MethodHandle_GetLoaderAllocator MethodHandle_GetLoaderAllocator;

		protected d_CreateRuntimeMethodInfoStub CreateRuntimeMethodInfoStub;

		protected d_CreateRuntimeMethodHandle CreateRuntimeMethodHandle;

		protected d_GetDeclaringTypeOfMethodHandle GetDeclaringTypeOfMethodHandle;

		protected d_GetTypeFromNativeHandle GetTypeFromNativeHandle;

		private MethodInfo _getTypeFromHandleUnsafeMethod;

		private static FieldInfo _runtimeAssemblyPtrField = Type.GetType("System.Reflection.RuntimeAssembly").GetField("m_assembly", BindingFlags.Instance | BindingFlags.NonPublic);

		public override bool OnMethodCompiledWillBeCalled => true;

		protected unsafe override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
			ushort* ptr = (ushort*)(void*)handle.Value + 3;
			*ptr |= 0x2000;
		}

		private d_compileMethod GetCompileMethod(IntPtr jit)
		{
			return DetourRuntimeNETCorePlatform.ReadObjectVTable(jit, VTableIndex_ICorJitCompiler_compileMethod).AsDelegate<d_compileMethod>();
		}

		protected unsafe override void InstallJitHooks(IntPtr jit)
		{
			SetupJitHookHelpers();
			real_compileMethod = GetCompileMethod(jit);
			our_compileMethod = CompileMethodHook;
			IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate((Delegate)our_compileMethod);
			NativeDetourData data = CreateNativeTrampolineTo(functionPointerForDelegate);
			data.Method.AsDelegate<d_compileMethod>()(IntPtr.Zero, IntPtr.Zero, default(CORINFO_METHOD_INFO), 0u, out var _, out var _);
			FreeNativeTrampoline(data);
			_ = hookEntrancy;
			IntPtr* vTableEntry = DetourRuntimeNETCorePlatform.GetVTableEntry(jit, VTableIndex_ICorJitCompiler_compileMethod);
			DetourHelper.Native.MakeWritable((IntPtr)vTableEntry, (uint)IntPtr.Size);
			real_compileMethodPtr = *vTableEntry;
			*vTableEntry = functionPointerForDelegate;
		}

		private static NativeDetourData CreateNativeTrampolineTo(IntPtr target)
		{
			IntPtr intPtr = DetourHelper.Native.MemAlloc(64u);
			NativeDetourData nativeDetourData = DetourHelper.Native.Create(intPtr, target);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			return nativeDetourData;
		}

		private static void FreeNativeTrampoline(NativeDetourData data)
		{
			DetourHelper.Native.MakeWritable(data);
			DetourHelper.Native.MemFree(data.Method);
			DetourHelper.Native.Free(data);
		}

		private unsafe CorJitResult CompileMethodHook(IntPtr jit, IntPtr corJitInfo, in CORINFO_METHOD_INFO methodInfo, uint flags, out byte* nativeEntry, out ulong nativeSizeOfCode)
		{
			nativeEntry = null;
			nativeSizeOfCode = 0uL;
			if (jit == IntPtr.Zero)
			{
				return CorJitResult.CORJIT_OK;
			}
			hookEntrancy++;
			try
			{
				CorJitResult result = real_compileMethod(jit, corJitInfo, in methodInfo, flags, out nativeEntry, out nativeSizeOfCode);
				if (hookEntrancy == 1)
				{
					try
					{
						RuntimeTypeHandle[] array = null;
						RuntimeTypeHandle[] array2 = null;
						if (methodInfo.args.sigInst.classInst != null)
						{
							array = new RuntimeTypeHandle[methodInfo.args.sigInst.classInstCount];
							for (int i = 0; i < array.Length; i++)
							{
								array[i] = GetTypeFromNativeHandle(methodInfo.args.sigInst.classInst[i]).TypeHandle;
							}
						}
						if (methodInfo.args.sigInst.methInst != null)
						{
							array2 = new RuntimeTypeHandle[methodInfo.args.sigInst.methInstCount];
							for (int j = 0; j < array2.Length; j++)
							{
								array2[j] = GetTypeFromNativeHandle(methodInfo.args.sigInst.methInst[j]).TypeHandle;
							}
						}
						RuntimeTypeHandle typeHandle = GetDeclaringTypeOfMethodHandle(methodInfo.ftn).TypeHandle;
						RuntimeMethodHandle methodHandle = CreateHandleForHandlePointer(methodInfo.ftn);
						JitHookCore(typeHandle, methodHandle, (IntPtr)nativeEntry, nativeSizeOfCode, array, array2);
					}
					catch
					{
					}
				}
				return result;
			}
			finally
			{
				hookEntrancy--;
			}
		}

		protected RuntimeMethodHandle CreateHandleForHandlePointer(IntPtr handle)
		{
			return CreateRuntimeMethodHandle(CreateRuntimeMethodInfoStub(handle, MethodHandle_GetLoaderAllocator(handle)));
		}

		protected virtual void SetupJitHookHelpers()
		{
			MethodInfo methodInfo = typeof(object).Assembly.GetType("Internal.Runtime.CompilerServices.Unsafe").GetMethods().First((MethodInfo m) => m.Name == "As" && m.ReturnType.IsByRef);
			MethodInfo method = typeof(RuntimeMethodHandle).GetMethod("GetLoaderAllocator", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo method2;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("MethodHandle_GetLoaderAllocator", typeof(object), new Type[1] { typeof(IntPtr) }))
			{
				ILGenerator iLGenerator = dynamicMethodDefinition.GetILGenerator();
				Type parameterType = method.GetParameters().First().ParameterType;
				iLGenerator.Emit(OpCodes.Ldarga_S, 0);
				iLGenerator.Emit(OpCodes.Call, methodInfo.MakeGenericMethod(typeof(IntPtr), parameterType));
				iLGenerator.Emit(OpCodes.Ldobj, parameterType);
				iLGenerator.Emit(OpCodes.Call, method);
				iLGenerator.Emit(OpCodes.Ret);
				method2 = dynamicMethodDefinition.Generate();
			}
			MethodHandle_GetLoaderAllocator = method2.CreateDelegate<d_MethodHandle_GetLoaderAllocator>();
			MethodInfo orCreateGetTypeFromHandleUnsafe = GetOrCreateGetTypeFromHandleUnsafe();
			GetTypeFromNativeHandle = orCreateGetTypeFromHandleUnsafe.CreateDelegate<d_GetTypeFromNativeHandle>();
			Type type = typeof(RuntimeMethodHandle).Assembly.GetType("System.RuntimeMethodHandleInternal");
			MethodInfo method3 = typeof(RuntimeMethodHandle).GetMethod("GetDeclaringType", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[1] { type }, null);
			MethodInfo method4;
			using (DynamicMethodDefinition dynamicMethodDefinition2 = new DynamicMethodDefinition("GetDeclaringTypeOfMethodHandle", typeof(Type), new Type[1] { typeof(IntPtr) }))
			{
				ILGenerator iLGenerator2 = dynamicMethodDefinition2.GetILGenerator();
				iLGenerator2.Emit(OpCodes.Ldarga_S, 0);
				iLGenerator2.Emit(OpCodes.Call, methodInfo.MakeGenericMethod(typeof(IntPtr), type));
				iLGenerator2.Emit(OpCodes.Ldobj, type);
				iLGenerator2.Emit(OpCodes.Call, method3);
				iLGenerator2.Emit(OpCodes.Ret);
				method4 = dynamicMethodDefinition2.Generate();
			}
			GetDeclaringTypeOfMethodHandle = method4.CreateDelegate<d_GetDeclaringTypeOfMethodHandle>();
			Type[] array = new Type[2]
			{
				typeof(IntPtr),
				typeof(object)
			};
			Type type2 = typeof(RuntimeMethodHandle).Assembly.GetType("System.RuntimeMethodInfoStub");
			ConstructorInfo constructor = type2.GetConstructor(array);
			MethodInfo method5;
			using (DynamicMethodDefinition dynamicMethodDefinition3 = new DynamicMethodDefinition("new RuntimeMethodInfoStub", type2, array))
			{
				ILGenerator iLGenerator3 = dynamicMethodDefinition3.GetILGenerator();
				iLGenerator3.Emit(OpCodes.Ldarg_0);
				iLGenerator3.Emit(OpCodes.Ldarg_1);
				iLGenerator3.Emit(OpCodes.Newobj, constructor);
				iLGenerator3.Emit(OpCodes.Ret);
				method5 = dynamicMethodDefinition3.Generate();
			}
			CreateRuntimeMethodInfoStub = method5.CreateDelegate<d_CreateRuntimeMethodInfoStub>();
			ConstructorInfo con = typeof(RuntimeMethodHandle).GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).First();
			MethodInfo method6;
			using (DynamicMethodDefinition dynamicMethodDefinition4 = new DynamicMethodDefinition("new RuntimeMethodHandle", typeof(RuntimeMethodHandle), new Type[1] { typeof(object) }))
			{
				ILGenerator iLGenerator4 = dynamicMethodDefinition4.GetILGenerator();
				iLGenerator4.Emit(OpCodes.Ldarg_0);
				iLGenerator4.Emit(OpCodes.Newobj, con);
				iLGenerator4.Emit(OpCodes.Ret);
				method6 = dynamicMethodDefinition4.Generate();
			}
			CreateRuntimeMethodHandle = method6.CreateDelegate<d_CreateRuntimeMethodHandle>();
		}

		private MethodInfo GetOrCreateGetTypeFromHandleUnsafe()
		{
			if ((object)_getTypeFromHandleUnsafeMethod != null)
			{
				return _getTypeFromHandleUnsafeMethod;
			}
			Assembly assembly;
			using (ModuleDefinition moduleDefinition = ModuleDefinition.CreateModule("MonoMod.RuntimeDetour.Runtime.NETCore3+Helpers", new ModuleParameters
			{
				Kind = ModuleKind.Dll
			}))
			{
				TypeDefinition typeDefinition = new TypeDefinition("System", "Type", Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract)
				{
					BaseType = moduleDefinition.TypeSystem.Object
				};
				moduleDefinition.Types.Add(typeDefinition);
				MethodDefinition methodDefinition = new MethodDefinition("GetTypeFromHandleUnsafe", Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Static, moduleDefinition.ImportReference(typeof(Type)))
				{
					IsInternalCall = true
				};
				methodDefinition.Parameters.Add(new ParameterDefinition(moduleDefinition.ImportReference(typeof(IntPtr))));
				typeDefinition.Methods.Add(methodDefinition);
				assembly = ReflectionHelper.Load(moduleDefinition);
			}
			MakeAssemblySystemAssembly(assembly);
			return _getTypeFromHandleUnsafeMethod = assembly.GetType("System.Type").GetMethod("GetTypeFromHandleUnsafe");
		}

		protected unsafe virtual void MakeAssemblySystemAssembly(Assembly assembly)
		{
			IntPtr intPtr = (IntPtr)_runtimeAssemblyPtrField.GetValue(assembly);
			int num = IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size + 4 + IntPtr.Size + IntPtr.Size + 4 + 4 + IntPtr.Size + IntPtr.Size + 4 + 4 + IntPtr.Size;
			if (IntPtr.Size == 8)
			{
				num += 4;
			}
			IntPtr intPtr2 = *(IntPtr*)((byte*)(void*)intPtr + num);
			int num2 = IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size;
			IntPtr intPtr3 = *(IntPtr*)((byte*)(void*)intPtr2 + num2);
			int num3 = IntPtr.Size + IntPtr.Size + IntPtr.Size + 4 + 4 + IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size + 4;
			int* ptr = (int*)((byte*)(void*)intPtr3 + num3);
			*ptr |= 1;
		}

		protected void HookPermanent(MethodBase from, MethodBase to)
		{
			Pin(from);
			Pin(to);
			HookPermanent(GetNativeStart(from), GetNativeStart(to));
		}

		protected void HookPermanent(IntPtr from, IntPtr to)
		{
			NativeDetourData detour = DetourHelper.Native.Create(from, to);
			DetourHelper.Native.MakeWritable(detour);
			DetourHelper.Native.Apply(detour);
			DetourHelper.Native.MakeExecutable(detour);
			DetourHelper.Native.FlushICache(detour);
			DetourHelper.Native.Free(detour);
		}
	}
}

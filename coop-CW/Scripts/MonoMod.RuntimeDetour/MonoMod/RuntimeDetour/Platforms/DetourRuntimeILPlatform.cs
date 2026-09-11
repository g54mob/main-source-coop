using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	public abstract class DetourRuntimeILPlatform : IDetourRuntimePlatform
	{
		private struct _SelftestStruct
		{
			private readonly short Value;

			private readonly byte E1;

			private readonly byte E2;

			private readonly byte E3;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public short _SelftestGetInStruct()
			{
				Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
				return -1;
			}
		}

		protected class PrivateMethodPin
		{
			public MethodPinInfo Pin;
		}

		public struct MethodPinInfo
		{
			public int Count;

			public MethodBase Method;

			public RuntimeMethodHandle Handle;

			public override string ToString()
			{
				return $"(MethodPinInfo: {Count}, {Method}, 0x{(long)Handle.Value:X})";
			}
		}

		protected enum GlueThiscallStructRetPtrOrder
		{
			Original = 0,
			ThisRetArgs = 1,
			RetThisArgs = 2
		}

		protected GlueThiscallStructRetPtrOrder GlueThiscallStructRetPtr;

		protected GlueThiscallStructRetPtrOrder GlueThiscallInStructRetPtr;

		protected Dictionary<MethodBase, PrivateMethodPin> PinnedMethods = new Dictionary<MethodBase, PrivateMethodPin>();

		protected Dictionary<RuntimeMethodHandle, PrivateMethodPin> PinnedHandles = new Dictionary<RuntimeMethodHandle, PrivateMethodPin>();

		private IntPtr ReferenceNonDynamicPoolPtr;

		private IntPtr ReferenceDynamicPoolPtr;

		protected static readonly uint _MemAllocScratchDummySafeSize = 16u;

		protected static readonly MethodInfo _MemAllocScratchDummy = typeof(DetourRuntimeILPlatform).GetMethod("MemAllocScratchDummy", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

		public abstract bool OnMethodCompiledWillBeCalled { get; }

		public abstract event OnMethodCompiledEvent OnMethodCompiled;

		protected abstract RuntimeMethodHandle GetMethodHandle(MethodBase method);

		public unsafe DetourRuntimeILPlatform()
		{
			MethodInfo method = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetRefPtr", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo method2 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetRefPtrHook", BindingFlags.Static | BindingFlags.NonPublic);
			_HookSelftest(method, method2);
			IntPtr arg = ((Func<IntPtr>)Delegate.CreateDelegate(typeof(Func<IntPtr>), this, method))();
			MethodInfo method3 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetStruct", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo method4 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetStructHook", BindingFlags.Static | BindingFlags.NonPublic);
			_HookSelftest(method3, method4);
			fixed (GlueThiscallStructRetPtrOrder* glueThiscallStructRetPtr = &GlueThiscallStructRetPtr)
			{
				((Func<IntPtr, IntPtr, IntPtr, _SelftestStruct>)Delegate.CreateDelegate(typeof(Func<IntPtr, IntPtr, IntPtr, _SelftestStruct>), this, method3))((IntPtr)glueThiscallStructRetPtr, (IntPtr)glueThiscallStructRetPtr, arg);
			}
			MethodInfo method5 = typeof(_SelftestStruct).GetMethod("_SelftestGetInStruct", BindingFlags.Instance | BindingFlags.Public);
			MethodInfo method6 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetInStructHook", BindingFlags.Static | BindingFlags.NonPublic);
			_HookSelftest(method5, method6);
			fixed (GlueThiscallStructRetPtrOrder* glueThiscallInStructRetPtr = &GlueThiscallInStructRetPtr)
			{
				object firstArgument = default(_SelftestStruct);
				*glueThiscallInStructRetPtr = (GlueThiscallStructRetPtrOrder)((Func<short>)Delegate.CreateDelegate(typeof(Func<short>), firstArgument, method5))();
				if (*glueThiscallInStructRetPtr == (GlueThiscallStructRetPtrOrder)(-1))
				{
					throw new Exception("_SelftestGetInStruct failed!");
				}
			}
			Pin(method);
			ReferenceNonDynamicPoolPtr = GetNativeStart(method);
			if (DynamicMethodDefinition.IsDynamicILAvailable)
			{
				MethodBase method7;
				using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(_MemAllocScratchDummy))
				{
					dynamicMethodDefinition.Name = "MemAllocScratch<Reference>";
					method7 = DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(dynamicMethodDefinition);
				}
				Pin(method7);
				ReferenceDynamicPoolPtr = GetNativeStart(method7);
			}
		}

		private void _HookSelftest(MethodInfo from, MethodInfo to)
		{
			Pin(from);
			Pin(to);
			NativeDetourData detour = DetourHelper.Native.Create(GetNativeStart(from), GetNativeStart(to));
			DetourHelper.Native.MakeWritable(detour);
			DetourHelper.Native.Apply(detour);
			DetourHelper.Native.MakeExecutable(detour);
			DetourHelper.Native.FlushICache(detour);
			DetourHelper.Native.Free(detour);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private IntPtr _SelftestGetRefPtr()
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("This method should've been detoured!");
		}

		private static IntPtr _SelftestGetRefPtrHook(IntPtr self)
		{
			return self;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private _SelftestStruct _SelftestGetStruct(IntPtr x, IntPtr y, IntPtr thisPtr)
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("_SelftestGetStruct failed!");
		}

		private unsafe static void _SelftestGetStructHook(IntPtr a, IntPtr b, IntPtr c, IntPtr d, IntPtr e)
		{
			if (b == c)
			{
				*(int*)(void*)b = 0;
			}
			else if (b == e)
			{
				*(int*)(void*)c = 2;
			}
			else
			{
				*(int*)(void*)c = 1;
			}
		}

		private unsafe static short _SelftestGetInStructHook(IntPtr a)
		{
			*(short*)(void*)a = 2;
			return 0;
		}

		protected virtual IntPtr GetFunctionPointer(MethodBase method, RuntimeMethodHandle handle)
		{
			return handle.GetFunctionPointer();
		}

		protected virtual void PrepareMethod(MethodBase method, RuntimeMethodHandle handle)
		{
			RuntimeHelpers.PrepareMethod(handle);
		}

		protected virtual void PrepareMethod(MethodBase method, RuntimeMethodHandle handle, RuntimeTypeHandle[] instantiation)
		{
			RuntimeHelpers.PrepareMethod(handle, instantiation);
		}

		protected virtual void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}

		public virtual MethodBase GetIdentifiable(MethodBase method)
		{
			lock (PinnedMethods)
			{
				PrivateMethodPin value;
				return PinnedHandles.TryGetValue(GetMethodHandle(method), out value) ? value.Pin.Method : method;
			}
		}

		public virtual MethodPinInfo GetPin(MethodBase method)
		{
			lock (PinnedMethods)
			{
				PrivateMethodPin value;
				return PinnedMethods.TryGetValue(method, out value) ? value.Pin : default(MethodPinInfo);
			}
		}

		public virtual MethodPinInfo GetPin(RuntimeMethodHandle handle)
		{
			lock (PinnedMethods)
			{
				PrivateMethodPin value;
				return PinnedHandles.TryGetValue(handle, out value) ? value.Pin : default(MethodPinInfo);
			}
		}

		public virtual MethodPinInfo[] GetPins()
		{
			lock (PinnedMethods)
			{
				return PinnedHandles.Values.Select((PrivateMethodPin p) => p.Pin).ToArray();
			}
		}

		public virtual IntPtr GetNativeStart(MethodBase method)
		{
			method = GetIdentifiable(method);
			bool flag;
			PrivateMethodPin value;
			lock (PinnedMethods)
			{
				flag = PinnedMethods.TryGetValue(method, out value);
			}
			if (flag)
			{
				return GetFunctionPointer(method, value.Pin.Handle);
			}
			return GetFunctionPointer(method, GetMethodHandle(method));
		}

		public virtual void Pin(MethodBase method)
		{
			method = GetIdentifiable(method);
			lock (PinnedMethods)
			{
				if (PinnedMethods.TryGetValue(method, out var value))
				{
					value.Pin.Count++;
					return;
				}
				MethodBase method2 = method;
				value = new PrivateMethodPin();
				value.Pin.Count = 1;
				value.Pin.Method = method2;
				RuntimeMethodHandle runtimeMethodHandle = (value.Pin.Handle = GetMethodHandle(method2));
				PinnedHandles[runtimeMethodHandle] = value;
				DisableInlining(method, runtimeMethodHandle);
				Type declaringType = method.DeclaringType;
				if ((object)declaringType != null && declaringType.IsGenericType)
				{
					PrepareMethod(method, runtimeMethodHandle, (from type in method.DeclaringType.GetGenericArguments()
						select type.TypeHandle).ToArray());
				}
				else
				{
					PrepareMethod(method, runtimeMethodHandle);
				}
			}
		}

		public virtual void Unpin(MethodBase method)
		{
			method = GetIdentifiable(method);
			lock (PinnedMethods)
			{
				if (PinnedMethods.TryGetValue(method, out var value))
				{
					if (value.Pin.Count <= 1)
					{
						PinnedMethods.Remove(method);
						PinnedHandles.Remove(value.Pin.Handle);
					}
					else
					{
						value.Pin.Count--;
					}
				}
			}
		}

		public MethodInfo CreateCopy(MethodBase method)
		{
			method = GetIdentifiable(method);
			if ((object)method == null || (method.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL)
			{
				throw new InvalidOperationException("Uncopyable method: " + (method?.ToString() ?? "NULL"));
			}
			using DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(method);
			return dynamicMethodDefinition.Generate();
		}

		public bool TryCreateCopy(MethodBase method, out MethodInfo dm)
		{
			method = GetIdentifiable(method);
			if ((object)method == null || (method.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL)
			{
				dm = null;
				return false;
			}
			try
			{
				dm = CreateCopy(method);
				return true;
			}
			catch
			{
				dm = null;
				return false;
			}
		}

		public MethodBase GetDetourTarget(MethodBase from, MethodBase to)
		{
			to = GetIdentifiable(to);
			MethodInfo methodInfo = null;
			if (from is MethodInfo methodInfo2 && !from.IsStatic && to is MethodInfo methodInfo3 && to.IsStatic && (object)methodInfo2.ReturnType == methodInfo3.ReturnType && methodInfo2.ReturnType.IsValueType)
			{
				Type declaringType = from.DeclaringType;
				GlueThiscallStructRetPtrOrder glueThiscallStructRetPtrOrder;
				if ((glueThiscallStructRetPtrOrder = (((object)declaringType != null && declaringType.IsValueType) ? GlueThiscallInStructRetPtr : GlueThiscallStructRetPtr)) != GlueThiscallStructRetPtrOrder.Original)
				{
					int managedSize = methodInfo2.ReturnType.GetManagedSize();
					if (managedSize == 3 || managedSize == 5 || managedSize == 6 || managedSize == 7 || managedSize > IntPtr.Size)
					{
						Type thisParamType = from.GetThisParamType();
						Type item = methodInfo2.ReturnType.MakeByRefType();
						int value = 0;
						int num = 1;
						if (glueThiscallStructRetPtrOrder == GlueThiscallStructRetPtrOrder.RetThisArgs)
						{
							value = 1;
							num = 0;
						}
						List<Type> list = new List<Type> { thisParamType };
						list.Insert(num, item);
						list.AddRange(from p in @from.GetParameters()
							select p.ParameterType);
						using DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("Glue:ThiscallStructRetPtr<" + from.GetID(null, null, withType: true, proxyMethod: false, simple: true) + "," + to.GetID(null, null, withType: true, proxyMethod: false, simple: true) + ">", typeof(void), list.ToArray());
						ILProcessor iLProcessor = dynamicMethodDefinition.GetILProcessor();
						iLProcessor.Emit(OpCodes.Ldarg, num);
						iLProcessor.Emit(OpCodes.Ldarg, value);
						for (int num2 = 2; num2 < list.Count; num2++)
						{
							iLProcessor.Emit(OpCodes.Ldarg, num2);
						}
						iLProcessor.Emit(OpCodes.Call, iLProcessor.Body.Method.Module.ImportReference(to));
						iLProcessor.Emit(OpCodes.Stobj, iLProcessor.Body.Method.Module.ImportReference(methodInfo2.ReturnType));
						iLProcessor.Emit(OpCodes.Ret);
						methodInfo = dynamicMethodDefinition.Generate();
					}
				}
			}
			return methodInfo ?? to;
		}

		public uint TryMemAllocScratchCloseTo(IntPtr target, out IntPtr ptr, int size)
		{
			if (size == 0 || size > _MemAllocScratchDummySafeSize)
			{
				ptr = IntPtr.Zero;
				return 0u;
			}
			bool num = Math.Abs((long)target - (long)ReferenceNonDynamicPoolPtr) < 1073741824;
			bool flag = DynamicMethodDefinition.IsDynamicILAvailable && Math.Abs((long)target - (long)ReferenceDynamicPoolPtr) < 1073741824;
			if (!num && !flag)
			{
				ptr = IntPtr.Zero;
				return 0u;
			}
			MethodBase method;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(_MemAllocScratchDummy))
			{
				dynamicMethodDefinition.Name = $"MemAllocScratch<{(long)target:X16}>";
				method = ((!flag) ? DMDGenerator<DMDCecilGenerator>.Generate(dynamicMethodDefinition) : DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(dynamicMethodDefinition));
			}
			Pin(method);
			ptr = GetNativeStart(method);
			DetourHelper.Native.MakeReadWriteExecutable(ptr, _MemAllocScratchDummySafeSize);
			return _MemAllocScratchDummySafeSize;
		}

		public static int MemAllocScratchDummy(int a, int b)
		{
			if (a >= 1024 && b >= 1024)
			{
				return a + b;
			}
			return MemAllocScratchDummy(a + b, b + 1);
		}
	}
}

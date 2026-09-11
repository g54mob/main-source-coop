using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	public class DetourRuntimeNETPlatform : DetourRuntimeILPlatform
	{
		private static readonly object[] _NoArgs = new object[0];

		private static readonly Type _RTDynamicMethod = typeof(DynamicMethod).GetNestedType("RTDynamicMethod", BindingFlags.NonPublic);

		private static readonly FieldInfo _RTDynamicMethod_m_owner = _RTDynamicMethod?.GetField("m_owner", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		private static readonly FieldInfo _DynamicMethod_m_method = typeof(DynamicMethod).GetField("m_method", BindingFlags.Instance | BindingFlags.NonPublic);

		private static readonly MethodInfo _DynamicMethod_GetMethodDescriptor = typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.Instance | BindingFlags.NonPublic);

		private static readonly FieldInfo _RuntimeMethodHandle_m_value = typeof(RuntimeMethodHandle).GetField("m_value", BindingFlags.Instance | BindingFlags.NonPublic);

		private static readonly MethodInfo _RuntimeHelpers__CompileMethod = typeof(RuntimeHelpers).GetMethod("_CompileMethod", BindingFlags.Static | BindingFlags.NonPublic);

		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIntPtr;

		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo;

		private static IntPtr ThePreStub;

		public override bool OnMethodCompiledWillBeCalled => false;

		public override event OnMethodCompiledEvent OnMethodCompiled;

		public override MethodBase GetIdentifiable(MethodBase method)
		{
			if ((object)_RTDynamicMethod_m_owner != null && (object)method.GetType() == _RTDynamicMethod)
			{
				return (MethodBase)_RTDynamicMethod_m_owner.GetValue(method);
			}
			return base.GetIdentifiable(method);
		}

		protected override RuntimeMethodHandle GetMethodHandle(MethodBase method)
		{
			if (method is DynamicMethod dynamicMethod)
			{
				if (_RuntimeHelpers__CompileMethod_TakesIntPtr)
				{
					_RuntimeHelpers__CompileMethod.Invoke(null, new object[1] { ((RuntimeMethodHandle)_DynamicMethod_GetMethodDescriptor.Invoke(dynamicMethod, _NoArgs)).Value });
				}
				else if (_RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo)
				{
					_RuntimeHelpers__CompileMethod.Invoke(null, new object[1] { _RuntimeMethodHandle_m_value.GetValue((RuntimeMethodHandle)_DynamicMethod_GetMethodDescriptor.Invoke(dynamicMethod, _NoArgs)) });
				}
				else
				{
					try
					{
						dynamicMethod.CreateDelegate(typeof(MulticastDelegate));
					}
					catch
					{
					}
				}
				if ((object)_DynamicMethod_m_method != null)
				{
					return (RuntimeMethodHandle)_DynamicMethod_m_method.GetValue(method);
				}
				if ((object)_DynamicMethod_GetMethodDescriptor != null)
				{
					return (RuntimeMethodHandle)_DynamicMethod_GetMethodDescriptor.Invoke(method, _NoArgs);
				}
			}
			return method.MethodHandle;
		}

		protected override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}

		protected unsafe override IntPtr GetFunctionPointer(MethodBase method, RuntimeMethodHandle handle)
		{
			MonoMod.MMDbgLog.Log("mets: " + method.GetID());
			MonoMod.MMDbgLog.Log($"meth: 0x{(long)handle.Value:X16}");
			MonoMod.MMDbgLog.Log($"getf: 0x{(long)handle.GetFunctionPointer():X16}");
			bool flag = false;
			IntPtr intPtr;
			while (true)
			{
				if (method.IsVirtual)
				{
					Type declaringType = method.DeclaringType;
					if ((object)declaringType != null && declaringType.IsValueType)
					{
						MonoMod.MMDbgLog.Log($"ldfn: 0x{(long)method.GetLdftnPointer():X16}");
						bool flag2 = false;
						Type[] interfaces = method.DeclaringType.GetInterfaces();
						foreach (Type interfaceType in interfaces)
						{
							if (method.DeclaringType.GetInterfaceMap(interfaceType).TargetMethods.Contains(method))
							{
								flag2 = true;
								break;
							}
						}
						intPtr = method.GetLdftnPointer();
						if (!flag2)
						{
							return intPtr;
						}
						goto IL_00f3;
					}
				}
				intPtr = base.GetFunctionPointer(method, handle);
				goto IL_00f3;
				IL_00f3:
				if (PlatformHelper.Is(Platform.ARM))
				{
					break;
				}
				if (IntPtr.Size == 4)
				{
					int num = (int)intPtr;
					if (*(byte*)num == 184 && *(byte*)(num + 5) == 144 && *(byte*)(num + 6) == 232 && *(byte*)(num + 11) == 233)
					{
						int num2 = num + 11;
						int num3 = *(int*)(num2 + 1) + (num2 + 1 + 4);
						intPtr = NotThePreStub(intPtr, (IntPtr)num3);
						MonoMod.MMDbgLog.Log($"ngen: 0x{(long)intPtr:X8}");
					}
					num = (int)intPtr;
					if (*(byte*)num == 233 && *(byte*)(num + 5) == 95)
					{
						int num4 = num;
						int num5 = *(int*)(num4 + 1) + (num4 + 1 + 4);
						intPtr = NotThePreStub(intPtr, (IntPtr)num5);
						MonoMod.MMDbgLog.Log($"ngen: 0x{(int)intPtr:X8}");
					}
					break;
				}
				long num6 = (long)intPtr;
				if (*(uint*)num6 == 1959363912 && *(uint*)(num6 + 5) == 1224837960 && *(uint*)(num6 + 18) == 1958886217 && *(ushort*)(num6 + 23) == 47176)
				{
					intPtr = NotThePreStub(intPtr, (IntPtr)(*(long*)(num6 + 25)));
					MonoMod.MMDbgLog.Log($"ngen: 0x{(long)intPtr:X16}");
					return intPtr;
				}
				if (*(byte*)num6 == 233 && *(byte*)(num6 + 5) == 95)
				{
					long num7 = num6;
					long num8 = *(int*)(num7 + 1) + (num7 + 1 + 4);
					intPtr = NotThePreStub(intPtr, (IntPtr)num8);
					for (int j = 0; j < 16; j++)
					{
						num6 = (long)intPtr + j;
						if (*(ushort*)num6 == 47176 && *(ushort*)(num6 + 10) == 57599)
						{
							num8 = *(long*)(num6 + 2);
							intPtr = NotThePreStub(intPtr, (IntPtr)num8);
							j = -1;
						}
						else if ((*(ushort*)num6 & 0xFFF0) == 47168 && (*(uint*)(num6 + 10) & 0xF0FFFF) == 65382 && *(ushort*)(num6 + 13) == 34063 && (*(byte*)num6 & 0xF) == (*(byte*)(num6 + 12) & 0xF))
						{
							num7 = num6;
							num8 = *(int*)(num7 + 13 + 2) + (num7 + 13 + 2 + 4);
							intPtr = NotThePreStub(intPtr, (IntPtr)num8);
							j = -1;
						}
					}
					MonoMod.MMDbgLog.Log($"ngen: 0x{(long)intPtr:X16}");
					return intPtr;
				}
				if (*(byte*)num6 != 232 || flag)
				{
					break;
				}
				MonoMod.MMDbgLog.Log("Method thunk reset; regenerating");
				flag = true;
				long num9 = *(int*)(num6 + 1) + (num6 + 1 + 4);
				MonoMod.MMDbgLog.Log($"PrecodeFixupThunk: 0x{num9:X16}");
				PrepareMethod(method, handle);
			}
			return intPtr;
		}

		private IntPtr NotThePreStub(IntPtr ptrGot, IntPtr ptrParsed)
		{
			if (ThePreStub == IntPtr.Zero)
			{
				ThePreStub = (IntPtr)(-2);
				MethodInfo methodInfo = typeof(HttpWebRequest).Assembly.GetType("System.Net.Connection")?.GetMethod("SubmitRequest", BindingFlags.Instance | BindingFlags.NonPublic);
				if ((object)methodInfo != null)
				{
					ThePreStub = GetNativeStart(methodInfo);
					MonoMod.MMDbgLog.Log($"ThePreStub: 0x{(long)ThePreStub:X16}");
				}
				else if (PlatformHelper.Is(Platform.Windows))
				{
					ThePreStub = (IntPtr)(-1);
				}
			}
			if (!(ptrParsed == ThePreStub))
			{
				return ptrParsed;
			}
			return ptrGot;
		}

		static DetourRuntimeNETPlatform()
		{
			MethodInfo runtimeHelpers__CompileMethod = _RuntimeHelpers__CompileMethod;
			_RuntimeHelpers__CompileMethod_TakesIntPtr = (((object)runtimeHelpers__CompileMethod != null) ? runtimeHelpers__CompileMethod.GetParameters()[0].ParameterType.FullName : null) == "System.IntPtr";
			MethodInfo runtimeHelpers__CompileMethod2 = _RuntimeHelpers__CompileMethod;
			_RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo = (((object)runtimeHelpers__CompileMethod2 != null) ? runtimeHelpers__CompileMethod2.GetParameters()[0].ParameterType.FullName : null) == "System.IRuntimeMethodInfo";
			ThePreStub = IntPtr.Zero;
		}
	}
}

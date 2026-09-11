using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using Mono.Cecil;

namespace MonoMod.Utils
{
	public static class ReflectionHelper
	{
		private class CacheFixEntry
		{
			public object Cache;

			public Array Properties;

			public Array Fields;
		}

		private static readonly object[] _NoArgs;

		internal static readonly Dictionary<string, WeakReference> AssemblyCache;

		internal static readonly Dictionary<string, WeakReference[]> AssembliesCache;

		internal static readonly Dictionary<string, WeakReference> ResolveReflectionCache;

		public static readonly byte[] AssemblyHashPrefix;

		public static readonly string AssemblyHashNameTag;

		private const BindingFlags _BindingFlagsAll = (BindingFlags)(-1);

		private static readonly object[] _CacheGetterArgs;

		private static Type t_RuntimeType;

		private static PropertyInfo p_RuntimeType_Cache;

		private static MethodInfo m_RuntimeTypeCache_GetFieldList;

		private static MethodInfo m_RuntimeTypeCache_GetPropertyList;

		private static readonly Dictionary<WeakReference, CacheFixEntry> _CacheFixed;

		private static readonly HashSet<WeakReference> _CacheFixedDead;

		private static Type t_RuntimeModule;

		private static PropertyInfo p_RuntimeModule_RuntimeType;

		private static FieldInfo f_RuntimeModule__impl;

		private static MethodInfo m_RuntimeModule_GetGlobalType;

		private static readonly FieldInfo f_SignatureHelper_module;

		private static MemberInfo _Cache(string cacheKey, MemberInfo value)
		{
			if (cacheKey != null && (object)value == null)
			{
				MMDbgLog.Log("ResolveRefl failure: " + cacheKey);
			}
			if (cacheKey != null && (object)value != null)
			{
				lock (ResolveReflectionCache)
				{
					ResolveReflectionCache[cacheKey] = new WeakReference(value);
				}
			}
			return value;
		}

		public static Assembly Load(ModuleDefinition module)
		{
			using MemoryStream memoryStream = new MemoryStream();
			module.Write(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return Load(memoryStream);
		}

		public static Assembly Load(Stream stream)
		{
			Assembly asm;
			if (stream is MemoryStream memoryStream)
			{
				asm = Assembly.Load(memoryStream.GetBuffer());
			}
			else
			{
				using MemoryStream memoryStream2 = new MemoryStream();
				byte[] array = new byte[4096];
				int count;
				while (0 < (count = stream.Read(array, 0, array.Length)))
				{
					memoryStream2.Write(array, 0, count);
				}
				memoryStream2.Seek(0L, SeekOrigin.Begin);
				asm = Assembly.Load(memoryStream2.GetBuffer());
			}
			AppDomain.CurrentDomain.AssemblyResolve += (object s, ResolveEventArgs e) => (!(e.Name == asm.FullName)) ? null : asm;
			return asm;
		}

		public static Type GetType(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type type = Type.GetType(name);
			if ((object)type != null)
			{
				return type;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				type = assemblies[i].GetType(name);
				if ((object)type != null)
				{
					return type;
				}
			}
			return null;
		}

		public static void ApplyRuntimeHash(this AssemblyNameReference asmRef, Assembly asm)
		{
			byte[] array = new byte[AssemblyHashPrefix.Length + 4];
			Array.Copy(AssemblyHashPrefix, 0, array, 0, AssemblyHashPrefix.Length);
			Array.Copy(BitConverter.GetBytes(asm.GetHashCode()), 0, array, AssemblyHashPrefix.Length, 4);
			asmRef.HashAlgorithm = (AssemblyHashAlgorithm)4294967295u;
			asmRef.Hash = array;
		}

		public static string GetRuntimeHashedFullName(this Assembly asm)
		{
			return $"{asm.FullName}{AssemblyHashNameTag}{asm.GetHashCode()}";
		}

		public static string GetRuntimeHashedFullName(this AssemblyNameReference asm)
		{
			if (asm.HashAlgorithm != (AssemblyHashAlgorithm)4294967295u)
			{
				return asm.FullName;
			}
			byte[] hash = asm.Hash;
			if (hash.Length != AssemblyHashPrefix.Length + 4)
			{
				return asm.FullName;
			}
			for (int i = 0; i < AssemblyHashPrefix.Length; i++)
			{
				if (hash[i] != AssemblyHashPrefix[i])
				{
					return asm.FullName;
				}
			}
			return $"{asm.FullName}{AssemblyHashNameTag}{BitConverter.ToInt32(hash, AssemblyHashPrefix.Length)}";
		}

		public static Type ResolveReflection(this TypeReference mref)
		{
			return _ResolveReflection(mref, null) as Type;
		}

		public static MethodBase ResolveReflection(this MethodReference mref)
		{
			return _ResolveReflection(mref, null) as MethodBase;
		}

		public static FieldInfo ResolveReflection(this FieldReference mref)
		{
			return _ResolveReflection(mref, null) as FieldInfo;
		}

		public static PropertyInfo ResolveReflection(this PropertyReference mref)
		{
			return _ResolveReflection(mref, null) as PropertyInfo;
		}

		public static EventInfo ResolveReflection(this EventReference mref)
		{
			return _ResolveReflection(mref, null) as EventInfo;
		}

		public static MemberInfo ResolveReflection(this MemberReference mref)
		{
			return _ResolveReflection(mref, null);
		}

		private static MemberInfo _ResolveReflection(MemberReference mref, Module[] modules)
		{
			if (mref == null)
			{
				return null;
			}
			if (mref is DynamicMethodReference dynamicMethodReference)
			{
				return dynamicMethodReference.DynamicMethod;
			}
			string text = (mref as MethodReference)?.GetID() ?? mref.FullName;
			TypeReference typeReference = mref.DeclaringType ?? (mref as TypeReference) ?? null;
			IMetadataScope metadataScope = typeReference?.Scope;
			string asmName;
			string moduleName;
			if (!(metadataScope is AssemblyNameReference asm))
			{
				if (!(metadataScope is ModuleDefinition moduleDefinition))
				{
					if (!(metadataScope is ModuleReference))
					{
						if (metadataScope != null)
						{
						}
						asmName = null;
						moduleName = null;
					}
					else
					{
						asmName = typeReference.Module.Assembly.Name.GetRuntimeHashedFullName();
						moduleName = typeReference.Module.Name;
					}
				}
				else
				{
					asmName = moduleDefinition.Assembly.Name.GetRuntimeHashedFullName();
					moduleName = moduleDefinition.Name;
				}
			}
			else
			{
				asmName = asm.GetRuntimeHashedFullName();
				moduleName = null;
			}
			text = text + " | " + (asmName ?? "NOASSEMBLY") + ", " + (moduleName ?? "NOMODULE");
			lock (ResolveReflectionCache)
			{
				if (ResolveReflectionCache.TryGetValue(text, out var value) && value != null && value.SafeGetTarget() is MemberInfo result)
				{
					return result;
				}
			}
			if (mref is GenericParameter)
			{
				throw new NotSupportedException("ResolveReflection on GenericParameter currently not supported");
			}
			if (mref is MethodReference method && mref.DeclaringType is ArrayType)
			{
				Type type = _ResolveReflection(mref.DeclaringType, modules) as Type;
				string methodID = method.GetID(null, null, withType: false);
				MethodBase methodBase = type.GetMethods((BindingFlags)(-1)).Cast<MethodBase>().Concat(type.GetConstructors((BindingFlags)(-1)))
					.FirstOrDefault((MethodBase m) => m.GetID(null, null, withType: false) == methodID);
				if ((object)methodBase != null)
				{
					return _Cache(text, methodBase);
				}
			}
			if (typeReference == null)
			{
				throw new ArgumentException("MemberReference hasn't got a DeclaringType / isn't a TypeReference in itself");
			}
			if (asmName == null && moduleName == null)
			{
				throw new NotSupportedException("Unsupported scope type " + typeReference.Scope.GetType().FullName);
			}
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			MemberInfo memberInfo;
			while (true)
			{
				if (flag3)
				{
					modules = null;
				}
				flag3 = true;
				if (modules == null)
				{
					Assembly[] array = null;
					if (flag && flag2)
					{
						flag2 = false;
						flag = false;
					}
					if (flag)
					{
						lock (AssemblyCache)
						{
							if (AssemblyCache.TryGetValue(asmName, out var value2) && value2.SafeGetTarget() is Assembly assembly)
							{
								array = new Assembly[1] { assembly };
							}
						}
					}
					if (array == null && !flag2)
					{
						lock (AssembliesCache)
						{
							if (AssembliesCache.TryGetValue(asmName, out var value3))
							{
								array = (from asmRef in value3
									select asmRef.SafeGetTarget() as Assembly into assembly3
									where (object)assembly3 != null
									select assembly3).ToArray();
							}
						}
					}
					if (array == null)
					{
						int num = asmName.IndexOf(AssemblyHashNameTag, StringComparison.Ordinal);
						if (num != -1 && int.TryParse(asmName.Substring(num + 2), out var hash))
						{
							array = (from other in AppDomain.CurrentDomain.GetAssemblies()
								where other.GetHashCode() == hash
								select other).ToArray();
							if (array.Length == 0)
							{
								array = null;
							}
							asmName = asmName.Substring(0, num);
						}
						if (array == null)
						{
							array = (from other in AppDomain.CurrentDomain.GetAssemblies()
								where other.GetName().FullName == asmName
								select other).ToArray();
							if (array.Length == 0)
							{
								array = (from other in AppDomain.CurrentDomain.GetAssemblies()
									where other.GetName().Name == asmName
									select other).ToArray();
							}
							if (array.Length == 0)
							{
								Assembly assembly2 = Assembly.Load(new AssemblyName(asmName));
								if ((object)assembly2 != null)
								{
									array = new Assembly[1] { assembly2 };
								}
							}
						}
						if (array.Length != 0)
						{
							lock (AssembliesCache)
							{
								AssembliesCache[asmName] = array.Select((Assembly target) => new WeakReference(target)).ToArray();
							}
						}
					}
					modules = (string.IsNullOrEmpty(moduleName) ? array.SelectMany((Assembly assembly3) => assembly3.GetModules()) : array.Select((Assembly assembly3) => assembly3.GetModule(moduleName))).Where((Module mod) => (object)mod != null).ToArray();
					if (modules.Length == 0)
					{
						throw new Exception("Cannot resolve assembly / module " + asmName + " / " + moduleName);
					}
				}
				if (mref is TypeReference typeReference2)
				{
					if (typeReference2.FullName == "<Module>")
					{
						throw new ArgumentException("Type <Module> cannot be resolved to a runtime reflection type");
					}
					Type type;
					if (mref is TypeSpecification typeSpecification)
					{
						type = _ResolveReflection(typeSpecification.ElementType, null) as Type;
						if ((object)type == null)
						{
							return null;
						}
						if (typeSpecification.IsByReference)
						{
							return _Cache(text, type.MakeByRefType());
						}
						if (typeSpecification.IsPointer)
						{
							return _Cache(text, type.MakePointerType());
						}
						if (typeSpecification.IsArray)
						{
							return _Cache(text, (typeSpecification as ArrayType).IsVector ? type.MakeArrayType() : type.MakeArrayType((typeSpecification as ArrayType).Dimensions.Count));
						}
						if (typeSpecification.IsGenericInstance)
						{
							return _Cache(text, type.MakeGenericType((typeSpecification as GenericInstanceType).GenericArguments.Select((TypeReference arg) => _ResolveReflection(arg, null) as Type).ToArray()));
						}
					}
					else
					{
						type = modules.Select((Module module) => module.GetType(MultiTargetShims.Replace(mref.FullName, "/", "+", StringComparison.Ordinal), throwOnError: false, ignoreCase: false)).FirstOrDefault((Type m) => (object)m != null);
						if ((object)type == null)
						{
							type = modules.Select((Module module) => module.GetTypes().FirstOrDefault((Type m) => mref.Is(m))).FirstOrDefault((Type m) => (object)m != null);
						}
						if ((object)type == null && !flag2)
						{
							goto IL_02a1;
						}
					}
					return _Cache(text, type);
				}
				bool flag4 = mref.DeclaringType.FullName == "<Module>";
				if (mref is GenericInstanceMethod genericInstanceMethod)
				{
					memberInfo = _ResolveReflection(genericInstanceMethod.ElementMethod, modules);
					memberInfo = (memberInfo as MethodInfo)?.MakeGenericMethod(genericInstanceMethod.GenericArguments.Select((TypeReference arg) => _ResolveReflection(arg, null) as Type).ToArray());
				}
				else if (flag4)
				{
					if (mref is MethodReference)
					{
						memberInfo = modules.Select((Module module) => module.GetMethods((BindingFlags)(-1)).FirstOrDefault((MethodInfo m) => mref.Is(m))).FirstOrDefault((MethodInfo m) => (object)m != null);
					}
					else
					{
						if (!(mref is FieldReference))
						{
							throw new NotSupportedException("Unsupported <Module> member type " + mref.GetType().FullName);
						}
						memberInfo = modules.Select((Module module) => module.GetFields((BindingFlags)(-1)).FirstOrDefault((FieldInfo m) => mref.Is(m))).FirstOrDefault((FieldInfo m) => (object)m != null);
					}
				}
				else
				{
					Type type2 = _ResolveReflection(mref.DeclaringType, modules) as Type;
					memberInfo = ((mref is MethodReference) ? type2.GetMethods((BindingFlags)(-1)).Cast<MethodBase>().Concat(type2.GetConstructors((BindingFlags)(-1)))
						.FirstOrDefault((MethodBase m) => mref.Is(m)) : ((!(mref is FieldReference)) ? type2.GetMembers((BindingFlags)(-1)).FirstOrDefault((MemberInfo m) => mref.Is(m)) : type2.GetFields((BindingFlags)(-1)).FirstOrDefault((FieldInfo m) => mref.Is(m))));
				}
				if ((object)memberInfo != null || flag2)
				{
					break;
				}
				goto IL_02a1;
				IL_02a1:
				flag2 = true;
			}
			return _Cache(text, memberInfo);
		}

		public static SignatureHelper ResolveReflection(this CallSite csite, Module context)
		{
			return csite.ResolveReflectionSignature(context);
		}

		public static SignatureHelper ResolveReflectionSignature(this IMethodSignature csite, Module context)
		{
			SignatureHelper signatureHelper = csite.CallingConvention switch
			{
				MethodCallingConvention.C => SignatureHelper.GetMethodSigHelper(context, CallingConvention.Cdecl, csite.ReturnType.ResolveReflection()), 
				MethodCallingConvention.StdCall => SignatureHelper.GetMethodSigHelper(context, CallingConvention.StdCall, csite.ReturnType.ResolveReflection()), 
				MethodCallingConvention.ThisCall => SignatureHelper.GetMethodSigHelper(context, CallingConvention.ThisCall, csite.ReturnType.ResolveReflection()), 
				MethodCallingConvention.FastCall => SignatureHelper.GetMethodSigHelper(context, CallingConvention.FastCall, csite.ReturnType.ResolveReflection()), 
				MethodCallingConvention.VarArg => SignatureHelper.GetMethodSigHelper(context, CallingConventions.VarArgs, csite.ReturnType.ResolveReflection()), 
				_ => (!csite.ExplicitThis) ? SignatureHelper.GetMethodSigHelper(context, CallingConventions.Standard, csite.ReturnType.ResolveReflection()) : SignatureHelper.GetMethodSigHelper(context, CallingConventions.ExplicitThis, csite.ReturnType.ResolveReflection()), 
			};
			if ((object)context != null)
			{
				List<Type> list = new List<Type>();
				List<Type> list2 = new List<Type>();
				foreach (ParameterDefinition parameter in csite.Parameters)
				{
					if (parameter.ParameterType.IsSentinel)
					{
						signatureHelper.AddSentinel();
					}
					if (parameter.ParameterType.IsPinned)
					{
						signatureHelper.AddArgument(parameter.ParameterType.ResolveReflection(), pinned: true);
						continue;
					}
					list2.Clear();
					list.Clear();
					for (TypeReference typeReference = parameter.ParameterType; typeReference is TypeSpecification typeSpecification; typeReference = typeSpecification.ElementType)
					{
						if (!(typeReference is RequiredModifierType requiredModifierType))
						{
							if (typeReference is OptionalModifierType optionalModifierType)
							{
								list2.Add(optionalModifierType.ModifierType.ResolveReflection());
							}
						}
						else
						{
							list.Add(requiredModifierType.ModifierType.ResolveReflection());
						}
					}
					signatureHelper.AddArgument(parameter.ParameterType.ResolveReflection(), list.ToArray(), list2.ToArray());
				}
			}
			else
			{
				foreach (ParameterDefinition parameter2 in csite.Parameters)
				{
					signatureHelper.AddArgument(parameter2.ParameterType.ResolveReflection());
				}
			}
			return signatureHelper;
		}

		static ReflectionHelper()
		{
			_NoArgs = new object[0];
			AssemblyCache = new Dictionary<string, WeakReference>();
			AssembliesCache = new Dictionary<string, WeakReference[]>();
			ResolveReflectionCache = new Dictionary<string, WeakReference>();
			AssemblyHashPrefix = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetBytes("MonoModRefl").Concat(new byte[1]).ToArray();
			AssemblyHashNameTag = "@#";
			_CacheGetterArgs = new object[2] { 0, null };
			t_RuntimeType = typeof(Type).Assembly.GetType("System.RuntimeType");
			p_RuntimeType_Cache = typeof(Type).Assembly.GetType("System.RuntimeType")?.GetProperty("Cache", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			m_RuntimeTypeCache_GetFieldList = typeof(Type).Assembly.GetType("System.RuntimeType+RuntimeTypeCache")?.GetMethod("GetFieldList", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			m_RuntimeTypeCache_GetPropertyList = typeof(Type).Assembly.GetType("System.RuntimeType+RuntimeTypeCache")?.GetMethod("GetPropertyList", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			_CacheFixed = new Dictionary<WeakReference, CacheFixEntry>(new WeakReferenceComparer());
			_CacheFixedDead = new HashSet<WeakReference>();
			t_RuntimeModule = typeof(Module).Assembly.GetType("System.Reflection.RuntimeModule");
			p_RuntimeModule_RuntimeType = typeof(Module).Assembly.GetType("System.Reflection.RuntimeModule")?.GetProperty("RuntimeType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			f_RuntimeModule__impl = typeof(Module).Assembly.GetType("System.Reflection.RuntimeModule")?.GetField("_impl", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			m_RuntimeModule_GetGlobalType = typeof(Module).Assembly.GetType("System.Reflection.RuntimeModule")?.GetMethod("GetGlobalType", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			f_SignatureHelper_module = typeof(SignatureHelper).GetField("m_module", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? typeof(SignatureHelper).GetField("module", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			GCListener.OnCollect += delegate
			{
				lock (_CacheFixed)
				{
					foreach (KeyValuePair<WeakReference, CacheFixEntry> item in _CacheFixed)
					{
						if (!item.Key.SafeGetIsAlive())
						{
							_CacheFixedDead.Add(item.Key);
						}
					}
					foreach (WeakReference item2 in _CacheFixedDead)
					{
						_CacheFixed.Remove(item2);
					}
					_CacheFixedDead.Clear();
				}
			};
		}

		public static void FixReflectionCacheAuto(this Type type)
		{
			type.FixReflectionCache();
		}

		public static void FixReflectionCache(this Type type)
		{
			if ((object)t_RuntimeType == null || (object)p_RuntimeType_Cache == null || (object)m_RuntimeTypeCache_GetFieldList == null || (object)m_RuntimeTypeCache_GetPropertyList == null)
			{
				return;
			}
			for (; (object)type != null; type = type.DeclaringType)
			{
				if (!t_RuntimeType.IsInstanceOfType(type))
				{
					continue;
				}
				WeakReference key = new WeakReference(type);
				CacheFixEntry value;
				lock (_CacheFixed)
				{
					if (!_CacheFixed.TryGetValue(key, out value))
					{
						_CacheFixed.Add(key, value = new CacheFixEntry());
						object cache = (value.Cache = p_RuntimeType_Cache.GetValue(type, _NoArgs));
						value.Properties = _GetArray(cache, m_RuntimeTypeCache_GetPropertyList);
						value.Fields = _GetArray(cache, m_RuntimeTypeCache_GetFieldList);
					}
					else if (!_Verify(value, type))
					{
						continue;
					}
				}
				lock (value)
				{
					_FixReflectionCacheOrder<PropertyInfo>(value.Properties);
					_FixReflectionCacheOrder<FieldInfo>(value.Fields);
				}
			}
		}

		private static bool _Verify(CacheFixEntry entry, Type type)
		{
			object value;
			if (entry.Cache != (value = p_RuntimeType_Cache.GetValue(type, _NoArgs)))
			{
				entry.Cache = value;
				entry.Properties = _GetArray(value, m_RuntimeTypeCache_GetPropertyList);
				entry.Fields = _GetArray(value, m_RuntimeTypeCache_GetFieldList);
				return false;
			}
			Array properties;
			if (entry.Properties != (properties = _GetArray(value, m_RuntimeTypeCache_GetPropertyList)))
			{
				entry.Properties = properties;
				entry.Fields = _GetArray(value, m_RuntimeTypeCache_GetFieldList);
				return false;
			}
			Array fields;
			if (entry.Fields != (fields = _GetArray(value, m_RuntimeTypeCache_GetFieldList)))
			{
				entry.Fields = fields;
				return false;
			}
			return true;
		}

		private static Array _GetArray(object cache, MethodInfo getter)
		{
			getter.Invoke(cache, _CacheGetterArgs);
			return (Array)getter.Invoke(cache, _CacheGetterArgs);
		}

		private static void _FixReflectionCacheOrder<T>(Array orig) where T : MemberInfo
		{
			List<T> list = new List<T>(orig.Length);
			for (int i = 0; i < orig.Length; i++)
			{
				list.Add((T)orig.GetValue(i));
			}
			list.Sort((T a, T b) => a.MetadataToken - b.MetadataToken);
			for (int num = orig.Length - 1; num >= 0; num--)
			{
				orig.SetValue(list[num], num);
			}
		}

		public static Type GetModuleType(this Module module)
		{
			if ((object)module == null || (object)t_RuntimeModule == null || !t_RuntimeModule.IsInstanceOfType(module))
			{
				return null;
			}
			if ((object)p_RuntimeModule_RuntimeType != null)
			{
				return (Type)p_RuntimeModule_RuntimeType.GetValue(module, _NoArgs);
			}
			if ((object)f_RuntimeModule__impl != null && (object)m_RuntimeModule_GetGlobalType != null)
			{
				return (Type)m_RuntimeModule_GetGlobalType.Invoke(null, new object[1] { f_RuntimeModule__impl.GetValue(module) });
			}
			return null;
		}

		public static Type GetRealDeclaringType(this MemberInfo member)
		{
			Type type = member.DeclaringType;
			if ((object)type == null)
			{
				Module module = member.Module;
				if ((object)module == null)
				{
					return null;
				}
				type = module.GetModuleType();
			}
			return type;
		}

		public static CallSite ImportCallSite(this ModuleDefinition moduleTo, ICallSiteGenerator signature)
		{
			return signature.ToCallSite(moduleTo);
		}

		public static CallSite ImportCallSite(this ModuleDefinition moduleTo, SignatureHelper signature)
		{
			return moduleTo.ImportCallSite(f_SignatureHelper_module.GetValue(signature) as Module, signature.GetSignature());
		}

		public static CallSite ImportCallSite(this ModuleDefinition moduleTo, Module moduleFrom, int token)
		{
			return moduleTo.ImportCallSite(moduleFrom, moduleFrom.ResolveSignature(token));
		}

		public static CallSite ImportCallSite(this ModuleDefinition moduleTo, Module moduleFrom, byte[] data)
		{
			CallSite callSite = new CallSite(moduleTo.TypeSystem.Void);
			BinaryReader reader;
			using (MemoryStream input = new MemoryStream(data, writable: false))
			{
				reader = new BinaryReader(input);
				try
				{
					ReadMethodSignature(callSite);
					return callSite;
				}
				finally
				{
					if (reader != null)
					{
						((IDisposable)reader).Dispose();
					}
				}
				void ReadMethodSignature(IMethodSignature method)
				{
					byte b = reader.ReadByte();
					if ((b & 0x20) != 0)
					{
						method.HasThis = true;
						b = (byte)(b & -33);
					}
					if ((b & 0x40) != 0)
					{
						method.ExplicitThis = true;
						b = (byte)(b & -65);
					}
					method.CallingConvention = (MethodCallingConvention)b;
					if ((b & 0x10) != 0)
					{
						ReadCompressedUInt();
					}
					uint num = ReadCompressedUInt();
					method.MethodReturnType.ReturnType = ReadTypeSignature();
					for (int i = 0; i < num; i++)
					{
						method.Parameters.Add(new ParameterDefinition(ReadTypeSignature()));
					}
				}
			}
			TypeReference GetTypeDefOrRef()
			{
				uint num = ReadCompressedUInt();
				uint num2 = num >> 2;
				return moduleTo.ImportReference(moduleFrom.ResolveType((num & 3) switch
				{
					0u => (int)(0x2000000 | num2), 
					1u => (int)(0x1000000 | num2), 
					2u => (int)(0x1B000000 | num2), 
					_ => 0, 
				}));
			}
			int ReadCompressedInt()
			{
				byte b = reader.ReadByte();
				reader.BaseStream.Seek(-1L, SeekOrigin.Current);
				uint num = ReadCompressedUInt();
				int num2 = (int)num >> 1;
				if ((num & 1) == 0)
				{
					return num2;
				}
				switch (b & 0xC0)
				{
				case 0:
				case 64:
					return num2 - 64;
				case 128:
					return num2 - 8192;
				default:
					return num2 - 268435456;
				}
			}
			uint ReadCompressedUInt()
			{
				byte b = reader.ReadByte();
				if ((b & 0x80) == 0)
				{
					return b;
				}
				if ((b & 0x40) == 0)
				{
					return (uint)(((b & -129) << 8) | reader.ReadByte());
				}
				return (uint)(((b & -193) << 24) | (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | reader.ReadByte());
			}
			TypeReference ReadTypeSignature()
			{
				MetadataType metadataType = (MetadataType)reader.ReadByte();
				switch (metadataType)
				{
				case MetadataType.ValueType:
				case MetadataType.Class:
					return GetTypeDefOrRef();
				case MetadataType.Pointer:
					return new PointerType(ReadTypeSignature());
				case MetadataType.FunctionPointer:
				{
					FunctionPointerType functionPointerType = new FunctionPointerType();
					ReadMethodSignature(functionPointerType);
					return functionPointerType;
				}
				case MetadataType.ByReference:
					return new ByReferenceType(ReadTypeSignature());
				case MetadataType.Pinned:
					return new PinnedType(ReadTypeSignature());
				case (MetadataType)29:
					return new ArrayType(ReadTypeSignature());
				case MetadataType.Array:
				{
					ArrayType arrayType = new ArrayType(ReadTypeSignature());
					uint num = ReadCompressedUInt();
					uint[] array = new uint[ReadCompressedUInt()];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = ReadCompressedUInt();
					}
					int[] array2 = new int[ReadCompressedUInt()];
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = ReadCompressedInt();
					}
					arrayType.Dimensions.Clear();
					for (int k = 0; k < num; k++)
					{
						int? num2 = null;
						int? upperBound = null;
						if (k < array2.Length)
						{
							num2 = array2[k];
						}
						if (k < array.Length)
						{
							upperBound = num2 + (int)array[k] - 1;
						}
						arrayType.Dimensions.Add(new ArrayDimension(num2, upperBound));
					}
					return arrayType;
				}
				case MetadataType.OptionalModifier:
					return new OptionalModifierType(GetTypeDefOrRef(), ReadTypeSignature());
				case MetadataType.RequiredModifier:
					return new RequiredModifierType(GetTypeDefOrRef(), ReadTypeSignature());
				case MetadataType.Sentinel:
					return new SentinelType(ReadTypeSignature());
				case MetadataType.Var:
				case MetadataType.GenericInstance:
				case MetadataType.MVar:
					throw new NotSupportedException($"Unsupported generic callsite element: {metadataType}");
				case MetadataType.Object:
					return moduleTo.TypeSystem.Object;
				case MetadataType.Void:
					return moduleTo.TypeSystem.Void;
				case MetadataType.TypedByReference:
					return moduleTo.TypeSystem.TypedReference;
				case MetadataType.IntPtr:
					return moduleTo.TypeSystem.IntPtr;
				case MetadataType.UIntPtr:
					return moduleTo.TypeSystem.UIntPtr;
				case MetadataType.Boolean:
					return moduleTo.TypeSystem.Boolean;
				case MetadataType.Char:
					return moduleTo.TypeSystem.Char;
				case MetadataType.SByte:
					return moduleTo.TypeSystem.SByte;
				case MetadataType.Byte:
					return moduleTo.TypeSystem.Byte;
				case MetadataType.Int16:
					return moduleTo.TypeSystem.Int16;
				case MetadataType.UInt16:
					return moduleTo.TypeSystem.UInt16;
				case MetadataType.Int32:
					return moduleTo.TypeSystem.Int32;
				case MetadataType.UInt32:
					return moduleTo.TypeSystem.UInt32;
				case MetadataType.Int64:
					return moduleTo.TypeSystem.Int64;
				case MetadataType.UInt64:
					return moduleTo.TypeSystem.UInt64;
				case MetadataType.Single:
					return moduleTo.TypeSystem.Single;
				case MetadataType.Double:
					return moduleTo.TypeSystem.Double;
				case MetadataType.String:
					return moduleTo.TypeSystem.String;
				default:
					throw new NotSupportedException($"Unsupported callsite element: {metadataType}");
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Den.Tools.GUI
{
	public sealed class SpecialEditorAttribute : Attribute
	{
		public string className;

		public string actionName;

		[NonSerialized]
		private static readonly Dictionary<Type, Delegate> actionsCache = new Dictionary<Type, Delegate>();

		[NonSerialized]
		private static readonly Dictionary<Type, Delegate> delegateCaches = new Dictionary<Type, Delegate>();

		[NonSerialized]
		private static readonly Dictionary<Type, MethodInfo> methodsCaches = new Dictionary<Type, MethodInfo>();

		public SpecialEditorAttribute(string className, string actionName)
		{
			this.className = className;
			this.actionName = actionName;
		}

		public SpecialEditorAttribute(string className, string actionName, string cat)
		{
			this.className = className;
			this.actionName = actionName;
		}

		private static MethodInfo GetEditorMethod(Type type)
		{
			if (methodsCaches.TryGetValue(type, out var value))
			{
				return value;
			}
			if (Attribute.GetCustomAttribute(type, typeof(SpecialEditorAttribute)) is SpecialEditorAttribute specialEditorAttribute)
			{
				Type editorType = GetEditorType(type, specialEditorAttribute.className);
				if (editorType != null)
				{
					value = editorType.GetMethod(specialEditorAttribute.actionName);
					if (value == null)
					{
						throw new Exception("Could not find method " + specialEditorAttribute.actionName + " in " + specialEditorAttribute.className);
					}
				}
			}
			methodsCaches.Add(type, value);
			return value;
		}

		public static void Draw2(object obj, Type nullObjType = null)
		{
			MethodInfo editorMethod = GetEditorMethod((obj != null) ? obj.GetType() : nullObjType);
			if (!(editorMethod == null))
			{
				(Delegate.CreateDelegate(typeof(Action<object>), editorMethod) as Action<object>)(obj);
			}
		}

		public static void Draw<TO>(TO obj)
		{
			Type type = obj.GetType();
			if (!(Attribute.GetCustomAttribute(type, typeof(SpecialEditorAttribute)) is SpecialEditorAttribute specialEditorAttribute))
			{
				return;
			}
			MethodInfo method = GetEditorType(type, specialEditorAttribute.className).GetMethod(specialEditorAttribute.actionName);
			if (method == null)
			{
				throw new Exception("Could not find method " + specialEditorAttribute.actionName + " in " + specialEditorAttribute.className);
			}
			Action<TO> action;
			if (actionsCache.ContainsKey(type))
			{
				action = actionsCache[type] as Action<TO>;
			}
			else
			{
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Length != 1)
				{
					throw new Exception("Special Editor: Number of method arguments (" + parameters.Length + ") doesn't match called count (1)");
				}
				if (parameters[0].ParameterType != typeof(TO))
				{
					throw new Exception("Special Editor: Arguments don't match: \n\t" + parameters[0].ParameterType?.ToString() + " vs " + typeof(TO));
				}
				action = Delegate.CreateDelegate(typeof(Action<TO>), method) as Action<TO>;
				actionsCache.Add(type, action);
			}
			action(obj);
		}

		public static void Draw<TO, T1>(TO obj, T1 t1)
		{
			Type type = obj.GetType();
			if (!(Attribute.GetCustomAttribute(type, typeof(SpecialEditorAttribute)) is SpecialEditorAttribute specialEditorAttribute))
			{
				return;
			}
			MethodInfo method = GetEditorType(type, specialEditorAttribute.className).GetMethod(specialEditorAttribute.actionName);
			if (method == null)
			{
				throw new Exception("Could not find method " + specialEditorAttribute.actionName + " in " + specialEditorAttribute.className);
			}
			Action<TO, T1> action;
			if (actionsCache.ContainsKey(type))
			{
				action = actionsCache[type] as Action<TO, T1>;
			}
			else
			{
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Length != 2)
				{
					throw new Exception("Special Editor: Number of method arguments (" + parameters.Length + ") doesn't match called count (2)");
				}
				if (parameters[0].ParameterType != typeof(TO) || parameters[1].ParameterType != typeof(T1))
				{
					throw new Exception("Special Editor: Arguments don't match: \n\t" + parameters[0].ParameterType?.ToString() + " vs " + typeof(TO)?.ToString() + "\n\t" + parameters[1].ParameterType?.ToString() + " vs " + typeof(T1));
				}
				action = Delegate.CreateDelegate(typeof(Action<TO, T1>), method) as Action<TO, T1>;
				actionsCache.Add(type, action);
			}
			action(obj, t1);
		}

		public static void Draw<TO, T1, T2>(TO obj, T1 t1, T2 t2)
		{
			Type type = obj.GetType();
			if (!(Attribute.GetCustomAttribute(type, typeof(SpecialEditorAttribute)) is SpecialEditorAttribute specialEditorAttribute))
			{
				return;
			}
			MethodInfo method = GetEditorType(type, specialEditorAttribute.className).GetMethod(specialEditorAttribute.actionName);
			if (method == null)
			{
				throw new Exception("Special Editor: Could not find method " + specialEditorAttribute.actionName + " in " + specialEditorAttribute.className);
			}
			Action<TO, T1, T2> action;
			if (actionsCache.ContainsKey(type))
			{
				action = actionsCache[type] as Action<TO, T1, T2>;
			}
			else
			{
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Length != 3)
				{
					throw new Exception("Special Editor: Number of method arguments (" + parameters.Length + ") doesn't match called count (3)");
				}
				if (parameters[0].ParameterType != typeof(TO) || parameters[1].ParameterType != typeof(T1) || parameters[2].ParameterType != typeof(T2))
				{
					throw new Exception("Special Editor: Arguments don't match: \n\t" + parameters[0].ParameterType?.ToString() + " vs " + typeof(TO)?.ToString() + "\n\t" + parameters[1].ParameterType?.ToString() + " vs " + typeof(T1)?.ToString() + "\n\t" + parameters[2].ParameterType?.ToString() + " vs " + typeof(T2));
				}
				action = Delegate.CreateDelegate(typeof(Action<TO, T1, T2>), method) as Action<TO, T1, T2>;
				actionsCache.Add(type, action);
			}
			action(obj, t1, t2);
		}

		public static void Draw<TO, T1, T2, T3>(TO obj, T1 t1, T2 t2, T3 t3)
		{
			Type type = obj.GetType();
			if (!(Attribute.GetCustomAttribute(type, typeof(SpecialEditorAttribute)) is SpecialEditorAttribute specialEditorAttribute))
			{
				return;
			}
			MethodInfo method = GetEditorType(type, specialEditorAttribute.className).GetMethod(specialEditorAttribute.actionName);
			if (method == null)
			{
				throw new Exception("Special Editor: Could not find method " + specialEditorAttribute.actionName + " in " + specialEditorAttribute.className);
			}
			Action<TO, T1, T2, T3> action;
			if (actionsCache.ContainsKey(type))
			{
				action = actionsCache[type] as Action<TO, T1, T2, T3>;
			}
			else
			{
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Length != 4)
				{
					throw new Exception("Special Editor: Number of method arguments (" + parameters.Length + ") doesn't match called count (4)");
				}
				if (parameters[0].ParameterType != typeof(TO) || parameters[1].ParameterType != typeof(T1) || parameters[2].ParameterType != typeof(T2) || parameters[3].ParameterType != typeof(T3))
				{
					throw new Exception("Special Editor: Arguments don't match: \n\t" + parameters[0].ParameterType?.ToString() + " vs " + typeof(TO)?.ToString() + "\n\t" + parameters[1].ParameterType?.ToString() + " vs " + typeof(T1)?.ToString() + "\n\t" + parameters[2].ParameterType?.ToString() + " vs " + typeof(T2)?.ToString() + "\n\t" + parameters[3].ParameterType?.ToString() + " vs " + typeof(T3));
				}
				action = Delegate.CreateDelegate(typeof(Action<TO, T1, T2, T3>), method) as Action<TO, T1, T2, T3>;
				actionsCache.Add(type, action);
			}
			action(obj, t1, t2, t3);
		}

		public static Type GetEditorType(Type baseType, string editorTypeName)
		{
			Type type = null;
			type = Type.GetType(editorTypeName);
			if (type != null)
			{
				return type;
			}
			Assembly assembly = baseType.Assembly;
			type = assembly.GetType(editorTypeName);
			if (type != null)
			{
				return type;
			}
			Assembly assembly2 = null;
			string text = assembly.GetName().Name + "Editor";
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly3 in assemblies)
			{
				if (assembly3.GetName().Name == text)
				{
					assembly2 = assembly3;
					break;
				}
			}
			if (assembly2 != null)
			{
				type = assembly2.GetType(editorTypeName);
				if (type != null)
				{
					return type;
				}
			}
			if (type == null)
			{
				Type[] types = assembly.GetTypes();
				for (int j = 0; j < types.Length; j++)
				{
					string text2 = types[j].Name;
					if (text2.LastIndexOf('.') >= 0)
					{
						text2 = text2.Substring(text2.LastIndexOf('.'), text2.Length);
					}
					if (text2 == editorTypeName)
					{
						type = types[j];
						break;
					}
				}
			}
			if (type != null)
			{
				return type;
			}
			if (type == null && assembly2 != null)
			{
				Type[] types2 = assembly2.GetTypes();
				for (int k = 0; k < types2.Length; k++)
				{
					string text3 = types2[k].Name;
					if (text3.LastIndexOf('.') >= 0)
					{
						text3 = text3.Substring(text3.LastIndexOf('.'), text3.Length);
					}
					if (text3 == editorTypeName)
					{
						type = types2[k];
						break;
					}
				}
			}
			return type;
		}
	}
}

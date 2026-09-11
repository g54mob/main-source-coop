using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyTextEffects.Editor.MyBoxCopy.Extensions
{
	[PublicAPI]
	public static class MyReflection
	{
		public static bool HasMethod(this object target, string methodName)
		{
			return target.GetType().GetMethod(methodName) != null;
		}

		public static bool HasField(this object target, string fieldName)
		{
			return target.GetType().GetField(fieldName) != null;
		}

		public static bool HasProperty(this object target, string propertyName)
		{
			return target.GetType().GetProperty(propertyName) != null;
		}

		public static T GetPrivateField<T>(this object target, string fieldName)
		{
			if (target == null)
			{
				Debug.LogError("Trying to get field from null");
				return default(T);
			}
			if (fieldName.IsNullOrEmpty())
			{
				Debug.LogError("Trying to fet unspecified field");
				return default(T);
			}
			FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				Debug.LogError($"Field {fieldName} not found in {target.GetType()}");
				return default(T);
			}
			return (T)field.GetValue(target);
		}

		public static void SetPrivateField<T>(this object target, string fieldName, T value)
		{
			if (target == null)
			{
				Debug.LogError("Trying to set field to null");
				return;
			}
			if (fieldName.IsNullOrEmpty())
			{
				Debug.LogError("Trying to set unspecified field");
				return;
			}
			FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				Debug.LogError($"Field {fieldName} not found in {target.GetType()}");
			}
			else if (field.FieldType != typeof(T))
			{
				Debug.LogError($"Field {fieldName} is of type {field.FieldType} while trying to set {value.GetType()}");
			}
			else
			{
				field.SetValue(target, value);
			}
		}

		public static T GetPrivateProperty<T>(this object target, string propertyName)
		{
			if (target == null)
			{
				Debug.LogError("Trying to get property from null");
				return default(T);
			}
			if (propertyName.IsNullOrEmpty())
			{
				Debug.LogError("Trying to get unspecified property");
				return default(T);
			}
			PropertyInfo property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
			if (property == null)
			{
				Debug.LogError($"Property {propertyName} not found in {target.GetType()}");
				return default(T);
			}
			return (T)property.GetValue(target);
		}
	}
}

using System;
using System.Reflection;
using MessagePack.Formatters;

namespace MessagePack.Internal
{
	internal static class ResolverUtilities
	{
		internal static IMessagePackFormatter ActivateFormatter(Type formatterType, object?[]? args = null)
		{
			if (args == null || args.Length == 0)
			{
				ConstructorInfo constructor = formatterType.GetConstructor(Type.EmptyTypes);
				if ((object)constructor != null)
				{
					return (IMessagePackFormatter)constructor.Invoke(Array.Empty<object>());
				}
				FieldInfo fieldInfo = FetchSingletonField(formatterType);
				if ((object)fieldInfo != null)
				{
					return (IMessagePackFormatter)(fieldInfo.GetValue(null) ?? throw new InvalidOperationException(fieldInfo.ReflectedType?.FullName + "." + fieldInfo.Name + " return null."));
				}
				throw new MessagePackSerializationException("The " + formatterType.FullName + " formatter has no default constructor nor implements the singleton pattern.");
			}
			return (IMessagePackFormatter)Activator.CreateInstance(formatterType, args);
		}

		internal static FieldInfo? FetchSingletonField(Type formatterType)
		{
			FieldInfo field = formatterType.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
			if ((object)field != null && field.IsInitOnly)
			{
				return field;
			}
			return null;
		}
	}
}

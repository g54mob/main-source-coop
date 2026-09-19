using System;
using System.Linq;
using System.Reflection;
using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class DynamicObjectResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly IMessagePackFormatter<T>? Formatter = BuildFormatterHelper<T>(Instance, DynamicAssemblyFactory, forceStringKey: false, contractless: false, allowPrivate: false);
		}

		private const string ModuleName = "MessagePack.Resolvers.DynamicObjectResolver";

		public static readonly DynamicObjectResolver Instance;

		public static readonly MessagePackSerializerOptions Options;

		internal static readonly DynamicAssemblyFactory DynamicAssemblyFactory;

		static DynamicObjectResolver()
		{
			Instance = new DynamicObjectResolver();
			DynamicAssemblyFactory = new DynamicAssemblyFactory("MessagePack.Resolvers.DynamicObjectResolver");
			Options = new MessagePackSerializerOptions(Instance);
		}

		private DynamicObjectResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}

		internal static IMessagePackFormatter<T>? BuildFormatterHelper<T>(IFormatterResolver self, DynamicAssemblyFactory dynamicAssemblyFactory, bool forceStringKey, bool contractless, bool allowPrivate)
		{
			TypeInfo typeInfo = typeof(T).GetTypeInfo();
			if (typeInfo.IsInterface || typeInfo.IsAbstract)
			{
				return null;
			}
			DynamicAssembly dynamicAssembly = null;
			if (typeInfo.IsAnonymous())
			{
				forceStringKey = true;
				contractless = true;
				allowPrivate = false;
				dynamicAssembly = DynamicAssemblyFactory.GetDynamicAssembly(typeof(T), allowPrivate: true);
			}
			else if (typeInfo.IsNullable())
			{
				typeInfo = typeInfo.GenericTypeArguments[0].GetTypeInfo();
				object formatterDynamic = self.GetFormatterDynamic(typeInfo.AsType());
				if (formatterDynamic == null)
				{
					return null;
				}
				return (IMessagePackFormatter<T>)Activator.CreateInstance(typeof(StaticNullableFormatter<>).MakeGenericType(typeInfo.AsType()), formatterDynamic);
			}
			allowPrivate |= !contractless && typeof(T).GetCustomAttributes<MessagePackObjectAttribute>().Any((MessagePackObjectAttribute a) => a.AllowPrivate);
			if (dynamicAssembly == null)
			{
				dynamicAssembly = DynamicAssemblyFactory.GetDynamicAssembly(typeof(T), allowPrivate);
			}
			TypeInfo typeInfo2 = DynamicObjectTypeBuilder.BuildType(dynamicAssembly, typeof(T), forceStringKey, contractless, allowPrivate);
			if ((object)typeInfo2 != null)
			{
				return (IMessagePackFormatter<T>)ResolverUtilities.ActivateFormatter(typeInfo2.AsType());
			}
			return null;
		}
	}
}

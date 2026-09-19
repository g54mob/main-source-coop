using System;
using System.CodeDom.Compiler;

namespace Features.UserReport.CustomUserReporting.Scripts.Plugin
{
	[GeneratedCode("simple-json", "1.0.0")]
	public interface IJsonSerializerStrategy
	{
		bool TrySerializeNonPrimitiveObject(object input, out object output);

		object DeserializeObject(object value, Type type);
	}
}

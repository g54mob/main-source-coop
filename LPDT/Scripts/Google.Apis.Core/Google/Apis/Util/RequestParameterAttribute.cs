using System;

namespace Google.Apis.Util
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class RequestParameterAttribute : Attribute
	{
		private readonly string name;

		private readonly RequestParameterType type;

		public string Name => name;

		public RequestParameterType Type => type;

		public RequestParameterAttribute(string name)
			: this(name, RequestParameterType.Query)
		{
		}

		public RequestParameterAttribute(string name, RequestParameterType type)
		{
			this.name = name;
			this.type = type;
		}
	}
}

using System;

namespace Google.Apis.Util
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class StringValueAttribute : Attribute
	{
		private readonly string text;

		public string Text => text;

		public StringValueAttribute(string text)
		{
			text.ThrowIfNull("text");
			this.text = text;
		}
	}
}

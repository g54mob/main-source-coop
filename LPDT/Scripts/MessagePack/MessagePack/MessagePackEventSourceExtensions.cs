using System;
using System.Diagnostics.Tracing;

namespace MessagePack
{
	internal static class MessagePackEventSourceExtensions
	{
		internal static void FormatterDynamicallyGeneratedStop(this MessagePackEventSource source, Type dataType)
		{
			if (source.IsEnabled(EventLevel.Informational, EventKeywords.None))
			{
				source.FormatterDynamicallyGeneratedStop(dataType.AssemblyQualifiedName);
			}
		}
	}
}

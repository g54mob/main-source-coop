using System.Diagnostics.Tracing;

namespace MessagePack
{
	[EventSource(Name = "MessagePack")]
	internal class MessagePackEventSource : EventSource
	{
		public static class Tasks
		{
			public const EventTask FormatterDynamicallyGenerated = (EventTask)1;
		}

		internal static readonly MessagePackEventSource Instance = new MessagePackEventSource();

		private const int FormatterDynamicallyGeneratedStartEvent = 1;

		private const int FormatterDynamicallyGeneratedStopEvent = 2;

		private MessagePackEventSource()
		{
		}

		[Event(1, Task = (EventTask)1, Opcode = EventOpcode.Start)]
		public void FormatterDynamicallyGeneratedStart()
		{
			WriteEvent(1);
		}

		[Event(2, Task = (EventTask)1, Opcode = EventOpcode.Stop)]
		public void FormatterDynamicallyGeneratedStop(string? dataType)
		{
			WriteEvent(2, dataType);
		}
	}
}

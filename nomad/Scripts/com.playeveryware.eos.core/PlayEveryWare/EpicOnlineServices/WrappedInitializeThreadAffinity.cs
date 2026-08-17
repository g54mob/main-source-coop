using Epic.OnlineServices.Platform;
using PlayEveryWare.Common;

namespace PlayEveryWare.EpicOnlineServices
{
	public class WrappedInitializeThreadAffinity : Wrapped<InitializeThreadAffinity>
	{
		[ConfigField("Network", ConfigFieldType.Ulong, "Any thread related to network management that is not IO.", -1, null)]
		public ulong NetworkWork
		{
			get
			{
				return _value.NetworkWork;
			}
			set
			{
				_value.NetworkWork = value;
			}
		}

		[ConfigField("Storage IO", ConfigFieldType.Ulong, "Any thread that will interact with a storage device.", -1, null)]
		public ulong StorageIo
		{
			get
			{
				return _value.StorageIo;
			}
			set
			{
				_value.StorageIo = value;
			}
		}

		[ConfigField("Web Socket IO", ConfigFieldType.Ulong, "Any thread that will generate web socket IO.", -1, null)]
		public ulong WebSocketIo
		{
			get
			{
				return _value.WebSocketIo;
			}
			set
			{
				_value.WebSocketIo = value;
			}
		}

		[ConfigField("P2P IO", ConfigFieldType.Ulong, "Any thread that will generate IO related to P2P traffic and management.", -1, null)]
		public ulong P2PIo
		{
			get
			{
				return _value.P2PIo;
			}
			set
			{
				_value.P2PIo = value;
			}
		}

		[ConfigField("HTTP Request IO", ConfigFieldType.Ulong, "Any thread that will generate http request IO.", -1, null)]
		public ulong HttpRequestIo
		{
			get
			{
				return _value.HttpRequestIo;
			}
			set
			{
				_value.HttpRequestIo = value;
			}
		}

		[ConfigField("RTC IO", ConfigFieldType.Ulong, "Any thread that will generate IO related to RTC traffic and management.", -1, null)]
		public ulong RTCIo
		{
			get
			{
				return _value.RTCIo;
			}
			set
			{
				_value.RTCIo = value;
			}
		}

		[ConfigField("Embedded Overlay Main Thread", ConfigFieldType.Ulong, "Main thread of the external overlay.", -1, null)]
		public ulong EmbeddedOverlayMainThread
		{
			get
			{
				return _value.EmbeddedOverlayMainThread;
			}
			set
			{
				_value.EmbeddedOverlayMainThread = value;
			}
		}

		[ConfigField("Embedded Overlay Worker Threads", ConfigFieldType.Ulong, "Worker threads of the external overlay.", -1, null)]
		public ulong EmbeddedOverlayWorkerThreads
		{
			get
			{
				return _value.EmbeddedOverlayWorkerThreads;
			}
			set
			{
				_value.EmbeddedOverlayWorkerThreads = value;
			}
		}
	}
}

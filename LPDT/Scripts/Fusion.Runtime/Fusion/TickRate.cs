using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit)]
	public struct TickRate
	{
		[Serializable]
		[StructLayout(LayoutKind.Explicit)]
		public struct Selection
		{
			[FieldOffset(0)]
			[RangeEx(8.0, 256.0)]
			[InlineHelp]
			[DisplayName("Client & Server Tick Rate")]
			public int Client;

			[FieldOffset(8)]
			[TickRateDivisor("Client", "ClientSendIndex", false)]
			[InlineHelp]
			[DisplayName("Client Send Rate")]
			public int ClientSendInterval;

			[FieldOffset(4)]
			[TickRateDivisor("Client", "ServerIndex", true)]
			[InlineHelp]
			[DisplayName("Server Tick Rate")]
			public int ServerTickInterval;

			[FieldOffset(12)]
			[TickRateDivisor("Client", "ServerSendIndex", false)]
			[InlineHelp]
			[DisplayName("Server Send Rate")]
			public int ServerSendInterval;

			[FieldOffset(8)]
			[Obsolete("Use ClientSendInterval instead.")]
			[LastSupportedVersion("2.0")]
			public int ClientSendIndex;

			[FieldOffset(4)]
			[Obsolete("Use ServerTickInterval instead.")]
			[LastSupportedVersion("2.0")]
			public int ServerIndex;

			[FieldOffset(12)]
			[Obsolete("Use ServerSendInterval instead.")]
			[LastSupportedVersion("2.0")]
			public int ServerSendIndex;

			public bool ConvertObsoleteIndicesToIntervals()
			{
				if (!Equals(Zero) && ServerTickInterval == 0)
				{
					ClientSendInterval = 1 << ClientSendInterval;
					ServerTickInterval = 1 << ServerTickInterval;
					ServerSendInterval = 1 << ServerSendInterval;
					return true;
				}
				return false;
			}

			public ValidateResult Validate()
			{
				if (!IsValid(Client) || !IsValidInterval(ClientSendInterval) || !IsValidInterval(ServerTickInterval, 8) || !IsValidInterval(ServerSendInterval))
				{
					return ValidateResult.InvalidTickRate;
				}
				if (ServerTickInterval < 1 || ServerTickInterval > 8 || Client / ServerTickInterval < 4)
				{
					return ValidateResult.ServerIndexOutOfRange;
				}
				if (ServerSendInterval < 1 || ServerSendInterval > 8 || Client / ServerSendInterval < 4)
				{
					return ValidateResult.ServerSendIndexOutOfRange;
				}
				if (ClientSendInterval < 1 || ClientSendInterval > 8 || Client / ClientSendInterval < 4)
				{
					return ValidateResult.ClientSendIndexOutOfRange;
				}
				if (ServerSendInterval < ServerTickInterval)
				{
					return ValidateResult.ServerSendRateLargerThanTickRate;
				}
				return ValidateResult.Ok;
			}

			internal bool IsValidForShared(out string invalidReason)
			{
				ValidateResult validateResult = Validate();
				if (validateResult != ValidateResult.Ok)
				{
					invalidReason = validateResult.ToString();
					return false;
				}
				if (Client > 32)
				{
					invalidReason = $"the tick rate exceeds the allowed maximum ({32})!";
					return false;
				}
				if (ServerTickInterval != 1 || ServerSendInterval != ClientSendInterval)
				{
					invalidReason = "the server and client tick and/or send rates don't match!";
					return false;
				}
				invalidReason = string.Empty;
				return true;
			}

			internal bool IsValidInterval(int interval, int minResultingRate = 4)
			{
				return IsValidInterval(Client, interval, minResultingRate);
			}

			internal static bool IsValidInterval(int tickRate, int interval, int minResultingRate = 4)
			{
				return interval >= 1 && interval <= 8 && tickRate % interval == 0 && tickRate / interval >= minResultingRate;
			}

			internal void Clamp()
			{
				Client = Maths.Clamp(Client, 8, 256);
				ClientSendInterval = ClampToValidInterval(ClientSendInterval);
				ServerTickInterval = 1;
				ServerSendInterval = Math.Max(ClampToValidInterval(ServerSendInterval), ServerTickInterval);
			}

			internal int ClampToValidInterval(int interval, int minResultingRate = 4)
			{
				return ClampToValidInterval(Client, interval, minResultingRate);
			}

			internal static int ClampToValidInterval(int tickRate, int interval, int minResultingRate = 4)
			{
				interval = Maths.Clamp(interval, 1, 8);
				for (int num = interval; num > 1; num--)
				{
					if (IsValidInterval(tickRate, num, minResultingRate))
					{
						return num;
					}
				}
				return 1;
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 16)]
		public struct Resolved
		{
			public const int WORDS = 4;

			[FieldOffset(0)]
			public int Client;

			[FieldOffset(4)]
			public int ClientSend;

			[FieldOffset(8)]
			public int Server;

			[FieldOffset(12)]
			public int ServerSend;

			public const int SIZE = 16;

			public const int WORD_COUNT = 4;

			internal const int BYTE_OF_CLIENT = 0;

			internal const int BYTE_COUNT_OF_CLIENT = 4;

			internal const int BYTE_OF_CLIENT_SEND = 4;

			internal const int BYTE_COUNT_OF_CLIENT_SEND = 4;

			internal const int BYTE_OF_SERVER = 8;

			internal const int BYTE_COUNT_OF_SERVER = 4;

			internal const int BYTE_OF_SERVER_SEND = 12;

			internal const int BYTE_COUNT_OF_SERVER_SEND = 4;

			private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

			public readonly double ServerTickDelta => Inverse(Server);

			public readonly double ServerSendDelta => Inverse(ServerSend);

			public readonly int ServerTickStride => Client / Server;

			public readonly double ClientTickDelta => Inverse(Client);

			public readonly double ClientSendDelta => Inverse(ClientSend);

			public readonly int ClientTickStride => 1;

			internal Resolved(int client, int clientSend, int server, int serverSend)
			{
				Client = client;
				ClientSend = clientSend;
				Server = server;
				ServerSend = serverSend;
			}

			public override readonly string ToString()
			{
				return $"[ClientTickRate = {Client}, ClientSendRate = {ClientSend}, ServerTickRate = {Server}, ServerSendRate = {ServerSend}]";
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static double Inverse(int rate)
			{
				return (rate == 0) ? 0.0 : (1.0 / (double)rate);
			}
		}

		public enum ValidateResult
		{
			Ok = 0,
			Error = 1,
			NotFound = 2,
			InvalidTickRate = 3,
			ServerIndexOutOfRange = 4,
			ClientSendIndexOutOfRange = 5,
			ServerSendIndexOutOfRange = 6,
			ServerSendRateLargerThanTickRate = 7
		}

		internal const int SharedMax = 32;

		internal const int Min = 8;

		internal const int Max = 256;

		private const int MinSend = 4;

		internal const int MinInterval = 1;

		internal const int MaxInterval = 8;

		[Obsolete("Use TickRate.Selection.Client or TickRate.Resolved.Client instead.", true)]
		[LastSupportedVersion("2.0")]
		public readonly int Client => 0;

		[Obsolete("TickRate no longer stores rate values, use TickRate.Selection instead.", true)]
		[LastSupportedVersion("2.0")]
		public readonly int Count => 0;

		[Obsolete("TickRate no longer stores rate values, use TickRate.Selection instead.", true)]
		[LastSupportedVersion("2.0")]
		public int this[int index] => GetTickRate(index);

		internal static Selection Zero => new Selection
		{
			Client = 0,
			ClientSendInterval = 0,
			ServerTickInterval = 0,
			ServerSendInterval = 0
		};

		internal static Selection Default => new Selection
		{
			Client = 64,
			ClientSendInterval = 2,
			ServerTickInterval = 1,
			ServerSendInterval = 2
		};

		internal static Selection SharedDefault => new Selection
		{
			Client = 32,
			ClientSendInterval = 2,
			ServerTickInterval = 1,
			ServerSendInterval = 2
		};

		[Obsolete("TickRate no longer stores rate values, use TickRate.Selection instead.", true)]
		[LastSupportedVersion("2.0")]
		public int GetDivisor(int index)
		{
			return 1;
		}

		[Obsolete("TickRate no longer stores rate values, use TickRate.Selection instead.", true)]
		[LastSupportedVersion("2.0")]
		public int GetTickRate(int index)
		{
			return 0;
		}

		[Obsolete("TickRate no longer stores rate values, use TickRate.Selection instead.", true)]
		[LastSupportedVersion("2.0")]
		public int[] ToArray()
		{
			return Array.Empty<int>();
		}

		public Selection ClampSelection(Selection selection)
		{
			if (!IsValid(selection.Client))
			{
				return default(Selection);
			}
			selection.Clamp();
			return selection;
		}

		[Obsolete("Use TickRate.Selection.Validate instead.")]
		[LastSupportedVersion("2.0")]
		public ValidateResult ValidateSelection(Selection selected)
		{
			return selected.Validate();
		}

		internal static Selection GetDefault(GameMode forMode)
		{
			return (forMode == GameMode.Shared) ? SharedDefault : Default;
		}

		[Obsolete("TickRate no longer stores rate values.", true)]
		[LastSupportedVersion("2.0")]
		public static bool IsValid(TickRate rate)
		{
			return Get(rate.Client).Count != 0;
		}

		public static bool IsValid(int rate)
		{
			return rate >= 8 && rate <= 256;
		}

		[Obsolete("TickRate no longer stores rate values, use TickRate.Selection instead.", true)]
		[LastSupportedVersion("2.0")]
		public static TickRate Get(int rate)
		{
			return default(TickRate);
		}

		public static Resolved Resolve(Selection selection)
		{
			ValidateResult validateResult = selection.Validate();
			Assert.Always(validateResult == ValidateResult.Ok, "result == ValidateResult.Ok");
			int num = selection.Client / selection.ServerTickInterval;
			return new Resolved(selection.Client, selection.Client / selection.ClientSendInterval, num, num / selection.ServerSendInterval);
		}
	}
}

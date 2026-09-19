using Features.LevelModule.Scripts;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.WeatherModule.Scripts
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Shared)]
	public sealed class WeatherSelectionModel : NetworkedModelBase
	{
		public Networked<long> Selection { get; } = new Networked<long>();

		public static long Encode(LevelType level, int index)
		{
			return ((long)level << 32) | (uint)(index + 1);
		}

		public static LevelType DecodeLevel(long packed)
		{
			return (LevelType)(packed >> 32);
		}

		public static int DecodeIndex(long packed)
		{
			return (int)(packed & 0xFFFFFFFFu) - 1;
		}
	}
}

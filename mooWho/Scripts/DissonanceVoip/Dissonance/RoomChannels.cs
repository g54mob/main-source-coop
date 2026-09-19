using Dissonance.Audio.Capture;
using JetBrains.Annotations;

namespace Dissonance
{
	public sealed class RoomChannels : Channels<RoomChannel, RoomName>
	{
		internal RoomChannels([NotNull] IChannelPriorityProvider priorityProvider)
			: base(priorityProvider)
		{
			base.OpenedChannel += delegate
			{
			};
			base.ClosedChannel += delegate
			{
			};
		}

		protected override RoomChannel CreateChannel(ushort subscriptionId, RoomName channelId, ChannelProperties properties)
		{
			return new RoomChannel(subscriptionId, channelId, this, properties);
		}
	}
}

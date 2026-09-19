using System.Runtime.CompilerServices;
using Fusion.Sockets.V2;

namespace Fusion.Sockets
{
	public readonly ref struct NetNotifyDataInfo
	{
		private unsafe readonly PacketGroup.StateType* _state;

		public unsafe int FragmentCount
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _state->FragmentCount;
			}
		}

		public unsafe nuint UserData
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _state->UserToken;
			}
		}

		internal unsafe NetNotifyDataInfo(PacketGroup.StateType* state)
		{
			_state = state;
		}
	}
}

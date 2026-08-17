using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
	[AddComponentMenu("")]
	public class NetworkRoomPlayerExt : NetworkRoomPlayer
	{
		public override void OnStartClient()
		{
		}

		public override void OnClientEnterRoom()
		{
		}

		public override void OnClientExitRoom()
		{
		}

		public override void IndexChanged(int oldIndex, int newIndex)
		{
		}

		public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
		{
		}

		public override void OnGUI()
		{
			base.OnGUI();
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}

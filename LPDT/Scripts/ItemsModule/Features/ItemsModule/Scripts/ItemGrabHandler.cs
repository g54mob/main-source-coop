using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ItemGrabHandler : NetworkBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private MonoItem _item;

		public override void Spawned()
		{
			_grabable.OnGrabbedPlayersChanged += ProcessGrabbedPlayersChanged;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_grabable.OnGrabbedPlayersChanged -= ProcessGrabbedPlayersChanged;
		}

		private void ProcessGrabbedPlayersChanged()
		{
			_item.SetLastGrabTime(base.Runner.Tick);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}

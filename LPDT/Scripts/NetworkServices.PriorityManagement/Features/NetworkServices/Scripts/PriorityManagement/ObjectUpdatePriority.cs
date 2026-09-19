using Fusion;
using UnityEngine;

namespace Features.NetworkServices.Scripts.PriorityManagement
{
	[NetworkBehaviourWeaved(0)]
	public class ObjectUpdatePriority : NetworkBehaviour
	{
		[SerializeField]
		private UpdatePriorityTracker _updatePriorityTracker;

		public override void Spawned()
		{
			base.Spawned();
			if (!(_updatePriorityTracker == null))
			{
				_updatePriorityTracker.OnObjectUpdatePriorityChanged += UpdateObjectPriority;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (!(_updatePriorityTracker == null))
			{
				_updatePriorityTracker.OnObjectUpdatePriorityChanged -= UpdateObjectPriority;
			}
		}

		private void UpdateObjectPriority(int priority)
		{
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

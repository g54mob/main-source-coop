using System;
using Fusion;

namespace Features.NetworkServices.Scripts.PriorityManagement
{
	[NetworkBehaviourWeaved(0)]
	public abstract class UpdatePriorityTracker : NetworkBehaviour
	{
		private int _lastFiredPriority = -1;

		public event Action<int> OnObjectUpdatePriorityChanged;

		protected void FirePriorityUpdate(int priority)
		{
			if (_lastFiredPriority != priority)
			{
				_lastFiredPriority = priority;
				this.OnObjectUpdatePriorityChanged?.Invoke(priority);
			}
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

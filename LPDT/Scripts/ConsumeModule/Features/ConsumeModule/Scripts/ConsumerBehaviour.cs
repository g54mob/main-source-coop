using Fusion;
using UnityEngine;

namespace Features.ConsumeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ConsumerBehaviour : NetworkBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<IConsumable>(out var component))
			{
				component.Apply(base.Object.InputAuthority);
			}
			if (other.TryGetComponent<IDestroyOnConsume>(out var component2))
			{
				component2.Destroy();
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

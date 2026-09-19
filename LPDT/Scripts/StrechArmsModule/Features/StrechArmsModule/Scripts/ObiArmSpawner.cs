using Fusion;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ObiArmSpawner : NetworkBehaviour
	{
		[SerializeField]
		private NetworkPrefabRef _armObiPrefab;

		public override void Spawned()
		{
			if (base.HasStateAuthority)
			{
				base.Runner.Spawn(_armObiPrefab, base.transform.position, Quaternion.identity, base.Object.StateAuthority);
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

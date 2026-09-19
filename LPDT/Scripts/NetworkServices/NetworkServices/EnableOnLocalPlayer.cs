using Fusion;
using UnityEngine;

namespace NetworkServices
{
	[NetworkBehaviourWeaved(0)]
	public class EnableOnLocalPlayer : NetworkBehaviour
	{
		[SerializeField]
		private GameObject[] _gameObjectsToEnable;

		public override void Spawned()
		{
			bool active = base.Object.StateAuthority == base.Runner.LocalPlayer;
			GameObject[] gameObjectsToEnable = _gameObjectsToEnable;
			for (int i = 0; i < gameObjectsToEnable.Length; i++)
			{
				gameObjectsToEnable[i].SetActive(active);
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

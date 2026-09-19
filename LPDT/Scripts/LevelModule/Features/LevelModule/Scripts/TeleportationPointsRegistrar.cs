using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class TeleportationPointsRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private List<Transform> _teleportationPoints;

		public List<Transform> TeleportationPoints => _teleportationPoints;

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

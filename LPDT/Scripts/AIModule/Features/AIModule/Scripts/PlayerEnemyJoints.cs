using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerEnemyJoints : NetworkBehaviour
	{
		[SerializeField]
		private List<EnemyJoint> _enemyJoints;

		public List<EnemyJoint> EnemyJoints => _enemyJoints;

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

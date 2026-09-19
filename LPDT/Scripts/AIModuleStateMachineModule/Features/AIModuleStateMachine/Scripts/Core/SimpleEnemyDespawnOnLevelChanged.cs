using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	[NetworkBehaviourWeaved(1)]
	public class SimpleEnemyDespawnOnLevelChanged : EnemyDespawnOnLevelChanged
	{
		[SerializeField]
		private SimpleEnemyDeadProcessor _simpleEnemyDeadProcessor;

		public override IEnemyDeadProcessor _enemyDeadProcessor => _simpleEnemyDeadProcessor;

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}

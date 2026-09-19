using Features.LevelModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class EnemyDespawnOnLevelChanged : DespawnOnLevelChanged
	{
		[SerializeField]
		private SimpleEnemyHealthController _simpleEnemyHealthController;

		protected override void TryDespawn(BeforeLevelChangeNetworkEvent evt)
		{
			_simpleEnemyHealthController.IsNeedToSpawnItem = false;
			base.TryDespawn(evt);
		}

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

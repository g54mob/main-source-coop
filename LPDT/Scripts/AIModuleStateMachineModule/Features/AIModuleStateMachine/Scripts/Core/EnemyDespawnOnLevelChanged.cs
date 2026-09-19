using Features.LevelModule.Scripts;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	[NetworkBehaviourWeaved(1)]
	public abstract class EnemyDespawnOnLevelChanged : DespawnOnLevelChanged
	{
		public abstract IEnemyDeadProcessor _enemyDeadProcessor { get; }

		protected override void TryDespawn(BeforeLevelChangeNetworkEvent evt)
		{
			if (_enemyDeadProcessor != null)
			{
				_enemyDeadProcessor.IsNeedToSpawnItem = false;
			}
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

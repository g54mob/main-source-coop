using Features.AIModuleStateMachine.Scripts.Core.Systems.Damage;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class EarDamageReactionSystem : EnemyDamageReactionSystemBase
	{
		private EarEnemyContext _context;

		[Inject]
		public void InjectDependencies(EarEnemyContext context, SpawnedPlayersModel spawnedPlayersModel)
		{
			_context = context;
			InjectBaseDependencies(spawnedPlayersModel);
		}

		protected override void React(PlayerDataHolder attacker, DamageData damageData)
		{
			if (!(attacker.NetworkObject == null))
			{
				Transform transform = attacker.NetworkObject.transform;
				int playerId = attacker.NetworkObject.InputAuthority.PlayerId;
				_context.TriggerEnemyBySound(new HeardSound(transform.position, float.MaxValue, transform.GetInstanceID(), SoundSourceKind.Player, playerId));
			}
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

using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class EnemyTransformRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private EnemyType _enemyType;

		private EnemyTransformsModel _enemyTransformsModel;

		private IEnemyBehaviour _enemyBehaviour;

		[Inject]
		private void InjectDependency(EnemyTransformsModel enemyTransformsModel)
		{
			_enemyTransformsModel = enemyTransformsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			TryGetComponent<IEnemyBehaviour>(out _enemyBehaviour);
			_enemyTransformsModel.AddEnemy(_enemyType, base.transform, base.Object, _enemyBehaviour);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_enemyTransformsModel?.RemoveEnemy(_enemyType, base.transform, base.Object, _enemyBehaviour);
			_enemyBehaviour = null;
			base.Despawned(runner, hasState);
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

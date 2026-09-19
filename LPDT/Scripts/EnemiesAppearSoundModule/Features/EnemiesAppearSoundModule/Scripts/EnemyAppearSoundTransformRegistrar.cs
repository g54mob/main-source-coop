using Features.AIModule.Scripts;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.EnemiesAppearSoundModule.Scripts
{
	public class EnemyAppearSoundTransformRegistrar : MonoBehaviour
	{
		[SerializeField]
		private EnemyType _enemyType;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private EnemiesAppearSoundTransformsModel _enemiesAppearSoundTransformsModel;

		[Inject]
		private void InjectDependencies(EnemiesAppearSoundTransformsModel enemiesAppearSoundTransformsModel)
		{
			_enemiesAppearSoundTransformsModel = enemiesAppearSoundTransformsModel;
		}

		private void Start()
		{
			_enemiesAppearSoundTransformsModel.AddEnemy(_enemyType, _soundSourceBehaviour);
		}

		private void OnDestroy()
		{
			_enemiesAppearSoundTransformsModel?.RemoveEnemy(_enemyType, _soundSourceBehaviour);
		}
	}
}

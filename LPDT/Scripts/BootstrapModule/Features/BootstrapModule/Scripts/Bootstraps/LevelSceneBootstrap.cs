using Features.LevelModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	public class LevelSceneBootstrap : MonoBehaviour
	{
		[SerializeField]
		private LevelType _levelType;

		private LevelModel _levelModel;

		[Inject]
		public void InjectDependencies(LevelModel levelModel)
		{
			_levelModel = levelModel;
		}

		private void Awake()
		{
			_levelModel.LastLoadedLevel = _levelType;
		}
	}
}

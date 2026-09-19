using UnityEngine;
using Zenject;

namespace Features.LevelGatesModule.Scripts
{
	public class LevelGateRegistrar : MonoBehaviour
	{
		[SerializeField]
		private LevelTipGate _levelTipGate;

		private LevelGatesModel _levelGatesModel;

		[Inject]
		public void InjectDependencies(LevelGatesModel levelGatesModel)
		{
			_levelGatesModel = levelGatesModel;
		}

		private void OnEnable()
		{
			_levelGatesModel.RegisterGate(_levelTipGate);
		}

		private void OnDisable()
		{
			_levelGatesModel.UnregisterGate(_levelTipGate);
		}
	}
}

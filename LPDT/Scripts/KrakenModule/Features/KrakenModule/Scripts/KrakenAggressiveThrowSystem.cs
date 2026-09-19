using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts
{
	public class KrakenAggressiveThrowSystem : MonoBehaviour
	{
		[SerializeField]
		private KrakenController _krakenController;

		private KrakenAggressiveThrowConfiguration _configuration;

		private KrakenPlayerThrowTrackerModel _throwTrackerModel;

		private MultiplayerModel _multiplayerModel;

		private float _lastAggressiveThrowTime = float.NegativeInfinity;

		private bool _isAggressiveThrowInProgress;

		[Inject]
		private void InjectDependencies(KrakenAggressiveThrowConfiguration configuration, KrakenPlayerThrowTrackerModel throwTrackerModel, MultiplayerModel multiplayerModel)
		{
			_configuration = configuration;
			_throwTrackerModel = throwTrackerModel;
			_multiplayerModel = multiplayerModel;
		}

		public bool TryTriggerAggressiveThrow()
		{
			if (_krakenController == null || !_krakenController.HasStateAuthority || _isAggressiveThrowInProgress || _configuration == null || _configuration.KrakenRockPrefab == null)
			{
				return false;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return false;
			}
			if (!_krakenController.IsIdle)
			{
				return false;
			}
			if (Time.time - _lastAggressiveThrowTime < _configuration.AggressiveThrowCooldown)
			{
				return false;
			}
			if (_throwTrackerModel.TotalCountInWindow < _configuration.MinTotalThrowsToTrigger)
			{
				return false;
			}
			if (!_throwTrackerModel.TryGetTopThrower(out var topThrower))
			{
				return false;
			}
			TryStartAggressiveThrowAsync(topThrower).Forget();
			return true;
		}

		private async UniTaskVoid TryStartAggressiveThrowAsync(PlayerRef target)
		{
			_isAggressiveThrowInProgress = true;
			try
			{
				if (await _krakenController.RequestAggressiveRockThrowAsync(target))
				{
					_lastAggressiveThrowTime = Time.time;
				}
			}
			finally
			{
				_isAggressiveThrowInProgress = false;
			}
		}
	}
}

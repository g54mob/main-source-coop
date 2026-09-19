using UnityEngine;
using Zenject;

namespace Features.GamePauseModule.Scripts
{
	public class RigidbodyPauseHandler : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		private GameGlobalNetworkingPause _globalNetworkingPause;

		private bool _wasKinematic;

		private Vector3 _previousAngularVelocity;

		private Vector3 _previousVelocity;

		[Inject]
		public void InjectDependencies(GameGlobalNetworkingPause globalNetworkingPause)
		{
			_globalNetworkingPause = globalNetworkingPause;
		}

		private void OnEnable()
		{
			HandlePause(_globalNetworkingPause.IsGlobalPausedEnable);
			_globalNetworkingPause.OnGlobalPausedChanged += HandlePause;
		}

		private void OnDisable()
		{
			_globalNetworkingPause.OnGlobalPausedChanged -= HandlePause;
		}

		private void HandlePause(bool paused)
		{
			if (paused)
			{
				_wasKinematic = _rigidbody.isKinematic;
				_previousAngularVelocity = _rigidbody.angularVelocity;
				_previousVelocity = _rigidbody.linearVelocity;
				_rigidbody.isKinematic = true;
				_rigidbody.angularVelocity = Vector3.zero;
				_rigidbody.linearVelocity = Vector3.zero;
			}
			else
			{
				_rigidbody.angularVelocity = _previousAngularVelocity;
				_rigidbody.linearVelocity = _previousVelocity;
				_rigidbody.isKinematic = _wasKinematic;
			}
		}
	}
}

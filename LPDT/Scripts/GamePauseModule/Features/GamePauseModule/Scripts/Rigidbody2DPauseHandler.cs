using UnityEngine;
using Zenject;

namespace Features.GamePauseModule.Scripts
{
	public class Rigidbody2DPauseHandler : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody2D _rigidbody;

		private GameGlobalNetworkingPause _globalNetworkingPause;

		private RigidbodyType2D _previousBodyType;

		private Vector2 _previousVelocity;

		private float _previousAngularVelocity;

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
				_previousBodyType = _rigidbody.bodyType;
				_previousAngularVelocity = _rigidbody.angularVelocity;
				_previousVelocity = _rigidbody.linearVelocity;
				_rigidbody.bodyType = RigidbodyType2D.Kinematic;
				_rigidbody.angularVelocity = 0f;
				_rigidbody.linearVelocity = Vector2.zero;
			}
			else
			{
				_rigidbody.angularVelocity = _previousAngularVelocity;
				_rigidbody.linearVelocity = _previousVelocity;
				_rigidbody.bodyType = _previousBodyType;
			}
		}
	}
}

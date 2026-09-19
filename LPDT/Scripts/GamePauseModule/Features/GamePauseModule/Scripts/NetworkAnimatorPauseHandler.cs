using UnityEngine;
using Zenject;

namespace Features.GamePauseModule.Scripts
{
	public class NetworkAnimatorPauseHandler : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;

		private GameGlobalNetworkingPause _globalNetworkingPause;

		private float _animatorSpeed;

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

		private void HandlePause(bool _)
		{
			_animatorSpeed = _globalNetworkingPause.NetworkTimeScale;
			_animator.speed = _animatorSpeed;
		}

		private void Update()
		{
			if (_globalNetworkingPause.IsGlobalPausedEnable)
			{
				_animator.speed = _animatorSpeed;
			}
		}
	}
}

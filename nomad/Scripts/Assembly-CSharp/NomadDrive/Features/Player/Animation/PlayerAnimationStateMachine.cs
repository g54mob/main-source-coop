using System.Collections.Generic;
using NomadDrive.Features.Player.PlayerStateMachine;
using UnityEngine;

namespace NomadDrive.Features.Player.Animation
{
	public class PlayerAnimationStateMachine : MonoBehaviour
	{
		private Animator _animator;

		private AnimationState _currentState;

		private PlayerState _currentStateKey;

		private readonly Dictionary<PlayerState, AnimationState> _states = new Dictionary<PlayerState, AnimationState>();

		public PlayerState CurrentStateKey => _currentStateKey;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
		}

		public void AddState(PlayerState stateKey, AnimationState state)
		{
			if (!_states.ContainsKey(stateKey))
			{
				if ((object)_animator == null)
				{
					_animator = GetComponentInChildren<Animator>();
				}
				state.Initialize(_animator);
				_states.Add(stateKey, state);
			}
		}

		public void ChangeState(PlayerState stateKey, float moveDirection, float moveStrafe, float speed)
		{
			if (_states.TryGetValue(stateKey, out var value))
			{
				if (_currentState == value)
				{
					_currentState.UpdateParameters(moveDirection, moveStrafe, speed);
					return;
				}
				_currentState?.Exit();
				_currentStateKey = stateKey;
				_currentState = value;
				_currentState.Enter();
				_currentState.UpdateParameters(moveDirection, moveStrafe, speed);
			}
		}

		public void UpdateCurrentState(float moveDirection, float moveStrafe, float speed)
		{
			_currentState?.UpdateParameters(moveDirection, moveStrafe, speed);
		}
	}
}

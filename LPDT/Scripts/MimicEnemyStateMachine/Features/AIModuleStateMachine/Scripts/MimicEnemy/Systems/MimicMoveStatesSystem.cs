using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class MimicMoveStatesSystem : MonoSystem
	{
		[SerializeField]
		private NavMeshAreas _crouchArea;

		private MimicEnemyContext _context;

		[SerializeField]
		private float _walkStateMinDuration = 2f;

		[SerializeField]
		private float _walkStateMaxDuration = 5f;

		[SerializeField]
		private float _runStateMinDuration = 1.5f;

		[SerializeField]
		private float _runStateMaxDuration = 4f;

		[SerializeField]
		private float _stateChangeRandomOffset = 0.5f;

		private bool _enabled;

		private bool _isWalking;

		private float _stateTimeRemaining;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context)
		{
			_context = context;
		}

		public override void Enable()
		{
			_context.OnAreaTypeChanged += ProcessAreaTypeChanged;
			_enabled = true;
			PickRandomStateAndDuration();
		}

		public override void Disable()
		{
			_context.OnAreaTypeChanged -= ProcessAreaTypeChanged;
			_enabled = false;
			Clear();
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.Initialized || !_enabled || !base.HasStateAuthority)
			{
				return;
			}
			if (_context.IsCrouching)
			{
				SetMoveSpeed(_context.CrouchSpeed);
				_context.SetIsSprinting(value: false);
				return;
			}
			_stateTimeRemaining -= base.Runner.DeltaTime;
			if (_stateTimeRemaining <= 0f)
			{
				_isWalking = !_isWalking;
				PickRandomStateAndDuration();
			}
			_context.SetIsSprinting(!_isWalking);
			float moveSpeed = (_isWalking ? _context.WalkSpeed : _context.SprintSpeed);
			SetMoveSpeed(moveSpeed);
		}

		private void SetMoveSpeed(float moveSpeed)
		{
			_context.SetMoveSpeed(moveSpeed);
		}

		private void ProcessAreaTypeChanged()
		{
			int num = 1 << NavMesh.GetAreaFromName(_crouchArea.ToString());
			bool flag = (_context.CurrentAreaType & num) != 0;
			if (flag && !_context.IsCrouching)
			{
				_context.NavMeshAgent.velocity = Vector3.zero;
			}
			_context.SetIsCrouching(flag);
		}

		private void PickRandomStateAndDuration()
		{
			if (_isWalking)
			{
				_stateTimeRemaining = Random.Range(_walkStateMinDuration, _walkStateMaxDuration);
			}
			else
			{
				_stateTimeRemaining = Random.Range(_runStateMinDuration, _runStateMaxDuration);
			}
			if (_stateChangeRandomOffset > 0f)
			{
				_stateTimeRemaining += Random.Range(0f - _stateChangeRandomOffset, _stateChangeRandomOffset);
				_stateTimeRemaining = Mathf.Max(0.1f, _stateTimeRemaining);
			}
		}

		public override void Clear()
		{
			_stateTimeRemaining = 0f;
			_isWalking = true;
			if (_context != null)
			{
				_context.SetMoveSpeed(_context.WalkSpeed);
				_context.SetIsSprinting(value: false);
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

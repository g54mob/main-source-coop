using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class MimicMoveStatesSetupSystem : MonoSystem
	{
		private MimicEnemyContext _context;

		[SerializeField]
		private SerializableDictionary<MimicStateId, MoveStatesData> _statesData;

		private MimicEnemy _stateMachine;

		[SerializeField]
		private MoveStatesData _defaultValue;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context, MimicEnemy stateMachine)
		{
			_context = context;
			_stateMachine = stateMachine;
		}

		public override void Enable()
		{
			_enabled = true;
			MoveStatesData moveStatesData = (_statesData.ContainsKey(_stateMachine.CurrentStateId) ? _statesData[_stateMachine.CurrentStateId] : _defaultValue);
			ApplyMoveStatesData(moveStatesData);
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
		}

		private void ApplyMoveStatesData(MoveStatesData moveStatesData)
		{
			_context.CrouchSpeed = moveStatesData.CrouchSpeed;
			_context.SprintSpeed = moveStatesData.SprintSpeed;
			_context.WalkSpeed = moveStatesData.WalkSpeed;
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

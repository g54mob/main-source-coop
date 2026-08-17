using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class VehicleDoorWindowCrank : VehicleInteractable
	{
		public UnityEvent onCrankTurned = new UnityEvent();

		[SerializeField]
		[Min(0.01f)]
		private float turnsCount = 3f;

		[SerializeField]
		private float duration = 0.3f;

		[SerializeField]
		private Ease ease;

		private Quaternion _closedRotation;

		private bool _isInOpenPosition;

		private bool _isTurning;

		private Tween _tween;

		private StaticInteractionStateMachine<VehicleDoorWindowCrankState> _stateMachine;

		protected override bool UseStateMachine => true;

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<VehicleDoorWindowCrankState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(VehicleDoorWindowCrankState.Ready);
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(VehicleDoorWindowCrankState.Ready, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.pull", TurnCrank).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(VehicleDoorWindowCrankState.Ready);
		}

		protected override void Awake()
		{
			base.Awake();
			_closedRotation = base.transform.localRotation;
		}

		private void TurnCrank()
		{
			if (!_isTurning)
			{
				_isTurning = true;
				bool flag = (_isInOpenPosition = !_isInOpenPosition);
				float endValue = 360f * turnsCount * (flag ? (-1f) : 1f);
				_tween = Tween.Custom(this, 0f, endValue, duration, delegate(VehicleDoorWindowCrank t, float angle)
				{
					t.transform.localRotation = t._closedRotation * Quaternion.Euler(angle, 0f, 0f);
				}, ease).OnComplete(this, OnTurnComplete);
				onCrankTurned.Invoke();
			}
		}

		private static void OnTurnComplete(VehicleDoorWindowCrank target)
		{
			target.transform.localRotation = target._closedRotation;
			target._isTurning = false;
			target.UpdateState();
		}

		public void SetCrankPosition(bool isOpen)
		{
			_isInOpenPosition = isOpen;
			base.transform.localRotation = _closedRotation;
		}
	}
}

using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Vehicle.Enums;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class DoorHandle : VehicleInteractable
	{
		public UnityEvent onHandlePulled = new UnityEvent();

		[SerializeField]
		private ControlRotationAxis rotationAxis;

		[SerializeField]
		private float rotationAngle = 15f;

		[SerializeField]
		private float animationDuration = 0.2f;

		[SerializeField]
		private Ease animationEase = Ease.OutCubic;

		private Transform _animationTarget;

		private Quaternion _originalRotation;

		private Sequence _sequence;

		private bool _isPulling;

		private StaticInteractionStateMachine<DoorHandleState> _stateMachine;

		protected override bool UseStateMachine => true;

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<DoorHandleState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DoorHandleState.Ready);
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(DoorHandleState.Ready, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.pull", PullHandle).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DoorHandleState.Ready);
		}

		protected override void Awake()
		{
			base.Awake();
			_animationTarget = ((base.ModelTransform != null) ? base.ModelTransform : base.transform);
			_originalRotation = _animationTarget.localRotation;
			_isPulling = false;
		}

		private void PullHandle()
		{
			if (TryStartPullAnimation())
			{
				onHandlePulled.Invoke();
			}
		}

		public void PlayPullAnimation()
		{
			TryStartPullAnimation();
		}

		private bool TryStartPullAnimation()
		{
			if (_isPulling)
			{
				return false;
			}
			_isPulling = true;
			PullHandleAnimation();
			return true;
		}

		private void PullHandleAnimation()
		{
			_sequence.Stop();
			_animationTarget.localRotation = _originalRotation;
			Vector3 axisVector = GetAxisVector(rotationAxis);
			Quaternion endValue = _originalRotation * Quaternion.AngleAxis(rotationAngle, axisVector);
			float duration = Mathf.Max(animationDuration * 0.5f, 0.0001f);
			_sequence = Sequence.Create().Chain(Tween.LocalRotation(_animationTarget, endValue, duration, animationEase, 2, CycleMode.Yoyo)).ChainCallback(this, OnPullAnimationComplete);
		}

		private static Vector3 GetAxisVector(ControlRotationAxis axis)
		{
			return axis switch
			{
				ControlRotationAxis.X => Vector3.right, 
				ControlRotationAxis.Y => Vector3.up, 
				ControlRotationAxis.Z => Vector3.forward, 
				_ => Vector3.right, 
			};
		}

		private static void OnPullAnimationComplete(DoorHandle target)
		{
			target._isPulling = false;
			target.UpdateState();
		}
	}
}

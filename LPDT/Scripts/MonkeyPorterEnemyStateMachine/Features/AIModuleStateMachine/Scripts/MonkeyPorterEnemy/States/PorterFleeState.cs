using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Services;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.States
{
	public class PorterFleeState : StateBase<MonkeyPorterStateId>
	{
		private const float COVER_ARRIVE_THRESHOLD = 0.6f;

		private const float COVER_HIDE_MAX_DISTANCE = 1.2f;

		private const float COVER_STOPPING_DISTANCE = 0.4f;

		private const float DOCK_ARRIVE_THRESHOLD = 0.35f;

		private const float DOCK_ANGLE_TOLERANCE = 5f;

		private const float DOCK_TURN_SPEED = 240f;

		private const float DOCK_TIMEOUT = 2f;

		private const float FORCE_STEP_DISTANCE = 3f;

		private const float FORCE_STEP_EPSILON = 0.0025f;

		private const int FLEE_AVOIDANCE_PRIORITY = 10;

		private readonly MonkeyPorterEnemy _monkeyPorterEnemy;

		private readonly MonkeyPorterContext _monkeyPorterContext;

		private readonly MonkeyPorterSettings _monkeyPorterSettings;

		private readonly PorterCoverFinder _porterCoverFinder;

		private PorterFleePhase _phase;

		private IEnemyTrackable _lockedThreat;

		private Vector3 _destination;

		private bool _hasDestination;

		private bool _isCoverDestination;

		private bool _isAdjustingCartItem;

		private float _phaseElapsed;

		private float _fleeElapsed;

		private float _coverRetryElapsed;

		private float _hidingElapsed;

		private int _originalAvoidancePriority;

		public PorterFleeState(MonkeyPorterEnemy monkeyPorterEnemy, MonkeyPorterContext monkeyPorterContext, MonkeyPorterSettings monkeyPorterSettings, PorterCoverFinder porterCoverFinder)
			: base(false, false)
		{
			_monkeyPorterEnemy = monkeyPorterEnemy;
			_monkeyPorterContext = monkeyPorterContext;
			_monkeyPorterSettings = monkeyPorterSettings;
			_porterCoverFinder = porterCoverFinder;
		}

		public override void OnEnter()
		{
			_phase = PorterFleePhase.Signal;
			_lockedThreat = _monkeyPorterEnemy.ThreatDetector.Threat;
			_phaseElapsed = 0f;
			_fleeElapsed = 0f;
			_coverRetryElapsed = 0f;
			_hidingElapsed = 0f;
			_hasDestination = false;
			_isCoverDestination = false;
			_isAdjustingCartItem = false;
			_porterCoverFinder.Reset();
			_monkeyPorterEnemy.SetHiddenInCover(isHidden: false);
			_monkeyPorterEnemy.SuspendCartFollow(isSuspended: true);
			_monkeyPorterEnemy.BlockDetector.Reset();
			_monkeyPorterContext.SetSpeed(_monkeyPorterSettings.FleeSpeed);
			_monkeyPorterContext.SetStoppingDistance(0.4f);
			_monkeyPorterContext.StopAgent();
			_originalAvoidancePriority = _monkeyPorterContext.AvoidancePriority;
			_monkeyPorterContext.SetAvoidancePriority(10);
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Panic);
			_monkeyPorterEnemy.NoiseEmitter.EmitPanic();
		}

		public override void OnExit()
		{
			_lockedThreat = null;
			_monkeyPorterEnemy.SetHiddenInCover(isHidden: false);
			_monkeyPorterEnemy.SuspendCartFollow(isSuspended: false);
			_monkeyPorterContext.SetStoppingDistance(_monkeyPorterSettings.StoppingDistance);
			_monkeyPorterContext.SetAvoidancePriority(_originalAvoidancePriority);
		}

		public override void OnLogic()
		{
			float tickDelta = _monkeyPorterEnemy.GetTickDelta();
			_phaseElapsed += tickDelta;
			_fleeElapsed += tickDelta;
			if (_fleeElapsed >= _monkeyPorterSettings.MaxFleeTime)
			{
				FinishReturn();
				return;
			}
			switch (_phase)
			{
			case PorterFleePhase.Signal:
				TickSignal();
				break;
			case PorterFleePhase.RunToCover:
				TickRunToCover(tickDelta);
				break;
			case PorterFleePhase.Hiding:
				TickHiding(tickDelta);
				break;
			case PorterFleePhase.ReturnToCart:
				TickReturnToCart();
				break;
			case PorterFleePhase.DockToCart:
				TickDockToCart(tickDelta);
				break;
			}
		}

		private void TickSignal()
		{
			RefreshLockedThreat();
			if (_monkeyPorterEnemy.ThreatDetector.IsThreatWithin(_lockedThreat, _monkeyPorterSettings.ThreatOverrideRadius) || !(_phaseElapsed < _monkeyPorterSettings.SignalDuration))
			{
				EnterPhase(PorterThreatDetector.IsThreatAlive(_lockedThreat) ? PorterFleePhase.RunToCover : PorterFleePhase.ReturnToCart);
			}
		}

		private void TickRunToCover(float deltaTime)
		{
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Panic);
			RefreshLockedThreat();
			if (!PorterThreatDetector.IsThreatAlive(_lockedThreat))
			{
				EnterPhase(PorterFleePhase.ReturnToCart);
				return;
			}
			bool flag = _monkeyPorterEnemy.ThreatDetector.IsThreatWithin(_lockedThreat, _monkeyPorterSettings.ThreatOverrideRadius);
			if (_phaseElapsed >= _monkeyPorterSettings.MaxRunToCoverTime && !flag)
			{
				EnterPhase(PorterFleePhase.ReturnToCart);
				return;
			}
			if (HasArrivedAtDestination())
			{
				if (_isCoverDestination)
				{
					EnterPhase(PorterFleePhase.Hiding);
					return;
				}
				DropDestination(isCoverUnreachable: false);
			}
			if (_hasDestination && IsStalled(deltaTime))
			{
				DropDestination(isCoverUnreachable: true);
			}
			if (_hasDestination && _monkeyPorterContext.HasActivePath)
			{
				if (_isCoverDestination)
				{
					return;
				}
				_coverRetryElapsed -= deltaTime;
				if (_coverRetryElapsed > 0f)
				{
					return;
				}
			}
			_coverRetryElapsed = _monkeyPorterSettings.CoverRetryInterval;
			Vector3 threatPosition = _monkeyPorterEnemy.ThreatDetector.GetThreatPosition(_lockedThreat);
			if (!TryTakeEscapeDestination(threatPosition))
			{
				ForceStepAwayFrom(threatPosition);
			}
		}

		private bool TryTakeEscapeDestination(Vector3 threatPosition)
		{
			if (_porterCoverFinder.TryFindCover(threatPosition, out var coverPosition))
			{
				SetDestination(coverPosition, isCover: true);
				return true;
			}
			if (_monkeyPorterContext.TryGetDirectedFleePosition(threatPosition, _monkeyPorterSettings.FleeFallbackRadius, mustIncreaseThreatDistance: true, out var position))
			{
				SetDestination(position, isCover: false);
				return true;
			}
			if (_monkeyPorterContext.TryGetDirectedFleePosition(threatPosition, _monkeyPorterSettings.FleeFallbackRadius, mustIncreaseThreatDistance: false, out var position2))
			{
				SetDestination(position2, isCover: false);
				return true;
			}
			if (_monkeyPorterContext.TryGetNearestPlayerPosition(out var position3, out var _))
			{
				SetDestination(position3, isCover: false);
				return true;
			}
			return false;
		}

		private void ForceStepAwayFrom(Vector3 threatPosition)
		{
			Vector3 position = _monkeyPorterContext.transform.position;
			Vector3 vector = position - threatPosition;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0025f)
			{
				vector = _monkeyPorterContext.transform.forward;
			}
			SetDestination(position + vector.normalized * 3f, isCover: false);
		}

		private bool IsStalled(float deltaTime)
		{
			if (!_monkeyPorterEnemy.BlockDetector.IsStalled(deltaTime, _monkeyPorterSettings.FleeStuckTime))
			{
				return false;
			}
			_monkeyPorterEnemy.BlockDetector.Reset();
			return true;
		}

		private void DropDestination(bool isCoverUnreachable)
		{
			if (isCoverUnreachable && _isCoverDestination)
			{
				_porterCoverFinder.RejectLastCover();
			}
			_hasDestination = false;
			_isCoverDestination = false;
			_coverRetryElapsed = 0f;
		}

		private bool HasArrivedAtDestination()
		{
			if (!_hasDestination)
			{
				return false;
			}
			float num = _monkeyPorterContext.HorizontalDistanceTo(_destination);
			if (num <= 0.6f)
			{
				return true;
			}
			if (!_monkeyPorterContext.AgentReachedDestination(0.6f))
			{
				return false;
			}
			if (_isCoverDestination)
			{
				return num <= 1.2f;
			}
			return true;
		}

		private void SetDestination(Vector3 destination, bool isCover)
		{
			_destination = destination;
			_hasDestination = true;
			_isCoverDestination = isCover;
			_monkeyPorterEnemy.BlockDetector.Reset();
			_monkeyPorterContext.MoveToPosition(_destination);
		}

		private void TickHiding(float deltaTime)
		{
			_monkeyPorterContext.StopAgent();
			_monkeyPorterEnemy.SetHiddenInCover(isHidden: true);
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Hiding);
			_hidingElapsed += deltaTime;
			if (_hidingElapsed >= _monkeyPorterSettings.MaxHideTime)
			{
				LeaveCover();
			}
			else if (!(_hidingElapsed < _monkeyPorterSettings.HideHoldTime) && !_monkeyPorterEnemy.ThreatDetector.IsAnyThreatWithin(_monkeyPorterSettings.ThreatClearRadius))
			{
				LeaveCover();
			}
		}

		private void LeaveCover()
		{
			_monkeyPorterEnemy.SetHiddenInCover(isHidden: false);
			EnterPhase(PorterFleePhase.ReturnToCart);
		}

		private void TickReturnToCart()
		{
			IEnemyTrackable threat = _monkeyPorterEnemy.ThreatDetector.Threat;
			if (_monkeyPorterEnemy.ThreatDetector.IsThreatWithin(threat, _monkeyPorterSettings.ThreatOverrideRadius))
			{
				_lockedThreat = threat;
				EnterPhase(PorterFleePhase.RunToCover);
				return;
			}
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Moving);
			if (!_monkeyPorterEnemy.TryGetCartDockPose(out var position, out var _))
			{
				FinishReturn();
				return;
			}
			if (_monkeyPorterContext.HorizontalDistanceTo(position) <= 0.35f)
			{
				EnterPhase(PorterFleePhase.DockToCart);
				return;
			}
			if (_phaseElapsed >= _monkeyPorterSettings.MaxRunToCoverTime)
			{
				EnterPhase(PorterFleePhase.DockToCart);
				return;
			}
			float tickDelta = _monkeyPorterEnemy.GetTickDelta();
			if (IsStalled(tickDelta))
			{
				FinishReturn();
				return;
			}
			_coverRetryElapsed -= tickDelta;
			if (!(_coverRetryElapsed > 0f))
			{
				_coverRetryElapsed = _monkeyPorterSettings.CoverRetryInterval;
				_monkeyPorterContext.MoveToPosition(position);
			}
		}

		private void TickDockToCart(float deltaTime)
		{
			_monkeyPorterContext.StopAgent();
			Vector3 position;
			Vector3 forward;
			if (_isAdjustingCartItem)
			{
				if (_monkeyPorterEnemy.IsCartItemAdjustFinished)
				{
					FinishReturn();
				}
			}
			else if (!_monkeyPorterEnemy.TryGetCartDockPose(out position, out forward))
			{
				FinishReturn();
			}
			else if (!(_monkeyPorterContext.RotateTowards(forward, 240f, deltaTime) > 5f) || !(_phaseElapsed < 2f))
			{
				if (_monkeyPorterEnemy.TryStartCartItemAdjust())
				{
					_isAdjustingCartItem = true;
				}
				else
				{
					FinishReturn();
				}
			}
		}

		private void RefreshLockedThreat()
		{
			if (!PorterThreatDetector.IsThreatAlive(_lockedThreat))
			{
				_lockedThreat = _monkeyPorterEnemy.ThreatDetector.Threat;
			}
		}

		private void FinishReturn()
		{
			_monkeyPorterEnemy.TryAdoptOrphanCartItem();
			_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnThreatGone);
		}

		private void EnterPhase(PorterFleePhase phase)
		{
			_phase = phase;
			_phaseElapsed = 0f;
			_coverRetryElapsed = 0f;
			_hidingElapsed = 0f;
			_hasDestination = false;
			_isCoverDestination = false;
			_isAdjustingCartItem = false;
			_monkeyPorterEnemy.BlockDetector.Reset();
			if (phase == PorterFleePhase.RunToCover)
			{
				_porterCoverFinder.Reset();
				_monkeyPorterContext.SetSpeed(_monkeyPorterSettings.FleeSpeed);
				_monkeyPorterEnemy.SetHiddenInCover(isHidden: false);
			}
			if (phase == PorterFleePhase.ReturnToCart)
			{
				_monkeyPorterContext.SetSpeed(_monkeyPorterSettings.MoveSpeed);
			}
		}
	}
}

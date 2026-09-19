using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.NetworkedModelCodegen.Scripts;
using UnityEngine;

namespace Features.SessionManagementModule.Models
{
	[NetworkedModel(ModelScope.Session, ModelOwnership.Shared)]
	public sealed class SessionStateMachine : NetworkedModelBase, ISessionStateContext
	{
		private readonly ISessionAuthorityGate _authorityGate;

		private readonly ISynchronizationGate _synchronizationGate;

		private readonly IReadOnlyDictionary<SessionState, ISessionState> _states;

		private readonly ISessionProfiler _profiler;

		private readonly ISessionRecoveryController _sessionRecoveryController;

		private readonly SessionStateSnapshot _sessionStateSnapshot;

		private ISessionState _current;

		private SessionState _previousState;

		public Networked<SessionState> Target { get; } = new Networked<SessionState>();

		public Networked<bool> IsTargetActive { get; } = new Networked<bool>();

		public SessionState Current => _current?.State ?? SessionState.None;

		public SessionState PreviousState => _previousState;

		public Networked<int> Epoch { get; } = new Networked<int>();

		public int CurrentEpoch => Epoch.Value;

		public SessionSubstate Substate { get; private set; }

		public bool IsActive => Substate == SessionSubstate.Active;

		public bool IsTargetStateActive
		{
			get
			{
				return IsTargetActive.Value;
			}
			set
			{
				IsTargetActive.Value = value;
			}
		}

		bool ISessionStateContext.IsAuthority => base.IsAuthority;

		public event Action<SessionState> CurrentChanged;

		public SessionStateMachine(ISessionAuthorityGate authorityGate, ISynchronizationGate synchronizationGate, List<ISessionState> states, ISessionProfiler profiler, SessionStateSnapshot sessionStateSnapshot, ISessionRecoveryController sessionRecoveryController)
		{
			_authorityGate = authorityGate;
			_synchronizationGate = synchronizationGate;
			_profiler = profiler;
			_sessionStateSnapshot = sessionStateSnapshot;
			_sessionRecoveryController = sessionRecoveryController;
			Dictionary<SessionState, ISessionState> dictionary = new Dictionary<SessionState, ISessionState>(states.Count);
			foreach (ISessionState state in states)
			{
				dictionary[state.State] = state;
				state.Bind(this);
			}
			_states = dictionary;
			base.AttachmentChanged += AttachmentChangedHandler;
		}

		public void Transition(SessionState target)
		{
			if (!base.IsAuthority)
			{
				Debug.LogError(string.Format("{0}: state transition to {1} was requested without authority over the session state; the request was ignored.", "SessionStateMachine", target));
			}
			else if (Substate == SessionSubstate.Enter || Substate == SessionSubstate.Exit)
			{
				Debug.LogWarning(string.Format("{0}: state transition to {1} was requested while a transition is in flight (substate={2}); the request was ignored.", "SessionStateMachine", target, Substate));
			}
			else if (Target.Value != target)
			{
				Epoch.Value++;
				Target.Value = target;
			}
		}

		public UniTask AuthorityGate(SessionAuthorityLane lane, Func<UniTask> work)
		{
			return _authorityGate.PassAsync((int)lane, Epoch.Value, work);
		}

		public UniTask AuthorityGate(SessionAuthorityLane lane, Action work)
		{
			return _authorityGate.PassAsync((int)lane, Epoch.Value, delegate
			{
				work();
				return UniTask.CompletedTask;
			});
		}

		public UniTask AuthorityGate(SessionAuthorityLane lane, Action<int> work)
		{
			int epoch = Epoch.Value;
			return _authorityGate.PassAsync((int)lane, epoch, delegate
			{
				work(epoch);
				return UniTask.CompletedTask;
			});
		}

		public UniTask AuthorityGate(SessionAuthorityLane lane, Func<int, UniTask> work)
		{
			int epoch = Epoch.Value;
			return _authorityGate.PassAsync((int)lane, epoch, () => work(epoch));
		}

		public UniTask SynchronizationGate(SynchronizationGateKey key)
		{
			return _synchronizationGate.WaitUntilPassedAsync(key);
		}

		protected override void OnAttach()
		{
			_sessionStateSnapshot.Activate(Time.realtimeSinceStartupAsDouble);
			if (base.IsAuthority)
			{
				Transition(SessionState.Lobby);
			}
		}

		protected override void OnFusionUpdate()
		{
			DriveTransition();
			_sessionStateSnapshot.Observe(Target.Value, IsTargetActive.Value, base.IsAuthority, Current, Substate, Time.realtimeSinceStartupAsDouble);
		}

		private void AttachmentChangedHandler(bool isAttached)
		{
			if (!isAttached)
			{
				_sessionStateSnapshot.Deactivate();
			}
		}

		private void DriveTransition()
		{
			if (Substate == SessionSubstate.Enter || Substate == SessionSubstate.Exit)
			{
				return;
			}
			_states.TryGetValue(Target.Value, out var value);
			if (value != _current)
			{
				RunTransitionAsync().Forget();
			}
			else
			{
				if (Substate != SessionSubstate.Active)
				{
					return;
				}
				try
				{
					_current?.UpdateActiveLocal();
					if (base.IsAuthority)
					{
						_current?.UpdateActive();
					}
				}
				catch (SessionRecoveryRequestedException ex)
				{
					_sessionRecoveryController.RequestRecovery(ex.Reason);
				}
			}
		}

		private async UniTask RunTransitionAsync()
		{
			_ = 5;
			try
			{
				SessionState next = Target.Value;
				await using (_profiler.Sample(SessionPhase.LocalStateExit))
				{
					if (_current != null)
					{
						Substate = SessionSubstate.Exit;
						await AuthorityGate(_current.DeactivateLane, (Action)delegate
						{
							IsTargetStateActive = false;
						});
						await _current.ExitAsync(next);
					}
				}
				SessionState sessionState = Target.Value;
				if (sessionState != next && next == SessionState.Lobby)
				{
					sessionState = next;
				}
				_states.TryGetValue(sessionState, out var value);
				await using (_profiler.Sample(SessionPhase.LocalStateEnter))
				{
					_previousState = Current;
					_current = value;
					Substate = SessionSubstate.Enter;
					this.CurrentChanged?.Invoke(sessionState);
					if (_current != null)
					{
						await _current.EnterAsync();
						await AuthorityGate(_current.ActivateLane, (Action)delegate
						{
							IsTargetStateActive = true;
						});
					}
				}
			}
			catch (SessionRosterRejectedException ex)
			{
				_sessionStateSnapshot.MarkRosterRejected();
				Debug.LogWarning("SessionStateMachine: " + ex.Message);
				return;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			Substate = SessionSubstate.Active;
		}
	}
}

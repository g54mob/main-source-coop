using System.Collections;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using UnityEngine.Events;

namespace EvilCore
{
	public class GameLoadingManager : MonoBehaviour, IGameLoadingManager
	{
		[Header("Settings")]
		[SerializeField]
		private float timeoutDuration = 60f;

		[SerializeField]
		private bool enableDebugLogs = true;

		[Header("State")]
		[SerializeField]
		private int currentState;

		[SerializeField]
		private float stateStartTime;

		[SerializeField]
		private bool isTimedOut;

		[Header("Events")]
		[SerializeField]
		private UnityEvent<int> onStateChanged = new UnityEvent<int>();

		[SerializeField]
		private UnityEvent onLoadingComplete = new UnityEvent();

		[SerializeField]
		private UnityEvent onTimeout = new UnityEvent();

		private readonly Dictionary<int, string> _stateDescriptions = new Dictionary<int, string>
		{
			{ 0, "Initializing" },
			{ 10, "Connecting" },
			{ 20, "Spawning player" },
			{ 90, "Finalizing" },
			{ 100, "Ready" }
		};

		private Coroutine _timeoutCoroutine;

		UnityEvent<int> IGameLoadingManager.OnStateChanged => onStateChanged;

		UnityEvent IGameLoadingManager.OnLoadingComplete => onLoadingComplete;

		UnityEvent IGameLoadingManager.OnTimeout => onTimeout;

		private void Awake()
		{
			stateStartTime = Time.time;
		}

		public void RegisterStateDescription(int stateId, string description)
		{
			_stateDescriptions[stateId] = description;
		}

		public string GetStateDescription(int state)
		{
			if (!_stateDescriptions.TryGetValue(state, out var value))
			{
				return $"State {state}";
			}
			return value;
		}

		public void SetState(int newState)
		{
			if (currentState != 100 || newState >= 100)
			{
				_ = currentState;
				currentState = newState;
				stateStartTime = Time.time;
				_ = enableDebugLogs;
				onStateChanged.Invoke(newState);
				if (_timeoutCoroutine != null)
				{
					StopCoroutine(_timeoutCoroutine);
				}
				_timeoutCoroutine = StartCoroutine(TimeoutCoroutine());
				if (newState == 100)
				{
					OnLoadingCompleted();
				}
			}
		}

		public int GetCurrentState()
		{
			return currentState;
		}

		public bool IsLoadingComplete()
		{
			return currentState == 100;
		}

		public bool IsTimedOut()
		{
			return isTimedOut;
		}

		public void SuspendTimeout()
		{
			if (_timeoutCoroutine != null)
			{
				StopCoroutine(_timeoutCoroutine);
				_timeoutCoroutine = null;
			}
		}

		private void OnLoadingCompleted()
		{
			if (_timeoutCoroutine != null)
			{
				StopCoroutine(_timeoutCoroutine);
				_timeoutCoroutine = null;
			}
			_ = Time.time;
			_ = stateStartTime;
			onLoadingComplete.Invoke();
		}

		private IEnumerator TimeoutCoroutine()
		{
			yield return new WaitForSeconds(timeoutDuration);
			if (currentState != 100)
			{
				isTimedOut = true;
				EvilLogger.LogError($"<color=red>[LoadingManager]</color> TIMEOUT! Stuck at state: {currentState} ({GetStateDescription(currentState)})", "TimeoutCoroutine", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Scripts\\GameLoadingManager.cs", 137);
				currentState = 100;
				_timeoutCoroutine = null;
				onLoadingComplete.Invoke();
				onTimeout.Invoke();
			}
		}

		public void Reset()
		{
			if (_timeoutCoroutine != null)
			{
				StopCoroutine(_timeoutCoroutine);
				_timeoutCoroutine = null;
			}
			currentState = 0;
			stateStartTime = Time.time;
			isTimedOut = false;
			_ = enableDebugLogs;
		}

		private void OnDestroy()
		{
			if (_timeoutCoroutine != null)
			{
				StopCoroutine(_timeoutCoroutine);
			}
		}
	}
}

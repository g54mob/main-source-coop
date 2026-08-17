using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class Pause : StaticInstance<Pause>
{
	public UnityEvent onGamePaused;

	public UnityEvent onGameResumed;

	private bool _paused;

	private bool _pauseEnabled;

	private Player _uiPlayer;

	public bool Paused => _paused;

	public void SetEnablePause(bool v)
	{
		_pauseEnabled = v;
	}

	protected override void Awake()
	{
		base.Awake();
		_pauseEnabled = true;
		onGameResumed = new UnityEvent();
		onGamePaused = new UnityEvent();
		_uiPlayer = ReInput.players.GetPlayer(3);
		_uiPlayer.AddInputEventDelegate(SendPauseEvent, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, 13);
	}

	private void SendPauseEvent(InputActionEventData obj)
	{
		if (_pauseEnabled)
		{
			StaticInstance<UIManager>.Instance.SendEvent(UIStateMachineEvents.Pause);
		}
	}

	public void SetPause(bool value)
	{
		if (value)
		{
			PauseGame();
		}
		else
		{
			ResumeGame();
		}
	}

	private void OnDestroy()
	{
		onGamePaused.RemoveAllListeners();
		onGameResumed.RemoveAllListeners();
		_uiPlayer.RemoveInputEventDelegate(SendPauseEvent);
	}

	private void PauseGame()
	{
		_paused = true;
		onGamePaused?.Invoke();
		Time.timeScale = 0f;
		StaticInstance<Bread>.Instance.DisableInput();
		StaticInstance<Fred>.Instance.DisableInput();
		StaticInstance<CameraController>.Instance.SetBlur(value: true);
	}

	public void ResumeGame()
	{
		_paused = false;
		onGameResumed?.Invoke();
		Time.timeScale = 1f;
		StaticInstance<Bread>.Instance.EnableInput();
		StaticInstance<Fred>.Instance.EnableInput();
		StaticInstance<CameraController>.Instance.SetBlur(value: false);
	}
}

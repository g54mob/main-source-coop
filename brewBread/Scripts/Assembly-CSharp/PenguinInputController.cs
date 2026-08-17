using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class PenguinInputController : MonoBehaviour
{
	private Player _player;

	[SerializeField]
	private int _playerID;

	[SerializeField]
	private PenguinActionsController _controller;

	public static UnityEvent<PenguinInputController, PenguinActionsController> OnGrabInput;

	public static UnityEvent<PenguinInputController, PenguinActionsController> OnUnGrabInput;

	public static UnityEvent<int, PenguinActionsController> OnEmotePlayed;

	public bool _throwing;

	private void Awake()
	{
		_player = ReInput.players.GetPlayer(_playerID);
		OnEmotePlayed = new UnityEvent<int, PenguinActionsController>();
		OnGrabInput = new UnityEvent<PenguinInputController, PenguinActionsController>();
		OnUnGrabInput = new UnityEvent<PenguinInputController, PenguinActionsController>();
	}

	private void Update()
	{
		float axisRaw = _player.GetAxisRaw(0);
		if (axisRaw != 0f)
		{
			_controller.SendPlayerMakerEvent("Moving");
			_controller.Movement = axisRaw;
		}
		else
		{
			_controller.SendPlayerMakerEvent("StopMoving");
			_controller.Movement = 0f;
		}
		_controller.SwingMovement = _player.GetAxisRaw(22);
		if (_player.GetButtonDown(1) && _controller.JumpEnabled && _controller.EnableToJump)
		{
			_controller.SendPlayerMakerEvent("Jump");
		}
		if (_player.GetButton(2))
		{
			if (_controller.CurrentState.Type == PenguinActions.Swing)
			{
				if (_controller.transform.position.y > _controller.CompanionTransform.position.y)
				{
					_controller.SendPlayerMakerEvent("Anchor");
				}
			}
			else
			{
				_controller.SendPlayerMakerEvent("Anchor");
			}
		}
		if (_player.GetButtonUp(2))
		{
			_controller.SendPlayerMakerEvent("UnAnchor");
		}
		if (_player.GetButtonDown(3) && _controller.CurrentState.Type != PenguinActions.PullRope)
		{
			OnGrabInput?.Invoke(this, _controller);
			_controller.SendPlayerMakerEvent("Grab");
		}
		if (!_player.GetButton(3) && _controller.CurrentState.Type != PenguinActions.PullRope)
		{
			_controller.SendPlayerMakerEvent("UnGrab");
		}
		if (_player.GetButtonUp(3) && _controller.CurrentState.Type != PenguinActions.PullRope)
		{
			OnUnGrabInput?.Invoke(this, _controller);
		}
		if (_player.GetButtonDown(4) && StaticInstance<AssistModeManager>.Instance.PullingRope && _controller.Companion.CurrentState.Type != PenguinActions.Grabbed)
		{
			_controller.SendPlayerMakerEvent("PullRope");
		}
		if (_player.GetButtonUp(4) && StaticInstance<AssistModeManager>.Instance.PullingRope)
		{
			_controller.SendPlayerMakerEvent("UnPullRope");
		}
		if (_player.GetButtonDown(7))
		{
			OnEmotePlayed?.Invoke(1, _controller);
		}
		if (_player.GetButtonDown(8))
		{
			OnEmotePlayed?.Invoke(2, _controller);
		}
		if (_player.GetButtonDown(9))
		{
			OnEmotePlayed?.Invoke(3, _controller);
		}
		if (_player.GetButtonDown(10))
		{
			OnEmotePlayed?.Invoke(4, _controller);
		}
		if (StaticInstance<AssistModeManager>.Instance.CheckPoints)
		{
			if (_player.GetButtonDown(5) && CheckpointSystemChecks())
			{
				StaticInstance<CheckpointController>.Instance.SaveGame();
			}
			if (_player.GetButtonDown(6) && CheckpointSystemChecks())
			{
				StaticInstance<CheckpointController>.Instance.LoadGame();
			}
		}
	}

	private bool CheckpointSystemChecks()
	{
		bool num = _controller.IsGrounded || _controller.CurrentState.Type == PenguinActions.Grabbed;
		bool flag = _controller.Companion.IsGrounded || _controller.Companion.CurrentState.Type == PenguinActions.Grabbed;
		return num && flag;
	}
}

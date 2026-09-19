using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerModelController))]
public class LobbyIntroController : NetworkBehaviour
{
	[Header("Geçiş Süreleri")]
	[Tooltip("Sinematik ↔ oyuncu kamerası arasındaki siyah perde süresi")]
	public float cameraFadeDuration = 0.25f;

	[Tooltip("Animator Speed_f'in 0↔1 arası yumuşama süresi (PlayerController.animDampTime ile aynı mantık)")]
	public float animRampDuration = 0.25f;

	private static readonly int _hashSpeedF;

	private PlayerController _playerController;

	private CharacterController _characterController;

	private NetworkedCameraController _networkedCam;

	private PlayerModelController _modelController;

	private AudioListener _playerListener;

	private Animator _anim
	{
		get
		{
			if (!(_modelController != null))
			{
				return null;
			}
			return _modelController.ActiveAnimator;
		}
	}

	private void EnsureRefs()
	{
		if (_playerController == null)
		{
			_playerController = GetComponent<PlayerController>();
		}
		if (_characterController == null)
		{
			_characterController = GetComponent<CharacterController>();
		}
		if (_networkedCam == null)
		{
			_networkedCam = GetComponent<NetworkedCameraController>();
		}
		if (_modelController == null)
		{
			_modelController = GetComponent<PlayerModelController>();
		}
	}

	[TargetRpc]
	public void TargetActivateCinematic(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void LobbyIntroController::TargetActivateCinematic(Mirror.NetworkConnection)", 1982911829, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private IEnumerator ActivateCinematicRoutine()
	{
		EnsureRefs();
		if (ScreenFader.Instance != null)
		{
			yield return ScreenFader.Instance.FadeTo(1f, cameraFadeDuration);
		}
		_playerController.enabled = false;
		_characterController.enabled = false;
		_networkedCam.enabled = false;
		if (_networkedCam.playerCamera != null)
		{
			_networkedCam.playerCamera.enabled = false;
			_playerListener = _networkedCam.playerCamera.GetComponent<AudioListener>();
			if (_playerListener != null)
			{
				_playerListener.enabled = false;
			}
		}
		BarnCinematicCamera.Instance?.Activate();
		if (ScreenFader.Instance != null)
		{
			yield return ScreenFader.Instance.FadeTo(0f, cameraFadeDuration);
		}
	}

	[TargetRpc]
	public void TargetBeginWalk(NetworkConnection target, Vector3 exitPos, Quaternion exitRot, float duration)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(exitPos);
		writer.WriteQuaternion(exitRot);
		writer.WriteFloat(duration);
		SendTargetRPCInternal(target, "System.Void LobbyIntroController::TargetBeginWalk(Mirror.NetworkConnection,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single)", 1463503612, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private IEnumerator WalkOutRoutine(Vector3 exitPos, Quaternion exitRot, float duration)
	{
		EnsureRefs();
		Animator anim = _anim;
		Vector3 startPos = base.transform.position;
		Quaternion startRot = base.transform.rotation;
		float t = 0f;
		float speedF = 0f;
		while (t < duration)
		{
			t += Time.deltaTime;
			float t2 = Mathf.Clamp01(t / duration);
			base.transform.SetPositionAndRotation(Vector3.Lerp(startPos, exitPos, t2), Quaternion.Slerp(startRot, exitRot, t2));
			speedF = Mathf.MoveTowards(speedF, 1f, Time.deltaTime / Mathf.Max(animRampDuration, 0.001f));
			if (anim != null)
			{
				anim.SetFloat(_hashSpeedF, speedF);
			}
			yield return null;
		}
		base.transform.SetPositionAndRotation(exitPos, exitRot);
		float settleT = 0f;
		while (settleT < animRampDuration)
		{
			settleT += Time.deltaTime;
			speedF = Mathf.MoveTowards(speedF, 0f, Time.deltaTime / Mathf.Max(animRampDuration, 0.001f));
			if (anim != null)
			{
				anim.SetFloat(_hashSpeedF, speedF);
			}
			yield return null;
		}
		if (anim != null)
		{
			anim.SetFloat(_hashSpeedF, 0f);
		}
		if (ScreenFader.Instance != null)
		{
			yield return ScreenFader.Instance.FadeTo(1f, cameraFadeDuration);
		}
		BarnCinematicCamera.Instance?.Deactivate();
		if (_networkedCam.playerCamera != null)
		{
			_networkedCam.playerCamera.enabled = true;
			if (_playerListener != null)
			{
				_playerListener.enabled = true;
			}
		}
		_networkedCam.enabled = true;
		_characterController.enabled = true;
		_playerController.enabled = true;
		if (ScreenFader.Instance != null)
		{
			yield return ScreenFader.Instance.FadeTo(0f, cameraFadeDuration);
		}
		CmdIntroComplete();
	}

	[Command]
	private void CmdIntroComplete()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void LobbyIntroController::CmdIntroComplete()", 1117807716, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	static LobbyIntroController()
	{
		_hashSpeedF = Animator.StringToHash("Speed_f");
		RemoteProcedureCalls.RegisterCommand(typeof(LobbyIntroController), "System.Void LobbyIntroController::CmdIntroComplete()", InvokeUserCode_CmdIntroComplete, requiresAuthority: true);
		RemoteProcedureCalls.RegisterRpc(typeof(LobbyIntroController), "System.Void LobbyIntroController::TargetActivateCinematic(Mirror.NetworkConnection)", InvokeUserCode_TargetActivateCinematic__NetworkConnection);
		RemoteProcedureCalls.RegisterRpc(typeof(LobbyIntroController), "System.Void LobbyIntroController::TargetBeginWalk(Mirror.NetworkConnection,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single)", InvokeUserCode_TargetBeginWalk__NetworkConnection__Vector3__Quaternion__Single);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_TargetActivateCinematic__NetworkConnection(NetworkConnection target)
	{
		StartCoroutine(ActivateCinematicRoutine());
	}

	protected static void InvokeUserCode_TargetActivateCinematic__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetActivateCinematic called on server.");
		}
		else
		{
			((LobbyIntroController)obj).UserCode_TargetActivateCinematic__NetworkConnection(null);
		}
	}

	protected void UserCode_TargetBeginWalk__NetworkConnection__Vector3__Quaternion__Single(NetworkConnection target, Vector3 exitPos, Quaternion exitRot, float duration)
	{
		StartCoroutine(WalkOutRoutine(exitPos, exitRot, duration));
	}

	protected static void InvokeUserCode_TargetBeginWalk__NetworkConnection__Vector3__Quaternion__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetBeginWalk called on server.");
		}
		else
		{
			((LobbyIntroController)obj).UserCode_TargetBeginWalk__NetworkConnection__Vector3__Quaternion__Single(null, reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat());
		}
	}

	protected void UserCode_CmdIntroComplete()
	{
		BarnIntroController.Instance?.ServerCompleteWalker(base.connectionToClient);
	}

	protected static void InvokeUserCode_CmdIntroComplete(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdIntroComplete called on client.");
		}
		else
		{
			((LobbyIntroController)obj).UserCode_CmdIntroComplete();
		}
	}
}

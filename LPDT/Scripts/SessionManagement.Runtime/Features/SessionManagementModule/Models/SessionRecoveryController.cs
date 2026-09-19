using Features.DisconnectHandlerModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;

namespace Features.SessionManagementModule.Models
{
	public sealed class SessionRecoveryController : ISessionRecoveryController
	{
		private const float AVATAR_MISSING_DEBOUNCE_SECONDS = 2f;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		private float _avatarMissingSinceRealtime = -1f;

		private bool _recoveryRequested;

		public SessionRecoveryController(DisconnectRequestEventClass disconnectRequestEventClass)
		{
			_disconnectRequestEventClass = disconnectRequestEventClass;
		}

		public void EvaluateAvatarPresence(bool isAvatarHeld)
		{
			if (_recoveryRequested)
			{
				return;
			}
			if (isAvatarHeld)
			{
				_avatarMissingSinceRealtime = -1f;
				if (PlayerSessionPrefs.IsRecoveryInFlight())
				{
					Debug.LogError("[SessionRecovery] Avatar present again after recovery — recovery SUCCEEDED, clearing record.");
					PlayerSessionPrefs.SetRecoveryResult(SessionRecoveryResult.Recovered);
					PlayerSessionPrefs.ClearRecovery();
				}
			}
			else if (_avatarMissingSinceRealtime < 0f)
			{
				_avatarMissingSinceRealtime = Time.realtimeSinceStartup;
			}
			else if (!(Time.realtimeSinceStartup - _avatarMissingSinceRealtime < 2f))
			{
				throw new SessionRecoveryRequestedException(RecoveryReason.AvatarMissing);
			}
		}

		public void RequestRecovery(RecoveryReason reason)
		{
			if (!_recoveryRequested)
			{
				_recoveryRequested = true;
				Debug.LogError($"[SessionRecovery] CRITICAL fault ({reason}) — recording recovery request and disconnecting to menu for auto-reconnect.");
				PlayerSessionPrefs.RecordRecoveryRequest((int)reason);
				_disconnectRequestEventClass.Publish(DisconnectRequestReason.Recovery);
			}
		}
	}
}

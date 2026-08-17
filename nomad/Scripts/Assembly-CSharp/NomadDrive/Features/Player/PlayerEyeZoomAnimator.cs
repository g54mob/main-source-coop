using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Player.Core;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	public class PlayerEyeZoomAnimator : NetworkBehaviour, IPlayerComponent
	{
		private sealed class EyeTarget
		{
			public Transform Transform;

			public Vector3 BaseScale;

			public Tween Tween;
		}

		[Header("Eyeballs")]
		[SerializeField]
		private Transform leftEye;

		[SerializeField]
		private Transform rightEye;

		[Header("Eye Lids")]
		[SerializeField]
		private Transform leftEyeLidTop;

		[SerializeField]
		private Transform leftEyeLidBot;

		[SerializeField]
		private Transform rightEyeLidTop;

		[SerializeField]
		private Transform rightEyeLidBot;

		[Header("Zoom Animation")]
		[SerializeField]
		private float zoomScaleMultiplier = 1.5f;

		[SerializeField]
		private float animationDuration = 0.15f;

		[SerializeField]
		private Ease ease = Ease.OutQuad;

		[SyncVar(hook = "OnEyesZoomedChanged")]
		private bool _isEyesZoomed;

		private bool _isLocalPlayer;

		private bool _isLateJoinCompleted;

		private readonly List<EyeTarget> _targets = new List<EyeTarget>();

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isEyesZoomed;

		public int SetupPriority => 25;

		public bool Network_isEyesZoomed
		{
			get
			{
				return _isEyesZoomed;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isEyesZoomed, 1uL, _Mirror_SyncVarHookDelegate__isEyesZoomed);
			}
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isLocalPlayer = isLocalPlayer;
			base.enabled = isLocalPlayer;
		}

		private void Awake()
		{
			RegisterTarget(leftEye);
			RegisterTarget(rightEye);
			RegisterTarget(leftEyeLidTop);
			RegisterTarget(leftEyeLidBot);
			RegisterTarget(rightEyeLidTop);
			RegisterTarget(rightEyeLidBot);
		}

		private void RegisterTarget(Transform target)
		{
			if (!(target == null))
			{
				_targets.Add(new EyeTarget
				{
					Transform = target,
					BaseScale = target.localScale
				});
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			_isLateJoinCompleted = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				_isLateJoinCompleted = true;
				if (_isEyesZoomed)
				{
					ApplyScale(zoomed: true, instant: true);
				}
			}
		}

		public void SetEyesZoomed(bool zoomed)
		{
			if (_isLocalPlayer)
			{
				CmdSetEyesZoomed(zoomed);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetEyesZoomed(bool zoomed)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(zoomed);
			SendCommandInternal("System.Void NomadDrive.Features.Player.PlayerEyeZoomAnimator::CmdSetEyesZoomed(System.Boolean)", 1895162933, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnEyesZoomedChanged(bool oldValue, bool newValue)
		{
			if (_isLateJoinCompleted)
			{
				ApplyScale(newValue, instant: false);
			}
		}

		private void ApplyScale(bool zoomed, bool instant)
		{
			if (_targets.Count == 0)
			{
				return;
			}
			foreach (EyeTarget target in _targets)
			{
				Vector3 vector = (zoomed ? (target.BaseScale * zoomScaleMultiplier) : target.BaseScale);
				if (target.Tween.isAlive)
				{
					target.Tween.Stop();
				}
				if (instant)
				{
					target.Transform.localScale = vector;
				}
				else
				{
					target.Tween = Tween.Scale(target.Transform, vector, animationDuration, ease);
				}
			}
		}

		public PlayerEyeZoomAnimator()
		{
			_Mirror_SyncVarHookDelegate__isEyesZoomed = OnEyesZoomedChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetEyesZoomed__Boolean(bool zoomed)
		{
			Network_isEyesZoomed = zoomed;
		}

		protected static void InvokeUserCode_CmdSetEyesZoomed__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetEyesZoomed called on client.");
			}
			else
			{
				((PlayerEyeZoomAnimator)obj).UserCode_CmdSetEyesZoomed__Boolean(reader.ReadBool());
			}
		}

		static PlayerEyeZoomAnimator()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerEyeZoomAnimator), "System.Void NomadDrive.Features.Player.PlayerEyeZoomAnimator::CmdSetEyesZoomed(System.Boolean)", InvokeUserCode_CmdSetEyesZoomed__Boolean, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isEyesZoomed);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(_isEyesZoomed);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isEyesZoomed, _Mirror_SyncVarHookDelegate__isEyesZoomed, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isEyesZoomed, _Mirror_SyncVarHookDelegate__isEyesZoomed, reader.ReadBool());
			}
		}
	}
}

using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.Digging
{
	public class MetalDetector : HeldItem
	{
		[Header("Metal Detector Configuration")]
		[SerializeField]
		private MetalDetectorConfig config;

		[Tooltip("Coil/head transform that distance is measured from (the player sweeps it). Falls back to the model root if unassigned.")]
		[SerializeField]
		private Transform scanPoint;

		[SyncVar(hook = "OnSignalActiveChanged")]
		private bool _isSignalActiveNet;

		private AudioHandle _signalHandle;

		private bool _signalActive;

		private float _beepTimer;

		private float _scanTimer;

		private BuriedTreasure _nearest;

		private bool _placementActive;

		private bool _warnedNoConfig;

		private bool _warnedNoScanPoint;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isSignalActiveNet;

		private Transform ScanOrigin
		{
			get
			{
				if (scanPoint != null)
				{
					return scanPoint;
				}
				if (!(base.ModelTransform != null))
				{
					return base.transform;
				}
				return base.ModelTransform;
			}
		}

		public bool Network_isSignalActiveNet
		{
			get
			{
				return _isSignalActiveNet;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isSignalActiveNet, 512uL, _Mirror_SyncVarHookDelegate__isSignalActiveNet);
			}
		}

		public override void OnUnequip()
		{
			StopAll();
			_nearest = null;
			base.OnUnequip();
		}

		public override void OnPlacementModeEnter()
		{
			base.OnPlacementModeEnter();
			_placementActive = true;
			StopAll();
		}

		public override void OnPlacementModeExit()
		{
			base.OnPlacementModeExit();
			_placementActive = false;
		}

		protected override void OnDisable()
		{
			StopAll();
			base.OnDisable();
		}

		private void Update()
		{
			if (!base.isOwned)
			{
				return;
			}
			if (!base.IsEquipped || _placementActive)
			{
				StopAll();
				return;
			}
			if (config == null)
			{
				if (!_warnedNoConfig)
				{
					EvilLogger.LogError("[MetalDetector] MetalDetectorConfig is not assigned.", "Update", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Digging\\Scripts\\MetalDetector.cs", 96);
					_warnedNoConfig = true;
				}
				return;
			}
			if (scanPoint == null && !_warnedNoScanPoint)
			{
				_warnedNoScanPoint = true;
			}
			Vector3 position = ScanOrigin.position;
			_scanTimer += Time.deltaTime;
			if (_scanTimer >= config.scanQueryInterval)
			{
				_scanTimer = 0f;
				_nearest = BuriedTreasure.FindNearestUndug(position, config.scanRadius);
			}
			if (_nearest == null || _nearest.Stage != 0)
			{
				StopAll();
				return;
			}
			float num = PlanarDistance(position, _nearest.transform.position);
			if (num > config.scanRadius)
			{
				StopAll();
				return;
			}
			if (num <= config.signalRadius)
			{
				_beepTimer = 0f;
				if (_signalActive && !_signalHandle.IsValid)
				{
					_signalActive = false;
				}
				StartSignal();
				return;
			}
			StopSignal();
			float num2 = Mathf.InverseLerp(config.signalRadius, config.scanRadius, num);
			if (config.beepRateCurve != null)
			{
				num2 = config.beepRateCurve.Evaluate(num2);
			}
			float num3 = Mathf.Lerp(config.minBeepInterval, config.maxBeepInterval, num2);
			_beepTimer += Time.deltaTime;
			if (_beepTimer >= num3)
			{
				_beepTimer = 0f;
				if (config.beepSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShotAttachedExcludeSelf(config.beepSound, base.netIdentity);
				}
			}
		}

		private void StartSignal()
		{
			if (!_signalActive && !(config == null) && config.signalSound.IsValid() && AudioManager != null)
			{
				_signalHandle = AudioManager.PlayEventAttached(config.signalSound, base.gameObject);
				_signalActive = true;
				if (base.isOwned)
				{
					CmdSetSignalActive(active: true);
				}
			}
		}

		private void StopSignal()
		{
			if (_signalActive)
			{
				AudioManager?.StopEvent(_signalHandle, AudioStopMode.AllowFadeout, (config != null) ? config.signalFadeout : 0.15f);
				_signalActive = false;
				_signalHandle = AudioHandle.Invalid;
				if (base.isOwned)
				{
					CmdSetSignalActive(active: false);
				}
			}
		}

		private void StopAll()
		{
			StopSignal();
			_beepTimer = 0f;
		}

		[Command]
		private void CmdSetSignalActive(bool active)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(active);
			SendCommandInternal("System.Void NomadDrive.Features.Digging.MetalDetector::CmdSetSignalActive(System.Boolean)", 612093568, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void OnSignalActiveChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted && !base.isOwned)
			{
				if (newValue)
				{
					StartSignal();
				}
				else
				{
					StopSignal();
				}
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (!base.isOwned && _isSignalActiveNet)
			{
				StartSignal();
			}
		}

		private static float PlanarDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		public MetalDetector()
		{
			_Mirror_SyncVarHookDelegate__isSignalActiveNet = OnSignalActiveChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetSignalActive__Boolean(bool active)
		{
			Network_isSignalActiveNet = active;
		}

		protected static void InvokeUserCode_CmdSetSignalActive__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSignalActive called on client.");
			}
			else
			{
				((MetalDetector)obj).UserCode_CmdSetSignalActive__Boolean(reader.ReadBool());
			}
		}

		static MetalDetector()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(MetalDetector), "System.Void NomadDrive.Features.Digging.MetalDetector::CmdSetSignalActive(System.Boolean)", InvokeUserCode_CmdSetSignalActive__Boolean, requiresAuthority: true);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isSignalActiveNet);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteBool(_isSignalActiveNet);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isSignalActiveNet, _Mirror_SyncVarHookDelegate__isSignalActiveNet, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isSignalActiveNet, _Mirror_SyncVarHookDelegate__isSignalActiveNet, reader.ReadBool());
			}
		}
	}
}

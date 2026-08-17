using System;
using System.Runtime.InteropServices;
using EvilCore.EvilSave;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.SaveSystem;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Attachables
{
	public class ConditionComponent : NetworkBehaviour, INetworkSaveable
	{
		public bool IsLateJoinCompleted;

		[Header("Condition")]
		[SerializeField]
		private float _maxCondition = 100f;

		[SerializeField]
		private float _initialCondition = 100f;

		[Tooltip("When spawned through the loot pipeline, randomize the starting condition within the range below (seed-based). Enable for tires.")]
		[SerializeField]
		private bool randomizeInitialCondition;

		[SerializeField]
		private Vector2 initialConditionRange = new Vector2(50f, 100f);

		[SyncVar(hook = "OnConditionValueChanged")]
		[SerializeField]
		private float _condition;

		private Interactable _interactable;

		public readonly UnityEvent<float, float> OnConditionChanged = new UnityEvent<float, float>();

		public Action<float, float> _Mirror_SyncVarHookDelegate__condition;

		public float Condition => _condition;

		public float MaxCondition => _maxCondition;

		public float ConditionRatio
		{
			get
			{
				if (!(_maxCondition > 0f))
				{
					return 0f;
				}
				return _condition / _maxCondition;
			}
		}

		public bool IsBroken => _condition <= 0f;

		public string DisplayName
		{
			get
			{
				if (!(_interactable != null))
				{
					return base.name;
				}
				return _interactable.interactableName;
			}
		}

		public string ContributorKey => "condition";

		public float Network_condition
		{
			get
			{
				return _condition;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _condition, 1uL, _Mirror_SyncVarHookDelegate__condition);
			}
		}

		private void Awake()
		{
			_interactable = GetComponent<Interactable>();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			Network_condition = Mathf.Clamp(_initialCondition, 0f, _maxCondition);
			IsLateJoinCompleted = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			IsLateJoinCompleted = true;
			OnConditionChanged.Invoke(_condition, _condition);
		}

		private void OnConditionValueChanged(float oldValue, float newValue)
		{
			if (IsLateJoinCompleted)
			{
				OnConditionChanged.Invoke(oldValue, newValue);
			}
		}

		[Server]
		public void ServerSetCondition(float value)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Attachables.ConditionComponent::ServerSetCondition(System.Single)' called when server was not active");
			}
			else
			{
				Network_condition = Mathf.Clamp(value, 0f, _maxCondition);
			}
		}

		[Server]
		public void ServerInitializeCondition(int seed)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Attachables.ConditionComponent::ServerInitializeCondition(System.Int32)' called when server was not active");
				return;
			}
			if (!randomizeInitialCondition)
			{
				ServerSetCondition(_initialCondition);
				return;
			}
			System.Random random = new System.Random(seed ^ 0x5EED);
			float a = Mathf.Min(initialConditionRange.x, initialConditionRange.y);
			float b = Mathf.Max(initialConditionRange.x, initialConditionRange.y);
			ServerSetCondition(Mathf.Lerp(a, b, (float)random.NextDouble()));
		}

		[Server]
		public void ServerAddCondition(float amount)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Attachables.ConditionComponent::ServerAddCondition(System.Single)' called when server was not active");
			}
			else
			{
				ServerSetCondition(_condition + amount);
			}
		}

		public void SetCondition(float value)
		{
			if (base.isServer)
			{
				ServerSetCondition(value);
			}
			else
			{
				CmdSetCondition(value);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetCondition(float value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(value);
			SendCommandInternal("System.Void NomadDrive.Features.Attachables.ConditionComponent::CmdSetCondition(System.Single)", 265850123, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_condition);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			float value = reader.ReadFloat();
			if (NetworkServer.active)
			{
				ServerSetCondition(value);
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public ConditionComponent()
		{
			_Mirror_SyncVarHookDelegate__condition = OnConditionValueChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetCondition__Single(float value)
		{
			ServerSetCondition(value);
		}

		protected static void InvokeUserCode_CmdSetCondition__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetCondition called on client.");
			}
			else
			{
				((ConditionComponent)obj).UserCode_CmdSetCondition__Single(reader.ReadFloat());
			}
		}

		static ConditionComponent()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(ConditionComponent), "System.Void NomadDrive.Features.Attachables.ConditionComponent::CmdSetCondition(System.Single)", InvokeUserCode_CmdSetCondition__Single, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteFloat(_condition);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteFloat(_condition);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _condition, _Mirror_SyncVarHookDelegate__condition, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _condition, _Mirror_SyncVarHookDelegate__condition, reader.ReadFloat());
			}
		}
	}
}

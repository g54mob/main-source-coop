using System;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.TeleportModule.Scripts.TeleportCommon;
using Fusion;
using UnityEngine;

namespace Features.KnifeThrowingModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class KnifeThrowingTableObjectTeleport : NetworkBehaviour, ITeleportable
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private ThrowTableItemType _throwTableItemType;

		[WeaverGenerated]
		[DefaultForProperty("NetSpawnPosition", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetSpawnPosition;

		[WeaverGenerated]
		[DefaultForProperty("NetHasSpawnPose", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _NetHasSpawnPose;

		private MonoItem _monoItem;

		[Networked]
		[NetworkedWeaved(0, 3)]
		private unsafe Vector3 NetSpawnPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KnifeThrowingTableObjectTeleport.NetSpawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KnifeThrowingTableObjectTeleport.NetSpawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe bool NetHasSpawnPose
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KnifeThrowingTableObjectTeleport.NetHasSpawnPose. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KnifeThrowingTableObjectTeleport.NetHasSpawnPose. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public bool CanTeleport => _simplePointGrabable.GrabbedByPlayers.Count <= 0;

		public bool HasSpawnPose => NetHasSpawnPose;

		public Vector3 SpawnPosition => NetSpawnPosition;

		public ThrowTableItemType ThrowTableItemType => _throwTableItemType;

		public event Action<KnifeThrowingTableObjectTeleport> OnRemovedFromTable;

		public void SetSpawnPose(Vector3 position)
		{
			if (base.HasStateAuthority)
			{
				NetSpawnPosition = position;
				NetHasSpawnPose = true;
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			if (TryGetComponent<MonoItem>(out _monoItem))
			{
				_monoItem.OnDespawn += HandleItemDespawned;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_monoItem != null)
			{
				_monoItem.OnDespawn -= HandleItemDespawned;
			}
			base.Despawned(runner, hasState);
		}

		private void HandleItemDespawned(IItem _)
		{
			this.OnRemovedFromTable?.Invoke(this);
		}

		public void Teleport(Vector3 position)
		{
			Vector3 vector = (NetHasSpawnPose ? NetSpawnPosition : position);
			Debug.Log($"[KnifeSpawn] Reset teleport '{_throwTableItemType}' -> {vector} (hasSpawnPose={NetHasSpawnPose}, storedSpawn={NetSpawnPosition}, requested={position}).");
			Quaternion identity = Quaternion.identity;
			_rigidbody.position = vector;
			_rigidbody.rotation = identity;
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			base.transform.SetPositionAndRotation(vector, identity);
			_rigidbody.isKinematic = false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetSpawnPosition = _NetSpawnPosition;
			NetHasSpawnPose = _NetHasSpawnPose;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_NetSpawnPosition = NetSpawnPosition;
			_NetHasSpawnPose = NetHasSpawnPose;
		}
	}
}

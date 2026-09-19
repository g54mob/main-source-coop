using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class EnemySpawnPointRegistrar : NetworkBehaviour, IEnemySpawnPointOccupancy
	{
		private const float GIZMO_AUTHORED_RADIUS = 0.08f;

		private const float GIZMO_RESOLVED_RADIUS = 0.12f;

		[SerializeField]
		private EnemyType _enemyType;

		[SerializeField]
		private Transform _areaPoint;

		[SerializeField]
		private bool _randomizePosition;

		[SerializeField]
		private Vector3 _offsetMin;

		[SerializeField]
		private Vector3 _offsetMax;

		private EnemySpawnPointsModel _enemySpawnPointsModel;

		private EnemySpawnPointData _enemySpawnPointData;

		private Vector3 _authoredWorldPosition;

		private Quaternion _authoredWorldRotation;

		private bool _hasAuthoredPose;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("OccupantNetworkId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _OccupantNetworkId;

		[WeaverGenerated]
		[DefaultForProperty("ResolvedPosition", 1, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _ResolvedPosition;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe NetworkId OccupantNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnPointRegistrar.OccupantNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnPointRegistrar.OccupantNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnResolvedPositionChanged")]
		[NetworkedWeaved(1, 3)]
		private unsafe Vector3 ResolvedPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnPointRegistrar.ResolvedPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnPointRegistrar.ResolvedPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 1) = value;
			}
		}

		public bool IsOccupied => OccupantNetworkId != default(NetworkId);

		public NetworkId OccupantId => OccupantNetworkId;

		[Inject]
		public void InjectDependencies(EnemySpawnPointsModel enemySpawnPointsModel)
		{
			_enemySpawnPointsModel = enemySpawnPointsModel;
		}

		private void Awake()
		{
			CacheAuthoredPose();
		}

		public override void Spawned()
		{
			base.Spawned();
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			if (!_hasAuthoredPose)
			{
				CacheAuthoredPose();
			}
			if (_randomizePosition)
			{
				if (base.HasStateAuthority && ResolvedPosition == default(Vector3))
				{
					ResolvedPosition = RollResolvedWorldPosition();
				}
				if (ResolvedPosition != default(Vector3))
				{
					ApplyResolvedPosition();
				}
			}
			RegisterOrUpdateSpawnPointData();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (base.gameObject.activeSelf && _enemySpawnPointData != null)
			{
				_enemySpawnPointsModel.UnRegisterSpawnPoint(_enemyType, _enemySpawnPointData);
				_enemySpawnPointData = null;
			}
		}

		public bool TryOccupy(NetworkObject occupant)
		{
			if (occupant == null || !occupant.IsValid)
			{
				return false;
			}
			if (!base.HasStateAuthority)
			{
				return false;
			}
			NetworkId id = occupant.Id;
			if (OccupantNetworkId != default(NetworkId) && OccupantNetworkId != id)
			{
				return false;
			}
			if (OccupantNetworkId == id)
			{
				return true;
			}
			OccupantNetworkId = id;
			return true;
		}

		public void Release(NetworkObject occupant)
		{
			if (!(occupant == null) && occupant.IsValid)
			{
				Release(occupant.Id);
			}
		}

		public void Release(NetworkId occupantId)
		{
			if (base.HasStateAuthority && !(OccupantNetworkId == default(NetworkId)) && !(OccupantNetworkId != occupantId))
			{
				OccupantNetworkId = default(NetworkId);
			}
		}

		private void OnResolvedPositionChanged()
		{
			if (_randomizePosition && !(ResolvedPosition == default(Vector3)))
			{
				ApplyResolvedPosition();
				RegisterOrUpdateSpawnPointData();
			}
		}

		private void CacheAuthoredPose()
		{
			_authoredWorldPosition = base.transform.position;
			_authoredWorldRotation = base.transform.rotation;
			_hasAuthoredPose = true;
		}

		private Vector3 RollResolvedWorldPosition()
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(_offsetMin.x, _offsetMax.x), UnityEngine.Random.Range(_offsetMin.y, _offsetMax.y), UnityEngine.Random.Range(_offsetMin.z, _offsetMax.z));
			return _authoredWorldPosition + _authoredWorldRotation * vector;
		}

		private void ApplyResolvedPosition()
		{
			base.transform.position = ResolvedPosition;
		}

		private void RegisterOrUpdateSpawnPointData()
		{
			Vector3 position = base.transform.position;
			Vector3 areaPosition = ((_areaPoint == null) ? Vector3.zero : _areaPoint.position);
			if (_enemySpawnPointData == null)
			{
				_enemySpawnPointData = new EnemySpawnPointData(position, base.transform.rotation, areaPosition, isOneTimeSpawnPoint: false, this);
				_enemySpawnPointsModel.RegisterSpawnPoint(_enemyType, _enemySpawnPointData);
			}
			else
			{
				_enemySpawnPointData.UpdatePose(position, areaPosition);
			}
		}

		private void OnDrawGizmos()
		{
			if (_randomizePosition)
			{
				DrawRandomizationGizmos(selected: false);
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (_randomizePosition)
			{
				DrawRandomizationGizmos(selected: true);
			}
		}

		private void DrawRandomizationGizmos(bool selected)
		{
			Vector3 vector = ((Application.isPlaying && _hasAuthoredPose) ? _authoredWorldPosition : base.transform.position);
			Quaternion authoredRotation = ((Application.isPlaying && _hasAuthoredPose) ? _authoredWorldRotation : base.transform.rotation);
			Color color = (selected ? new Color(0.2f, 0.85f, 1f, 0.95f) : new Color(0.2f, 0.85f, 1f, 0.55f));
			Color color2 = (selected ? new Color(1f, 0.85f, 0.2f, 0.95f) : new Color(1f, 0.85f, 0.2f, 0.55f));
			Color color3 = (selected ? new Color(0.3f, 1f, 0.35f, 0.95f) : new Color(0.3f, 1f, 0.35f, 0.55f));
			Gizmos.color = color;
			Gizmos.DrawSphere(vector, 0.08f);
			DrawAxisRange(vector, authoredRotation, Vector3.right, _offsetMin.x, _offsetMax.x, color2);
			DrawAxisRange(vector, authoredRotation, Vector3.up, _offsetMin.y, _offsetMax.y, color2);
			DrawAxisRange(vector, authoredRotation, Vector3.forward, _offsetMin.z, _offsetMax.z, color2);
			Vector3 resolvedPreviewPosition = GetResolvedPreviewPosition(vector, authoredRotation);
			Gizmos.color = color3;
			Gizmos.DrawSphere(resolvedPreviewPosition, 0.12f);
		}

		private Vector3 GetResolvedPreviewPosition(Vector3 authoredPosition, Quaternion authoredRotation)
		{
			if (Application.isPlaying && base.Object != null && base.Object.IsValid && ResolvedPosition != default(Vector3))
			{
				return ResolvedPosition;
			}
			Vector3 vector = (_offsetMin + _offsetMax) * 0.5f;
			return authoredPosition + authoredRotation * vector;
		}

		private static void DrawAxisRange(Vector3 authoredPosition, Quaternion authoredRotation, Vector3 localAxis, float min, float max, Color color)
		{
			if (!Mathf.Approximately(min, max))
			{
				Vector3 vector = authoredRotation * localAxis;
				Vector3 vector2 = authoredPosition + vector * min;
				Vector3 vector3 = authoredPosition + vector * max;
				Gizmos.color = color;
				Gizmos.DrawLine(vector2, vector3);
				Gizmos.DrawSphere(vector2, 0.048f);
				Gizmos.DrawSphere(vector3, 0.048f);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			OccupantNetworkId = _OccupantNetworkId;
			ResolvedPosition = _ResolvedPosition;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_OccupantNetworkId = OccupantNetworkId;
			_ResolvedPosition = ResolvedPosition;
		}
	}
}

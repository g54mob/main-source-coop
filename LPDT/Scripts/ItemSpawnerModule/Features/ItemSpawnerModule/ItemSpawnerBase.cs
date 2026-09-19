using System;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.ItemSpawnerModule
{
	public abstract class ItemSpawnerBase : MonoBehaviour, IItemSpawner
	{
		[Header("Spawn transform")]
		[SerializeField]
		private Transform _spawnPoint;

		[SerializeField]
		private bool _applyPosition = true;

		[SerializeField]
		private bool _applyRotation = true;

		[Header("Network")]
		[SerializeField]
		private bool _spawnOnlyOnHost = true;

		[SerializeField]
		private SimpleItemSpawnTrigger _spawnTrigger = SimpleItemSpawnTrigger.OnStart;

		[SerializeField]
		private bool _parentToSpawner;

		[SerializeField]
		private bool _resetTransformOnParent;

		[Header("Editor preview")]
		[SerializeField]
		private bool _showPreview = true;

		[SerializeField]
		private bool _useCustomPreviewMesh;

		[SerializeField]
		private Mesh _previewMesh;

		[SerializeField]
		private Vector3 _previewMeshLocalPosition;

		[SerializeField]
		private Quaternion _previewMeshLocalRotation = Quaternion.identity;

		[SerializeField]
		private Vector3 _previewMeshLocalScale = Vector3.one;

		[SerializeField]
		private Color _previewColor = new Color(0.35f, 0.75f, 1f, 0.45f);

		private bool _spawnRequested;

		protected MultiplayerModel MultiplayerModel { get; private set; }

		public NetworkObject SpawnedInstance { get; set; }

		protected bool HasValidSpawnedInstance
		{
			get
			{
				if (SpawnedInstance != null)
				{
					return SpawnedInstance.IsValid;
				}
				return false;
			}
		}

		public Transform SpawnPoint => _spawnPoint;

		public bool ShowPreview => _showPreview;

		public bool UseCustomPreviewMesh => _useCustomPreviewMesh;

		public Mesh PreviewMesh => _previewMesh;

		public Vector3 PreviewMeshLocalPosition => _previewMeshLocalPosition;

		public Quaternion PreviewMeshLocalRotation => _previewMeshLocalRotation;

		public Vector3 PreviewMeshLocalScale => _previewMeshLocalScale;

		public Color PreviewColor => _previewColor;

		public bool ApplyPosition => _applyPosition;

		public bool ApplyRotation => _applyRotation;

		protected bool ParentToSpawner => _parentToSpawner;

		public event Action<ItemSpawnerBase> OnItemSpawned;

		protected abstract bool CanSpawn();

		protected abstract GameObject GetPreviewSource();

		protected virtual UniTask SpawnItemAsync()
		{
			return UniTask.CompletedTask;
		}

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel)
		{
			MultiplayerModel = multiplayerModel;
		}

		private void Start()
		{
			if (_spawnTrigger == SimpleItemSpawnTrigger.OnStart)
			{
				ScheduleSpawn();
			}
		}

		public void Spawn()
		{
			ScheduleSpawn();
		}

		public async UniTask SpawnAsync()
		{
			if (!_spawnRequested && CanSpawn())
			{
				await RequestSpawnAsync();
			}
		}

		private void ScheduleSpawn()
		{
			if (!_spawnRequested && CanSpawn())
			{
				RequestSpawn();
			}
		}

		private void RequestSpawn()
		{
			if (!_spawnRequested)
			{
				_spawnRequested = true;
				ExecuteSpawnAsync().Forget();
			}
		}

		private async UniTask RequestSpawnAsync()
		{
			if (!_spawnRequested)
			{
				_spawnRequested = true;
				await ExecuteSpawnAsync();
			}
		}

		private async UniTask ExecuteSpawnAsync()
		{
			if (!CanSpawn())
			{
				return;
			}
			await UniTask.WaitUntil(() => MultiplayerModel?.NetworkRunner != null && MultiplayerModel.NetworkRunner.IsRunning);
			if (this == null)
			{
				return;
			}
			NetworkRunner networkRunner = MultiplayerModel.NetworkRunner;
			if ((!_spawnOnlyOnHost || networkRunner.IsSharedModeMasterClient) && !HasValidSpawnedInstance)
			{
				await SpawnItemAsync();
				if (!(this == null) && HasValidSpawnedInstance)
				{
					this.OnItemSpawned?.Invoke(this);
				}
			}
		}

		protected void GetSpawnPose(out Vector3 position, out Quaternion rotation)
		{
			Transform transform = ((_spawnPoint != null) ? _spawnPoint : base.transform);
			position = (_applyPosition ? transform.position : base.transform.position);
			rotation = (_applyRotation ? transform.rotation : base.transform.rotation);
		}

		protected void ApplyParentToSpawnerIfNeeded()
		{
			if (ParentToSpawner && SpawnedInstance != null)
			{
				SpawnedInstance.transform.SetParent(base.transform, worldPositionStays: true);
				if (_resetTransformOnParent)
				{
					SpawnedInstance.transform.localPosition = Vector3.zero;
					SpawnedInstance.transform.localRotation = Quaternion.identity;
				}
			}
		}
	}
}

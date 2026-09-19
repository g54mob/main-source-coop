using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AnimationModule.Scripts;
using Features.CustomSynchronizersModule.Scripts;
using Features.GrabModule.Scripts;
using Features.Movement.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class ArmEndController : NetworkBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private Transform _armEndTransform;

		[SerializeField]
		private StaticColorChangerByPlayerRef _colorChanger;

		[SerializeField]
		private NetworkedAnimationControllerBase _networkedAnimationController;

		[SerializeField]
		private Transform _visualsTransform;

		[SerializeField]
		private Vector3 _maxScale;

		[SerializeField]
		private float _scaleLerpSpeed = 1f;

		[SerializeField]
		private Vector3 _defaultScale;

		[SerializeField]
		private CopyRotationFromObject _copyRotationFromObject;

		[SerializeField]
		private PhysicsSynchronizer _physicsSynchronizer;

		[SerializeField]
		private bool _isAlwaysSynchronized = true;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ArmOrientation", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Arm _ArmOrientation;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsGrabbed", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsGrabbed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("LookPointNetworkId", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _LookPointNetworkId;

		private readonly HashSet<ArmEndSyncReason> _activeSyncReasons = new HashSet<ArmEndSyncReason>();

		private bool _isSyncEnabled = true;

		private ArmStartsModel _armStartsModel;

		private bool _isInitialized;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private IStat _maxWeightCapacityStat;

		private float _defaultWeightCapacity;

		private CancellationTokenSource _spawnCts;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe Arm ArmOrientation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmEndController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Arm*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmEndController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Arm*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe bool IsGrabbed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmEndController.IsGrabbed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmEndController.IsGrabbed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe NetworkId LookPointNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmEndController.LookPointNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmEndController.LookPointNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 2) = value;
			}
		}

		public bool IsGrabbedBySomeone => IsGabbed();

		public bool IsInitialized => _isInitialized;

		[Inject]
		public void InjectDependencies(ArmStartsModel armStartsModel, SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_armStartsModel = armStartsModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		private bool IsGabbed()
		{
			if (_simplePointGrabable == null)
			{
				return false;
			}
			return _simplePointGrabable.GrabbedByPlayers.Count > 0;
		}

		public override async void Spawned()
		{
			base.Spawned();
			_isInitialized = true;
			_spawnCts?.Cancel();
			_spawnCts?.Dispose();
			_spawnCts = new CancellationTokenSource();
			CancellationToken token = _spawnCts.Token;
			if (ArmOrientation == Arm.None)
			{
				try
				{
					await UniTask.WaitUntil(() => base.Object != null && base.Object.IsValid && ArmOrientation != Arm.None, PlayerLoopTiming.Update, token);
				}
				catch (OperationCanceledException)
				{
					return;
				}
			}
			if (!(base.Object == null) && base.Object.IsValid)
			{
				_armStartsModel.AddArmEnd(base.Object.StateAuthority, new ArmEndEntity(_armEndTransform, _colorChanger), ArmOrientation);
				ApplySyncState();
				if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(base.Object.InputAuthority.PlayerId))
				{
					_maxWeightCapacityStat = _spawnedEntityStatsModel.PlayerStats[base.Object.InputAuthority.PlayerId].GetStat(EntityStatType.MaxWeightCapacity);
					_defaultWeightCapacity = _maxWeightCapacityStat.FullValue;
				}
				else
				{
					_spawnedEntityStatsModel.OnPlayerStatRegistered += RegisterStat;
				}
			}
		}

		private void RegisterStat(int playerId)
		{
			if (base.Object.InputAuthority.PlayerId == playerId)
			{
				_maxWeightCapacityStat = _spawnedEntityStatsModel.PlayerStats[base.Object.InputAuthority.PlayerId].GetStat(EntityStatType.MaxWeightCapacity);
				_defaultWeightCapacity = _maxWeightCapacityStat.FullValue;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isInitialized = false;
			_spawnCts?.Cancel();
			_spawnCts?.Dispose();
			_spawnCts = null;
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= RegisterStat;
		}

		private void Update()
		{
			if (_isInitialized)
			{
				EnsureLookPointResolved();
				if (!(_networkedAnimationController == null))
				{
					UpdateScale();
					_networkedAnimationController.SetBool("IsGrabbed", IsGrabbed);
				}
			}
		}

		private void EnsureLookPointResolved()
		{
			if (!(_copyRotationFromObject.ObjectToCopy != null) && !(LookPointNetworkId == default(NetworkId)))
			{
				NetworkObject networkObject = base.Runner.FindObject(LookPointNetworkId);
				if (networkObject != null)
				{
					_copyRotationFromObject.ObjectToCopy = networkObject.transform;
				}
			}
		}

		private void UpdateScale()
		{
			if (_maxWeightCapacityStat != null && !(_visualsTransform == null))
			{
				float value = _maxWeightCapacityStat.Value;
				if (!(value <= Mathf.Epsilon))
				{
					float num = _maxWeightCapacityStat.FullValue / value;
					num = Mathf.Clamp01(num - 1f);
					Vector3 b = Vector3.Lerp(_defaultScale, _maxScale, num);
					_visualsTransform.localScale = Vector3.Lerp(_visualsTransform.localScale, b, Time.deltaTime * _scaleLerpSpeed);
				}
			}
		}

		public void SetArmOrientation(Arm armOrientation)
		{
			ArmOrientation = armOrientation;
		}

		public void SetLookPoint(NetworkObject lookPoint)
		{
			LookPointNetworkId = lookPoint.Id;
		}

		public void AddSynchronizationReason(ArmEndSyncReason reason)
		{
			if (_activeSyncReasons.Add(reason))
			{
				ApplySyncState();
			}
		}

		public void RemoveSynchronizationReason(ArmEndSyncReason reason)
		{
			if (_activeSyncReasons.Remove(reason))
			{
				ApplySyncState();
			}
		}

		private void ApplySyncState()
		{
			if (_physicsSynchronizer == null)
			{
				return;
			}
			bool flag = _isAlwaysSynchronized || _activeSyncReasons.Count > 0;
			if (flag != _isSyncEnabled)
			{
				_isSyncEnabled = flag;
				if (flag)
				{
					_physicsSynchronizer.EnableSynchronization();
				}
				else
				{
					_physicsSynchronizer.DisableSynchronization();
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ArmOrientation = _ArmOrientation;
			IsGrabbed = _IsGrabbed;
			LookPointNetworkId = _LookPointNetworkId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ArmOrientation = ArmOrientation;
			_IsGrabbed = IsGrabbed;
			_LookPointNetworkId = LookPointNetworkId;
		}
	}
}

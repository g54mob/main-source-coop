using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Systems;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy
{
	[NetworkBehaviourWeaved(8)]
	public class BartenderEnemyContext : NetworkBehaviour, IEnemyContext, IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
	{
		private const float SmoothedVelocityQuantizeFactor = 10f;

		private const float MoveSpeedChangeThreshold = 0.01f;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SmoothedVelocityQuantized", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _SmoothedVelocityQuantized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("MoveSpeed", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _MoveSpeed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsCrouching", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsCrouching;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsAggressive", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsAggressive;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsDancing", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsDancing;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ActiveDanceIndex", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ActiveDanceIndex;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsReacting", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsReacting;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("AppearanceSeed", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _AppearanceSeed;

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public BartenderAnimationSystem AnimationSystem { get; private set; }

		[field: SerializeField]
		public BartenderReplicateTargetSystem ReplicateTargetSystem { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe short SmoothedVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(short*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.SmoothedVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(short*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe float MoveSpeed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.MoveSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe bool IsCrouching
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe bool IsAggressive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsAggressive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsAggressive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe bool IsDancing
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsDancing. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 4);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsDancing. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe int ActiveDanceIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.ActiveDanceIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[5];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.ActiveDanceIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[5] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 1)]
		public unsafe bool IsReacting
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsReacting. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.IsReacting. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 6) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(7, 1)]
		public unsafe int AppearanceSeed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.AppearanceSeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[7];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEnemyContext.AppearanceSeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[7] = value;
			}
		}

		public int CurrentAreaType { get; private set; }

		public float SmoothedVelocityLerpSpeed { get; }

		public Vector3 TargetPosition { get; private set; }

		public bool TargetPositionCompleted { get; private set; }

		public bool NeedToFindTargetPosition { get; set; }

		public float CompletePointMinDistance { get; set; } = 1.5f;

		public float TargetSearchRange { get; set; } = 25f;

		public float AvailablePointRange { get; set; } = 0.5f;

		public float TimeToAttack { get; set; }

		public float DistanceToAttack { get; set; }

		public float AttackCooldown { get; set; }

		public bool IsAttackOnCooldown { get; set; }

		public float CurrentStateTime { get; set; }

		public bool NeedGenerateAppearance { get; set; }

		public Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; } = new Dictionary<PlayerDataHolder, float>();

		public Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; } = new Dictionary<PlayerDataHolder, float>();

		public List<PlayerDataHolder> DetectedPlayers { get; private set; } = new List<PlayerDataHolder>();

		public List<PlayerDataHolder> VisiblePlayers { get; private set; } = new List<PlayerDataHolder>();

		public PlayerDataHolder PriorityPlayer { get; private set; }

		public float SmoothedVelocity => (float)SmoothedVelocityQuantized / 10f;

		public event Action OnDetectedPlayersChanged;

		public event Action OnVisiblePlayersChanged;

		public event Action OnPriorityPlayerChanged;

		public void Initialize()
		{
			SetMoveSpeed(0f);
			SetIsAggressive(value: false);
			SetIsCrouching(value: false);
			SetIsDancing(value: false);
			SetActiveDanceIndex(-1);
			SetIsReacting(value: false);
			if (AppearanceSeed <= 0)
			{
				NeedGenerateAppearance = true;
			}
		}

		public void SetSmoothedVelocity(float value)
		{
			short num = (short)Mathf.Clamp(Mathf.RoundToInt(value * 10f), -32768, 32767);
			if (SmoothedVelocityQuantized != num)
			{
				SmoothedVelocityQuantized = num;
			}
		}

		public void SetMoveSpeed(float value)
		{
			if (!(Mathf.Abs(MoveSpeed - value) < 0.01f))
			{
				MoveSpeed = value;
			}
		}

		public void SetIsCrouching(bool value)
		{
			if (IsCrouching != value)
			{
				IsCrouching = value;
			}
		}

		public void SetIsAggressive(bool value)
		{
			if (IsAggressive != value)
			{
				IsAggressive = value;
			}
		}

		public void SetIsDancing(bool value)
		{
			if (IsDancing != value)
			{
				IsDancing = value;
			}
		}

		public void SetActiveDanceIndex(int value)
		{
			if (ActiveDanceIndex != value)
			{
				ActiveDanceIndex = value;
			}
		}

		public void SetIsReacting(bool value)
		{
			if (IsReacting != value)
			{
				IsReacting = value;
			}
		}

		public void SetAppearanceSeed(int value)
		{
			if (AppearanceSeed != value)
			{
				AppearanceSeed = value;
			}
		}

		public void SetTargetPosition(Vector3 position)
		{
			TargetPosition = position;
		}

		public void SetTargetPositionCompleted(bool isCompleted)
		{
			TargetPositionCompleted = isCompleted;
		}

		public void SetCurrentAreaType(int areaType)
		{
			CurrentAreaType = areaType;
		}

		public void SetDetectedPlayers(List<PlayerDataHolder> players)
		{
			DetectedPlayers = players;
			this.OnDetectedPlayersChanged?.Invoke();
		}

		public void SetVisiblePlayers(List<PlayerDataHolder> players)
		{
			VisiblePlayers = players;
			this.OnVisiblePlayersChanged?.Invoke();
		}

		public void SetPriorityPlayer(PlayerDataHolder priorityPlayer)
		{
			if (priorityPlayer != PriorityPlayer)
			{
				PriorityPlayer = priorityPlayer;
				this.OnPriorityPlayerChanged?.Invoke();
			}
		}

		public void SetDetectedPlayerDistance(PlayerDataHolder player, float distance)
		{
			DetectedPlayersDistance[player] = distance;
		}

		public void RemoveDetectedPlayerDistance(PlayerDataHolder player)
		{
			DetectedPlayersDistance.Remove(player);
		}

		public void ClearDetectedPlayersDistance()
		{
			DetectedPlayersDistance.Clear();
		}

		public void SetDetectedPlayerFirstSeenTime(PlayerDataHolder player, float time)
		{
			DetectedPlayerFirstDetectedTime[player] = time;
		}

		public void RemoveDetectedPlayerFirstSeenTime(PlayerDataHolder player)
		{
			DetectedPlayerFirstDetectedTime.Remove(player);
		}

		public void ClearDetectedPlayersFirstSeenTime()
		{
			DetectedPlayerFirstDetectedTime.Clear();
		}

		public float GetStatValue(EntityStatType statType, float fallbackValue = 0f)
		{
			if (StatEntity == null)
			{
				return fallbackValue;
			}
			return StatEntity.GetStat(statType)?.Value ?? fallbackValue;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SmoothedVelocityQuantized = _SmoothedVelocityQuantized;
			MoveSpeed = _MoveSpeed;
			IsCrouching = _IsCrouching;
			IsAggressive = _IsAggressive;
			IsDancing = _IsDancing;
			ActiveDanceIndex = _ActiveDanceIndex;
			IsReacting = _IsReacting;
			AppearanceSeed = _AppearanceSeed;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SmoothedVelocityQuantized = SmoothedVelocityQuantized;
			_MoveSpeed = MoveSpeed;
			_IsCrouching = IsCrouching;
			_IsAggressive = IsAggressive;
			_IsDancing = IsDancing;
			_ActiveDanceIndex = ActiveDanceIndex;
			_IsReacting = IsReacting;
			_AppearanceSeed = AppearanceSeed;
		}
	}
}

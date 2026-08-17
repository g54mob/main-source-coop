using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Timing;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using FishNet.Utility;
using FishNet.Utility.Extension;
using UnityEngine;

namespace FishNet.Component.Prediction
{
	[AddComponentMenu("FishNet/Component/PredictedObject")]
	public class PredictedObject : NetworkBehaviour
	{
		public enum SpectatorSmoothingType
		{
			Accuracy = 0,
			Mixed = 1,
			Gradual = 2,
			Custom = 3
		}

		private enum CollectionState : byte
		{
			Unset = 0,
			Added = 1,
			Removed = 2
		}

		internal enum PredictionType : byte
		{
			Other = 0,
			Rigidbody = 1,
			Rigidbody2D = 2
		}

		internal enum ResendType : byte
		{
			Disabled = 0,
			Interval = 1
		}

		[Serializable]
		public struct SmoothingData
		{
			[Tooltip("Percentage of ping to use as interpolation. Higher values will result in more interpolation.")]
			[Range(0.01f, 5f)]
			public float InterpolationPercent;

			[Tooltip("Percentage of ping to use as interpolation when colliding with an object local client owns.This is used to speed up local interpolation when predicted objects collide with a player as well keep graphics closer to the objects root while colliding.")]
			[Range(0.01f, 5f)]
			public float CollisionInterpolationPercent;

			[Tooltip("How much per tick to decrease to collision interpolation when colliding with a local player object. Higher values will set interpolation to collision settings faster.")]
			[Range(1f, 255f)]
			public byte InterpolationDecreaseStep;

			[Tooltip("How much per tick to increase to normal interpolation when not colliding with a local player object. Higher values will set interpolation to normal settings faster.")]
			[Range(1f, 255f)]
			public byte InterpolationIncreaseStep;
		}

		[Tooltip("True if this object implements replicate and reconcile methods.")]
		[SerializeField]
		private bool _implementsPredictionMethods = true;

		[Tooltip("Transform which holds the graphical features of this object. This transform will be smoothed when desynchronizations occur.")]
		[SerializeField]
		private Transform _graphicalObject;

		[Tooltip("True to enable teleport threshhold.")]
		[SerializeField]
		private bool _enableTeleport;

		[Tooltip("How far the transform must travel in a single update to cause a teleport rather than smoothing. Using 0f will teleport every update.")]
		[Range(0f, 200f)]
		[SerializeField]
		private float _teleportThreshold = 1f;

		[Tooltip("True to smooth position on owner objects.")]
		[SerializeField]
		private bool _ownerSmoothPosition = true;

		[Tooltip("True to smooth rotation on owner objects.")]
		[SerializeField]
		private bool _ownerSmoothRotation = true;

		[Tooltip("How far in the past to keep the graphical object when owner. Using a value of 0 will disable interpolation.")]
		[Range(0f, 255f)]
		[SerializeField]
		private byte _ownerInterpolation = 1;

		[Tooltip("Type of prediction movement which is being used.")]
		[SerializeField]
		private PredictionType _predictionType;

		[Tooltip("Rigidbody to predict.")]
		[SerializeField]
		private Rigidbody _rigidbody;

		[Tooltip("Rigidbody2D to predict.")]
		[SerializeField]
		private Rigidbody2D _rigidbody2d;

		[Tooltip("True to smooth position on spectated objects.")]
		[SerializeField]
		private bool _spectatorSmoothPosition = true;

		[Tooltip("True to smooth rotation on spectated objects.")]
		[SerializeField]
		private bool _spectatorSmoothRotation = true;

		[Tooltip("How to favor smoothing for predicted objects.")]
		[SerializeField]
		private SpectatorSmoothingType _spectatorSmoothingType = SpectatorSmoothingType.Mixed;

		[Tooltip("Custom settings for smoothing data.")]
		[SerializeField]
		private SmoothingData _customSmoothingData = _mixedSmoothingData;

		[SerializeField]
		private SmoothingData _preconfiguredSmoothingDataPreview = _mixedSmoothingData;

		[Tooltip("Multiplier applied to difference in velocity between ticks. Positive values will result in more velocity while lowers will result in less. A value of 1f will prevent any velocity from being lost between ticks, unless indicated by the server.")]
		[Range(-10f, 10f)]
		[SerializeField]
		private float _maintainedVelocity;

		[Tooltip("How often to resend current values regardless if the state has changed. Using this value will consume more bandwidth but may be preferred if you want to force synchronization the object move on the client but not on the server.")]
		[SerializeField]
		private ResendType _resendType;

		[Tooltip("How often in ticks to resend values.")]
		[SerializeField]
		private ushort _resendInterval = 30;

		[Tooltip("NetworkTransform to configure.")]
		[SerializeField]
		private NetworkTransform _networkTransform;

		private bool _clientSubscribed;

		private bool _registered;

		private Vector3 _graphicalInstantiatedOffsetPosition;

		private Quaternion _graphicalInstantiatedOffsetRotation;

		private uint _localTick;

		private PredictedObjectSpectatorSmoother _spectatorSmoother;

		private PredictedObjectOwnerSmoother _ownerSmoother;

		private RigidbodyPauser _rigidbodyPauser = new RigidbodyPauser();

		private uint _nextIntervalResend;

		private ushort _resendsRemaining;

		private bool _previouslyChanged;

		private Animator[] _graphicalAnimators;

		private bool _animatorsInitialized;

		private uint _lastStateLocalTick;

		private long _currentSpectatorInterpolation;

		private uint _targetSpectatorInterpolation;

		private uint _targetCollisionSpectatorInterpolation;

		private byte _interpolationDecreaseStep;

		private byte _interpolationIncreaseStep;

		private uint _collisionStayedTick;

		private HashSet<GameObject> _localClientCollidedObjects = new HashSet<GameObject>();

		private bool _spectatorPaused;

		private static SmoothingData _accurateSmoothingData = new SmoothingData
		{
			InterpolationPercent = 0.5f,
			CollisionInterpolationPercent = 0.05f,
			InterpolationDecreaseStep = 1,
			InterpolationIncreaseStep = 2
		};

		private static SmoothingData _mixedSmoothingData = new SmoothingData
		{
			InterpolationPercent = 1f,
			CollisionInterpolationPercent = 0.1f,
			InterpolationDecreaseStep = 1,
			InterpolationIncreaseStep = 3
		};

		private static SmoothingData _gradualSmoothingData = new SmoothingData
		{
			InterpolationPercent = 1.5f,
			CollisionInterpolationPercent = 0.2f,
			InterpolationDecreaseStep = 1,
			InterpolationIncreaseStep = 5
		};

		private uint _igtt;

		private RingBuffer<RigidbodyState> _rigidbodyStates = new RingBuffer<RigidbodyState>();

		private Vector3 _lastVelocity;

		private Vector3 _lastAngularVelocity;

		private float? _velocityBaseline;

		private float? _angularVelocityBaseline;

		private PhysicsScene _physicsScene;

		private RingBuffer<Rigidbody2DState> _rigidbody2dStates = new RingBuffer<Rigidbody2DState>();

		private Vector3 _lastVelocity2D;

		private float _lastAngularVelocity2D;

		private float? _velocityBaseline2D;

		private float? _angularVelocityBaseline2D;

		private PhysicsScene2D _physicsScene2D;

		private int _preReplicateReplayCacheIndex;

		private uint _lastPingUpdateTick;

		private long _lastPing;

		private bool NetworkInitialize___EarlyFishNet_002EComponent_002EPrediction_002EPredictedObjectFishNet_002ERuntime_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EComponent_002EPrediction_002EPredictedObjectFishNet_002ERuntime_002Edll_Excuted;

		public bool IsRigidbodyPrediction
		{
			get
			{
				if (_predictionType != PredictionType.Rigidbody)
				{
					return _predictionType == PredictionType.Rigidbody2D;
				}
				return true;
			}
		}

		public Transform GetGraphicalObject()
		{
			return _graphicalObject;
		}

		public void SetGraphicalObject(Transform value)
		{
			_graphicalObject = value;
			SetInstantiatedOffsetValues();
			_spectatorSmoother?.SetGraphicalObject(value);
			_ownerSmoother?.SetGraphicalObject(value);
		}

		[Obsolete("No longer used. This setting has been replaced by Smoothing Type.")]
		public bool GetSmoothTicks()
		{
			return true;
		}

		[Obsolete("No longer used. This setting has been replaced by Smoothing Type.")]
		public void SetSmoothTicks(bool value)
		{
		}

		[Obsolete("No longer used. This setting has been replaced by Smoothing Type.")]
		public byte GetInterpolation(bool asOwner)
		{
			return 0;
		}

		[Obsolete("No longer used. This setting has been replaced by Smoothing Type.")]
		public void SetInterpolation(byte value, bool asOwner)
		{
		}

		public void SetSpectatorSmoothingType(SpectatorSmoothingType value)
		{
			if (base.IsSpawned)
			{
				base.NetworkManager.LogWarning("Spectator smoothing type may only be set before the object is spawned, such as after instantiating but before spawning.");
			}
			else
			{
				_spectatorSmoothingType = value;
			}
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			Awake_UserLogic_FishNet_002EComponent_002EPrediction_002EPredictedObject_FishNet_002ERuntime_002Edll();
			NetworkInitialize__Late();
		}

		public override void OnStartNetwork()
		{
			if (base.IsHost)
			{
				InitializeSmoother(ownerSmoother: true);
			}
			UpdateRigidbodiesCount(add: true);
			ConfigureRigidbodies();
			ConfigureNetworkTransform();
			base.TimeManager.OnPostTick += TimeManager_OnPostTick;
		}

		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			Rigidbodies_OnSpawnServer(connection);
		}

		public override void OnStartClient()
		{
			ChangeSubscriptions(subscribe: true);
			Rigidbodies_OnStartClient();
		}

		public override void OnOwnershipClient(NetworkConnection prevOwner)
		{
			if (base.IsOwner && !base.IsServer)
			{
				InitializeSmoother(_implementsPredictionMethods);
				if (!_implementsPredictionMethods)
				{
					SetTargetSmoothing(base.TimeManager.RoundTripTime, setImmediately: true);
				}
			}
			else if (_predictionType != PredictionType.Other)
			{
				InitializeSmoother(ownerSmoother: false);
				SetTargetSmoothing(base.TimeManager.RoundTripTime, setImmediately: true);
			}
			Rigidbodies_OnOwnershipClient(prevOwner);
		}

		public override void OnStopNetwork()
		{
			ChangeSubscriptions(subscribe: false);
			UpdateRigidbodiesCount(add: false);
			base.TimeManager.OnPostTick -= TimeManager_OnPostTick;
		}

		private void UpdateRigidbodiesCount(bool add)
		{
			if (_registered == add || _predictionType == PredictionType.Other)
			{
				return;
			}
			NetworkManager networkManager = base.NetworkManager;
			if (!(networkManager == null))
			{
				_registered = add;
				if (add)
				{
					networkManager.PredictionManager.AddRigidbodyCount(this);
					networkManager.PredictionManager.OnPreServerReconcile += PredictionManager_OnPreServerReconcile;
				}
				else
				{
					networkManager.PredictionManager.RemoveRigidbodyCount(this);
					networkManager.PredictionManager.OnPreServerReconcile -= PredictionManager_OnPreServerReconcile;
				}
			}
		}

		private void SetInstantiatedOffsetValues()
		{
			base.transform.SetTransformOffsets(_graphicalObject, ref _graphicalInstantiatedOffsetPosition, ref _graphicalInstantiatedOffsetRotation);
		}

		private void TimeManager_OnUpdate()
		{
			_spectatorSmoother?.ManualUpdate();
			_ownerSmoother?.ManualUpdate();
		}

		private void TimeManager_OnPreTick()
		{
			_localTick = base.TimeManager.LocalTick;
			_spectatorSmoother?.OnPreTick();
			_ownerSmoother?.OnPreTick();
		}

		protected void TimeManager_OnPostTick()
		{
			_spectatorSmoother?.OnPostTick();
			_ownerSmoother?.OnPostTick();
			Rigidbodies_TimeManager_OnPostTick();
		}

		private void ChangeSubscriptions(bool subscribe)
		{
			if (base.TimeManager == null || subscribe == _clientSubscribed)
			{
				return;
			}
			if (subscribe)
			{
				base.TimeManager.OnUpdate += TimeManager_OnUpdate;
				base.TimeManager.OnPreTick += TimeManager_OnPreTick;
				if (!base.IsServer)
				{
					base.PredictionManager.OnPreReplicateReplay += PredictionManager_OnPreReplicateReplay;
					base.PredictionManager.OnPostReplicateReplay += PredictionManager_OnPostReplicateReplay;
					base.PredictionManager.OnPreReconcile += PredictionManager_OnPreReconcile;
					base.PredictionManager.OnPostReconcile += PredictionManager_OnPostReconcile;
					base.TimeManager.OnRoundTripTimeUpdated += TimeManager_OnRoundTripTimeUpdated;
				}
			}
			else
			{
				base.TimeManager.OnUpdate -= TimeManager_OnUpdate;
				base.TimeManager.OnPreTick -= TimeManager_OnPreTick;
				if (!base.IsServer)
				{
					base.PredictionManager.OnPreReplicateReplay -= PredictionManager_OnPreReplicateReplay;
					base.PredictionManager.OnPostReplicateReplay -= PredictionManager_OnPostReplicateReplay;
					base.PredictionManager.OnPreReconcile -= PredictionManager_OnPreReconcile;
					base.PredictionManager.OnPostReconcile -= PredictionManager_OnPostReconcile;
					base.TimeManager.OnRoundTripTimeUpdated -= TimeManager_OnRoundTripTimeUpdated;
				}
				_lastStateLocalTick = 0u;
				_rigidbodyStates.Clear();
				_rigidbody2dStates.Clear();
			}
			_clientSubscribed = subscribe;
		}

		private void TimeManager_OnRoundTripTimeUpdated(long obj)
		{
			Rigidbodies_OnRoundTripTimeUpdated(obj);
		}

		private void PredictionManager_OnPreServerReconcile(NetworkBehaviour obj)
		{
			SendRigidbodyState(obj);
		}

		protected virtual void PredictionManager_OnPreReplicateReplay(uint tick, PhysicsScene ps, PhysicsScene2D ps2d)
		{
			_spectatorSmoother?.OnPreReplay(tick);
			Rigidbodies_PredictionManager_OnPreReplicateReplay(tick, ps, ps2d);
		}

		private void PredictionManager_OnPostReplicateReplay(uint tick, PhysicsScene ps, PhysicsScene2D ps2d)
		{
			_spectatorSmoother?.OnPostReplay(tick);
			Rigidbodies_PredictionManager_OnPostReplicateReplay(tick, ps, ps2d);
		}

		private void PredictionManager_OnPreReconcile(NetworkBehaviour nb)
		{
			Rigidbodies_TimeManager_OnPreReconcile(nb);
		}

		private void PredictionManager_OnPostReconcile(NetworkBehaviour nb)
		{
			Rigidbodies_TimeManager_OnPostReconcile(nb);
		}

		private void InitializeSmoother(bool ownerSmoother)
		{
			ResetGraphicalTransform();
			if (ownerSmoother)
			{
				_ownerSmoother = new PredictedObjectOwnerSmoother();
				float teleportThreshold = (_enableTeleport ? _teleportThreshold : (-1f));
				_ownerSmoother.Initialize(this, _graphicalInstantiatedOffsetPosition, _graphicalInstantiatedOffsetRotation, _graphicalObject, _ownerSmoothPosition, _ownerSmoothRotation, _ownerInterpolation, teleportThreshold);
			}
			else
			{
				_spectatorSmoother = new PredictedObjectSpectatorSmoother();
				RigidbodyType rbType = ((_predictionType != PredictionType.Rigidbody) ? RigidbodyType.Rigidbody2D : RigidbodyType.Rigidbody);
				float teleportThreshold2 = (_enableTeleport ? _teleportThreshold : (-1f));
				_spectatorSmoother.Initialize(this, rbType, _rigidbody, _rigidbody2d, _graphicalObject, _spectatorSmoothPosition, _spectatorSmoothRotation, teleportThreshold2);
			}
			void ResetGraphicalTransform()
			{
				_graphicalObject.position = base.transform.position + _graphicalInstantiatedOffsetPosition;
				_graphicalObject.rotation = _graphicalInstantiatedOffsetRotation * base.transform.rotation;
			}
		}

		private void ConfigureRigidbodies()
		{
			if (IsRigidbodyPrediction)
			{
				_rigidbodyPauser = new RigidbodyPauser();
				if (_predictionType == PredictionType.Rigidbody)
				{
					_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
					_rigidbodyPauser.UpdateRigidbodies(base.transform, RigidbodyType.Rigidbody, getInChildren: true, _graphicalObject);
				}
				else
				{
					_rigidbody2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
					_rigidbodyPauser.UpdateRigidbodies(base.transform, RigidbodyType.Rigidbody2D, getInChildren: true, _graphicalObject);
				}
			}
		}

		private void ConfigureNetworkTransform()
		{
			if (!IsRigidbodyPrediction)
			{
				_networkTransform?.ConfigureForCSP();
			}
		}

		internal bool IsPredictingOwner()
		{
			if (base.IsOwner)
			{
				return _implementsPredictionMethods;
			}
			return false;
		}

		private bool _isPredictingOwner(NetworkConnection c)
		{
			if (c == base.Owner)
			{
				return _implementsPredictionMethods;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Rigidbodies_OnSpawnServer(NetworkConnection c)
		{
			if (IsRigidbodyPrediction && !(c == base.Owner) && !c.IsLocalClient)
			{
				uint remoteTick = c.PacketTick.RemoteTick;
				if (_predictionType == PredictionType.Rigidbody)
				{
					SendRigidbodyState(remoteTick, c, applyImmediately: true);
				}
				else
				{
					SendRigidbody2DState(remoteTick, c, applyImmediately: true);
				}
			}
		}

		private void Rigidbodies_OnStartClient()
		{
			int tickRate = base.TimeManager.TickRate;
			if (tickRate != _rigidbodyStates.Capacity)
			{
				_rigidbodyStates.Initialize(tickRate);
				_rigidbody2dStates.Initialize(tickRate);
			}
		}

		private void Rigidbodies_OnOwnershipClient(NetworkConnection prevOwner)
		{
			if (!IsRigidbodyPrediction || base.IsOwner || _animatorsInitialized)
			{
				return;
			}
			_animatorsInitialized = true;
			_graphicalAnimators = _graphicalObject.GetComponentsInChildren<Animator>(includeInactive: true);
			if (_graphicalAnimators.Length != 0)
			{
				for (int i = 0; i < _graphicalAnimators.Length; i++)
				{
					_graphicalAnimators[i].keepAnimatorStateOnDisable = true;
				}
				if (_graphicalAnimators[0].transform == _graphicalObject)
				{
					Transform transform = new GameObject().transform;
					transform.name = "GraphicalObjectHolder";
					transform.SetParent(base.transform);
					transform.localPosition = _graphicalInstantiatedOffsetPosition;
					transform.localRotation = _graphicalInstantiatedOffsetRotation;
					transform.localScale = _graphicalObject.localScale;
					_graphicalObject.SetParent(transform);
					_graphicalObject.localPosition = Vector3.zero;
					_graphicalObject.localRotation = Quaternion.identity;
					_graphicalObject.localScale = Vector3.one;
					SetGraphicalObject(transform);
				}
			}
		}

		private void Rigidbodies_TimeManager_OnPostTick()
		{
			if (!IsRigidbodyPrediction || base.IsServer)
			{
				return;
			}
			bool flag = _predictionType == PredictionType.Rigidbody2D;
			TrySetCollisionExited(flag);
			if (_rigidbodyStates.Initialized)
			{
				if (_localTick == 0)
				{
					_localTick = base.TimeManager.LocalTick;
				}
				if (!flag)
				{
					_rigidbodyStates.Add(new RigidbodyState(_rigidbody, _localTick));
				}
				else
				{
					_rigidbody2dStates.Add(new Rigidbody2DState(_rigidbody2d, _localTick));
				}
			}
			if (CanPredict())
			{
				UpdateSpectatorSmoothing();
				if (!flag)
				{
					PredictVelocity(base.gameObject.scene.GetPhysicsScene());
				}
				else
				{
					PredictVelocity(base.gameObject.scene.GetPhysicsScene2D());
				}
			}
		}

		private void TrySetCollisionExited(bool is2d)
		{
			if (_collisionStayedTick != 0 && base.TimeManager.LocalTick != _collisionStayedTick)
			{
				CollisionExited();
			}
		}

		private void Rigidbodies_TimeManager_OnPreReconcile(NetworkBehaviour nb)
		{
			if (IsPredictingOwner() || nb.gameObject == base.gameObject || !IsRigidbodyPrediction)
			{
				return;
			}
			bool flag = _predictionType == PredictionType.Rigidbody2D;
			uint lastReconcileTick = nb.GetLastReconcileTick();
			int cachedStateIndex = GetCachedStateIndex(lastReconcileTick, flag);
			if (cachedStateIndex == -1)
			{
				_spectatorSmoother?.SetLocalReconcileTick(-1L);
				_rigidbodyPauser.Pause();
				return;
			}
			_spectatorSmoother?.SetLocalReconcileTick(lastReconcileTick);
			if (flag)
			{
				_rigidbody2dStates.RemoveRange(fromStart: true, cachedStateIndex);
				ResetRigidbody2DToData(_rigidbody2dStates[0]);
			}
			else
			{
				_rigidbodyStates.RemoveRange(fromStart: true, cachedStateIndex);
				ResetRigidbodyToData(_rigidbodyStates[0]);
			}
		}

		private void Rigidbodies_TimeManager_OnPostReconcile(NetworkBehaviour nb)
		{
			_rigidbodyPauser.Unpause();
		}

		private void Rigidbodies_PredictionManager_OnPreReplicateReplay(uint tick, PhysicsScene ps, PhysicsScene2D ps2d)
		{
			if (!CanPredict())
			{
				return;
			}
			if (_predictionType == PredictionType.Rigidbody)
			{
				_preReplicateReplayCacheIndex = GetCachedStateIndex(tick, is2d: false);
				if (_preReplicateReplayCacheIndex != -1)
				{
					bool isKinematic = _rigidbodyStates[_preReplicateReplayCacheIndex].IsKinematic;
					_rigidbody.isKinematic = isKinematic;
				}
				PredictVelocity(ps);
			}
			else if (_predictionType == PredictionType.Rigidbody2D)
			{
				_preReplicateReplayCacheIndex = GetCachedStateIndex(tick, is2d: true);
				if (_preReplicateReplayCacheIndex != -1)
				{
					Rigidbody2DState rigidbody2DState = _rigidbody2dStates[_preReplicateReplayCacheIndex];
					_rigidbody2d.simulated = rigidbody2DState.Simulated;
					_rigidbody2d.isKinematic = rigidbody2DState.IsKinematic;
				}
				PredictVelocity(ps2d);
			}
		}

		private void Rigidbodies_PredictionManager_OnPostReplicateReplay(uint tick, PhysicsScene ps, PhysicsScene2D ps2d)
		{
			if (!CanPredict() || _rigidbodyPauser.Paused)
			{
				return;
			}
			if (_predictionType == PredictionType.Rigidbody)
			{
				int preReplicateReplayCacheIndex = _preReplicateReplayCacheIndex;
				if (preReplicateReplayCacheIndex != -1)
				{
					bool isKinematic = _rigidbodyStates[preReplicateReplayCacheIndex].IsKinematic;
					_rigidbodyStates[preReplicateReplayCacheIndex] = new RigidbodyState(_rigidbody, isKinematic, tick);
				}
			}
			if (_predictionType == PredictionType.Rigidbody2D)
			{
				int cachedStateIndex = GetCachedStateIndex(tick, is2d: true);
				if (cachedStateIndex != -1)
				{
					bool simulated = _rigidbody2dStates[cachedStateIndex].Simulated;
					_rigidbody2dStates[cachedStateIndex] = new Rigidbody2DState(_rigidbody2d, simulated, tick);
				}
			}
		}

		public void SetPauseSpectatorCorrections_Experimental(bool pause)
		{
			_spectatorPaused = pause;
			if (pause)
			{
				_rigidbodyStates.Clear();
				_rigidbody2dStates.Clear();
			}
		}

		private void Rigidbodies_OnRoundTripTimeUpdated(long ping)
		{
			if ((ulong)Mathf.Abs(ping - _lastPing) < 50)
			{
				uint num = base.TimeManager.TimeToTicks(5.0, TickRounding.RoundUp);
				if (base.TimeManager.LocalTick - _lastPingUpdateTick < num)
				{
					return;
				}
			}
			SetTargetSmoothing(ping, setImmediately: false);
		}

		private void SetTargetSmoothing(long ping, bool setImmediately)
		{
			if (_spectatorSmoother != null)
			{
				_lastPingUpdateTick = base.TimeManager.LocalTick;
				_lastPing = ping;
				SetValues();
				if (setImmediately)
				{
					_currentSpectatorInterpolation = (CollidingWithLocalClient() ? _targetCollisionSpectatorInterpolation : _targetSpectatorInterpolation);
					_spectatorSmoother.SetInterpolation((uint)_currentSpectatorInterpolation);
				}
			}
			void SetValues()
			{
				SmoothingData smoothingData = ((_spectatorSmoothingType == SpectatorSmoothingType.Accuracy) ? _accurateSmoothingData : ((_spectatorSmoothingType == SpectatorSmoothingType.Mixed) ? _mixedSmoothingData : ((_spectatorSmoothingType != SpectatorSmoothingType.Gradual) ? _customSmoothingData : _gradualSmoothingData)));
				TimeManager timeManager = base.TimeManager;
				double time = (double)ping / 1000.0 * (double)smoothingData.InterpolationPercent;
				_targetSpectatorInterpolation = timeManager.TimeToTicks(time, TickRounding.RoundUp);
				double time2 = (double)ping / 1000.0 * (double)smoothingData.CollisionInterpolationPercent;
				_targetCollisionSpectatorInterpolation = timeManager.TimeToTicks(time2, TickRounding.RoundUp);
				_interpolationDecreaseStep = smoothingData.InterpolationDecreaseStep;
				_interpolationIncreaseStep = smoothingData.InterpolationIncreaseStep;
			}
		}

		private bool CollidingWithLocalClient()
		{
			return base.TimeManager.LocalTick - _collisionStayedTick < 1;
		}

		private void UpdateSpectatorSmoothing()
		{
			if (CollidingWithLocalClient())
			{
				_currentSpectatorInterpolation -= _interpolationDecreaseStep;
			}
			else
			{
				_currentSpectatorInterpolation += _interpolationIncreaseStep;
			}
			_currentSpectatorInterpolation = (long)Mathf.Clamp(_currentSpectatorInterpolation, _targetCollisionSpectatorInterpolation, _targetSpectatorInterpolation);
			_spectatorSmoother.SetInterpolation((uint)_currentSpectatorInterpolation);
		}

		private bool CollisionEnteredLocalClientObject(GameObject go)
		{
			if (go.TryGetComponent<NetworkObject>(out var component))
			{
				return component.Owner.IsLocalClient;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SendRigidbodyState(NetworkBehaviour nb)
		{
			if (nb.Owner.IsActive && !(nb.NetworkManager == null))
			{
				uint lastReplicateTick = nb.GetLastReplicateTick();
				TrySendRigidbodyState(nb, lastReplicateTick);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TrySendRigidbodyState(NetworkBehaviour nb, uint tick)
		{
			if (!IsRigidbodyPrediction)
			{
				return;
			}
			NetworkConnection owner = nb.Owner;
			if (_isPredictingOwner(owner) || owner.IsLocalClient || !base.Observers.Contains(owner))
			{
				return;
			}
			bool flag = PredictedTransformMayChange();
			uint currentTick;
			if (!flag)
			{
				if (_previouslyChanged)
				{
					_resendsRemaining = base.TimeManager.TickRate;
				}
				currentTick = base.TimeManager.Tick;
				if (_resendsRemaining > 0)
				{
					_resendsRemaining--;
					if (_resendsRemaining == 0)
					{
						UpdateNextIntervalResend();
					}
				}
				else
				{
					if (_resendType == ResendType.Disabled || currentTick < _nextIntervalResend)
					{
						return;
					}
					UpdateNextIntervalResend();
				}
			}
			_previouslyChanged = flag;
			if (_predictionType == PredictionType.Rigidbody)
			{
				SendRigidbodyState(tick, owner, applyImmediately: false);
			}
			else
			{
				SendRigidbody2DState(tick, owner, applyImmediately: false);
			}
			void UpdateNextIntervalResend()
			{
				_nextIntervalResend = currentTick + _resendInterval;
			}
		}

		private int GetCachedStateIndex(uint tick, bool is2d)
		{
			int count;
			uint localTick;
			if (!is2d)
			{
				count = _rigidbodyStates.Count;
				if (count == 0)
				{
					return -1;
				}
				localTick = _rigidbodyStates[0].LocalTick;
			}
			else
			{
				count = _rigidbody2dStates.Count;
				if (count == 0)
				{
					return -1;
				}
				localTick = _rigidbody2dStates[0].LocalTick;
			}
			if (localTick > tick)
			{
				return -1;
			}
			long num = tick - localTick;
			if (num >= count)
			{
				return -1;
			}
			return (int)num;
		}

		protected bool PredictVector3Velocity(ref float? velocityBaseline, ref Vector3 lastVelocity, Vector3 velocity, out Vector3 result)
		{
			if ((velocityBaseline.HasValue ? Vector3.SqrMagnitude(lastVelocity.normalized - velocity.normalized) : 0f) > 0.01f)
			{
				velocityBaseline = null;
			}
			else
			{
				float num = Vector3.Magnitude(lastVelocity - velocity);
				if (!velocityBaseline.HasValue)
				{
					if (num > 0f)
					{
						velocityBaseline = num;
					}
				}
				else
				{
					if (!(num > velocityBaseline.Value * 1.1f) && !(num < velocityBaseline.Value * 0.9f))
					{
						Vector3 vector = (velocity - lastVelocity) * _maintainedVelocity;
						if (_maintainedVelocity > 0f)
						{
							result = velocity + vector;
						}
						else
						{
							result = velocity + vector;
							if (velocity.normalized != result.normalized)
							{
								result = Vector3.zero;
							}
						}
						return true;
					}
					velocityBaseline = null;
				}
			}
			result = Vector3.zero;
			return false;
		}

		private bool PredictFloatVelocity(ref float? velocityBaseline, ref float lastVelocity, float velocity, out float result)
		{
			if ((velocityBaseline.HasValue ? (velocity - lastVelocity) : 0f) > 0.01f)
			{
				velocityBaseline = null;
			}
			else
			{
				float num = Mathf.Abs(lastVelocity - velocity);
				if (!velocityBaseline.HasValue)
				{
					if (num > 0f)
					{
						velocityBaseline = num;
					}
				}
				else
				{
					if (!(num > velocityBaseline.Value * 1.1f) && !(num < velocityBaseline.Value * 0.9f))
					{
						float num2 = (velocity - lastVelocity) * _maintainedVelocity;
						if (_maintainedVelocity > 0f)
						{
							result = velocity + num2;
						}
						else
						{
							result = velocity + num2;
							if (Mathf.Abs(velocity) != Mathf.Abs(result))
							{
								result = 0f;
							}
						}
						return true;
					}
					velocityBaseline = null;
				}
			}
			result = 0f;
			return false;
		}

		private bool CanPredict()
		{
			if (!IsRigidbodyPrediction)
			{
				return false;
			}
			if (base.IsServer || IsPredictingOwner())
			{
				return false;
			}
			if (_spectatorPaused)
			{
				return false;
			}
			return true;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_predictionType == PredictionType.Rigidbody)
			{
				GameObject go = collision.gameObject;
				if (CollisionEnteredLocalClientObject(go))
				{
					CollisionEntered(go);
				}
			}
		}

		private void OnCollisionStay(Collision collision)
		{
			if (_predictionType == PredictionType.Rigidbody && _localClientCollidedObjects.Contains(collision.gameObject))
			{
				_collisionStayedTick = base.TimeManager.LocalTick;
			}
		}

		private void ResetRigidbodyToData(RigidbodyState state)
		{
			_rigidbody.transform.position = state.Position;
			_rigidbody.transform.rotation = state.Rotation;
			bool isKinematic = state.IsKinematic;
			_rigidbody.isKinematic = isKinematic;
			if (!isKinematic)
			{
				_rigidbody.velocity = state.Velocity;
				_rigidbody.angularVelocity = state.AngularVelocity;
			}
			_velocityBaseline = null;
			_angularVelocityBaseline = null;
			_lastVelocity = _rigidbody.velocity;
			_lastAngularVelocity = _rigidbody.angularVelocity;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void PredictVelocity(PhysicsScene ps)
		{
			if (_maintainedVelocity != 0f && !(ps != _physicsScene))
			{
				if (PredictVector3Velocity(ref _velocityBaseline, ref _lastVelocity, _rigidbody.velocity, out var result))
				{
					_rigidbody.velocity = result;
				}
				if (PredictVector3Velocity(ref _angularVelocityBaseline, ref _lastAngularVelocity, _rigidbody.angularVelocity, out result))
				{
					_rigidbody.angularVelocity = result;
				}
				_lastVelocity = _rigidbody.velocity;
				_lastAngularVelocity = _rigidbody.angularVelocity;
			}
		}

		private void SendRigidbodyState(uint reconcileTick, NetworkConnection conn, bool applyImmediately)
		{
			if (!_isPredictingOwner(conn))
			{
				reconcileTick = ((conn == base.NetworkObject.PredictedSpawner) ? conn.PacketTick.RemoteTick : reconcileTick);
				RigidbodyState state = new RigidbodyState(_rigidbody, reconcileTick);
				TargetSendRigidbodyState(conn, state, applyImmediately);
			}
		}

		[TargetRpc(ValidateTarget = false)]
		private void TargetSendRigidbodyState(NetworkConnection c, RigidbodyState state, bool applyImmediately, Channel channel = Channel.Unreliable)
		{
			RpcWriter___Target_TargetSendRigidbodyState_1016043495(c, state, applyImmediately, channel);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (_predictionType == PredictionType.Rigidbody2D)
			{
				GameObject go = collision.gameObject;
				if (CollisionEnteredLocalClientObject(go))
				{
					CollisionEntered(go);
				}
			}
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			if (_predictionType == PredictionType.Rigidbody2D && _localClientCollidedObjects.Contains(collision.gameObject))
			{
				_collisionStayedTick = base.TimeManager.LocalTick;
			}
		}

		private void CollisionEntered(GameObject go)
		{
			_collisionStayedTick = base.TimeManager.LocalTick;
			_localClientCollidedObjects.Add(go);
		}

		private void CollisionExited()
		{
			_localClientCollidedObjects.Clear();
			_collisionStayedTick = 0u;
		}

		private void ResetRigidbody2DToData(Rigidbody2DState state)
		{
			_rigidbody2d.transform.position = state.Position;
			_rigidbody2d.transform.rotation = state.Rotation;
			bool simulated = state.Simulated;
			_rigidbody2d.simulated = simulated;
			_rigidbody2d.isKinematic = state.IsKinematic;
			if (simulated)
			{
				_rigidbody2d.velocity = state.Velocity;
				_rigidbody2d.angularVelocity = state.AngularVelocity;
			}
			_velocityBaseline2D = null;
			_angularVelocityBaseline2D = null;
			_lastVelocity2D = _rigidbody2d.velocity;
			_lastAngularVelocity2D = _rigidbody2d.angularVelocity;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void PredictVelocity(PhysicsScene2D ps)
		{
			if (_maintainedVelocity != 0f && !(ps != _physicsScene2D))
			{
				if (PredictVector3Velocity(ref _velocityBaseline2D, ref _lastVelocity2D, _rigidbody2d.velocity, out var result))
				{
					_rigidbody2d.velocity = result;
				}
				if (PredictFloatVelocity(ref _angularVelocityBaseline2D, ref _lastAngularVelocity2D, _rigidbody2d.angularVelocity, out var result2))
				{
					_rigidbody2d.angularVelocity = result2;
				}
				_lastVelocity2D = _rigidbody2d.velocity;
				_lastAngularVelocity2D = _rigidbody2d.angularVelocity;
			}
		}

		private void SendRigidbody2DState(uint reconcileTick, NetworkConnection conn, bool applyImmediately)
		{
			Rigidbody2DState state = new Rigidbody2DState(_rigidbody2d, reconcileTick);
			TargetSendRigidbody2DState(conn, state, applyImmediately);
		}

		[TargetRpc(ValidateTarget = false)]
		private void TargetSendRigidbody2DState(NetworkConnection c, Rigidbody2DState state, bool applyImmediately, Channel channel = Channel.Unreliable)
		{
			RpcWriter___Target_TargetSendRigidbody2DState_700510009(c, state, applyImmediately, channel);
		}

		private bool CanProcessReceivedState(uint stateTick)
		{
			if (stateTick <= _lastStateLocalTick)
			{
				return false;
			}
			_lastStateLocalTick = stateTick;
			return true;
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EComponent_002EPrediction_002EPredictedObjectFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EComponent_002EPrediction_002EPredictedObjectFishNet_002ERuntime_002Edll_Excuted = true;
				RegisterTargetRpc(0u, RpcReader___Target_TargetSendRigidbodyState_1016043495);
				RegisterTargetRpc(1u, RpcReader___Target_TargetSendRigidbody2DState_700510009);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EComponent_002EPrediction_002EPredictedObjectFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EComponent_002EPrediction_002EPredictedObjectFishNet_002ERuntime_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void RpcWriter___Target_TargetSendRigidbodyState_1016043495(NetworkConnection c, RigidbodyState state, bool applyImmediately, Channel channel = Channel.Unreliable)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel2 = channel;
				PooledWriter writer = WriterPool.GetWriter();
				writer.WriteRigidbodyState(state);
				GeneratedWriters___Internal.InstancedExtension___WriteBoolean(writer, applyImmediately);
				SendTargetRpc(0u, writer, channel2, DataOrderType.Default, c, excludeServer: false, validateTarget: false);
				writer.Store();
			}
		}

		private void RpcLogic___TargetSendRigidbodyState_1016043495(NetworkConnection c, RigidbodyState state, bool applyImmediately, Channel channel = Channel.Unreliable)
		{
			if (!CanPredict())
			{
				return;
			}
			uint localTick = state.LocalTick;
			if (applyImmediately)
			{
				if (base.NetworkObject.PredictedSpawner.IsLocalClient)
				{
					return;
				}
			}
			else if (!CanProcessReceivedState(localTick))
			{
				return;
			}
			if (applyImmediately)
			{
				_rigidbodyStates.Clear();
				ResetRigidbodyToData(state);
				return;
			}
			int cachedStateIndex = GetCachedStateIndex(localTick, is2d: false);
			if (cachedStateIndex != -1)
			{
				_rigidbodyStates[cachedStateIndex] = state;
			}
			else
			{
				_rigidbodyStates.Add(state);
			}
		}

		private void RpcReader___Target_TargetSendRigidbodyState_1016043495(PooledReader PooledReader0, Channel channel)
		{
			RigidbodyState state = PooledReader0.ReadRigidbodyState();
			bool applyImmediately = GeneratedReaders___Internal.InstancedExtension___ReadBoolean(PooledReader0);
			if (base.IsClientInitialized)
			{
				RpcLogic___TargetSendRigidbodyState_1016043495(base.LocalConnection, state, applyImmediately, channel);
			}
		}

		private void RpcWriter___Target_TargetSendRigidbody2DState_700510009(NetworkConnection c, Rigidbody2DState state, bool applyImmediately, Channel channel = Channel.Unreliable)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel2 = channel;
				PooledWriter writer = WriterPool.GetWriter();
				writer.WriteRigidbody2DState(state);
				GeneratedWriters___Internal.InstancedExtension___WriteBoolean(writer, applyImmediately);
				SendTargetRpc(1u, writer, channel2, DataOrderType.Default, c, excludeServer: false, validateTarget: false);
				writer.Store();
			}
		}

		private void RpcLogic___TargetSendRigidbody2DState_700510009(NetworkConnection c, Rigidbody2DState state, bool applyImmediately, Channel channel = Channel.Unreliable)
		{
			if (!CanPredict())
			{
				return;
			}
			uint localTick = state.LocalTick;
			if (applyImmediately)
			{
				if (base.NetworkObject.PredictedSpawner.IsLocalClient)
				{
					return;
				}
			}
			else if (!CanProcessReceivedState(localTick))
			{
				return;
			}
			if (applyImmediately)
			{
				_rigidbody2dStates.Clear();
				ResetRigidbody2DToData(state);
				return;
			}
			int cachedStateIndex = GetCachedStateIndex(localTick, is2d: true);
			if (cachedStateIndex != -1)
			{
				_rigidbody2dStates[cachedStateIndex] = state;
			}
			else
			{
				_rigidbody2dStates.Add(state);
			}
		}

		private void RpcReader___Target_TargetSendRigidbody2DState_700510009(PooledReader PooledReader0, Channel channel)
		{
			Rigidbody2DState state = PooledReader0.ReadRigidbody2DState();
			bool applyImmediately = GeneratedReaders___Internal.InstancedExtension___ReadBoolean(PooledReader0);
			if (base.IsClientInitialized)
			{
				RpcLogic___TargetSendRigidbody2DState_700510009(base.LocalConnection, state, applyImmediately, channel);
			}
		}

		private void Awake_UserLogic_FishNet_002EComponent_002EPrediction_002EPredictedObject_FishNet_002ERuntime_002Edll()
		{
			SetInstantiatedOffsetValues();
		}
	}
}

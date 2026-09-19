using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion.Addons.Physics
{
	[DisallowMultipleComponent]
	[NetworkBehaviourWeaved(21)]
	public class NetworkRigidbody : NetworkTRSP, INetworkTRSPTeleport, IBeforeAllTicks, IPublicFacingInterface, IAfterTick, IInterestEnter
	{
		private const int WORDS = 21;

		[InlineHelp]
		[SerializeField]
		public bool SyncScale;

		[InlineHelp]
		[SerializeField]
		public bool SyncParent = true;

		[InlineHelp]
		[SerializeField]
		private Transform _interpolationTarget;

		private bool _rootIsDirtyFromInterpolation;

		private bool _targIsDirtyFromInterpolation;

		private AbstractPhysicsBody _physicsBody;

		private Transform _transform;

		private bool _aoiEnabled;

		private int _remainingSimulationsCount;

		private bool _doNotInterpolate;

		public Vector3 RBPosition => _physicsBody.Position;

		public Quaternion RBRotation => _physicsBody.Rotation;

		public bool RBIsKinematic => _physicsBody.Kinematic;

		public bool Is3D { get; private set; }

		public Transform InterpolationTarget
		{
			get
			{
				return _interpolationTarget;
			}
			set
			{
				SetInterpolationTarget(value);
			}
		}

		private ref NetworkTRSPData _transformData => ref base.State;

		private ref NetworkPhysicsData _physicsData => ref ReinterpretState<NetworkPhysicsData>(14);

		public void SetInterpolationTarget(Transform target)
		{
			if (target == null || target == base.transform)
			{
				_interpolationTarget = null;
				_targIsDirtyFromInterpolation = false;
			}
			else
			{
				_interpolationTarget = target;
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			if (SetupPhysicsBody())
			{
				_aoiEnabled = base.Runner.Config.Simulation.AreaOfInterestEnabled;
				_doNotInterpolate = base.Runner.Mode == SimulationModes.Server;
				_transform = base.transform;
				base.Runner.SetIsSimulated(base.Object, simulate: true);
			}
		}

		private bool SetupPhysicsBody()
		{
			if (base.Runner.Topology == Topologies.Shared)
			{
				Debug.LogWarning(GetType().Name + " should not be used in shared mode. Despawning " + base.gameObject.name + ".");
				base.Runner.Despawn(base.Object);
				return false;
			}
			Rigidbody2D component3;
			if (TryGetComponent<Rigidbody>(out var component))
			{
				_physicsBody = new PhysicsBody3D(component);
				Is3D = true;
				if (!base.Runner.TryGetComponent<RunnerSimulatePhysics>(out var component2))
				{
					component2 = base.Runner.gameObject.AddComponent<RunnerSimulatePhysics>();
					base.Runner.AddGlobal(component2);
				}
				component2.Update3DPhysicsScene = true;
			}
			else if (TryGetComponent<Rigidbody2D>(out component3))
			{
				_physicsBody = new PhysicsBody2D(component3);
				Is3D = false;
				if (!base.Runner.TryGetComponent<RunnerSimulatePhysics>(out var component4))
				{
					component4 = base.Runner.gameObject.AddComponent<RunnerSimulatePhysics>();
					base.Runner.AddGlobal(component4);
				}
				component4.Update2DPhysicsScene = true;
			}
			return true;
		}

		public void Teleport(Vector3? position = null, Quaternion? rotation = null)
		{
			if (position.HasValue)
			{
				_transform.position = position.Value;
				_transformData.Position = position.Value;
				_physicsBody.Position = position.Value;
			}
			if (rotation.HasValue)
			{
				_transform.rotation = rotation.Value;
				_transformData.Rotation = rotation.Value;
				_physicsBody.Rotation = rotation.Value;
			}
			_transformData.TeleportKey++;
		}

		void IBeforeAllTicks.BeforeAllTicks(bool resimulation, int tickCount)
		{
			_remainingSimulationsCount = tickCount;
			if (_targIsDirtyFromInterpolation && (bool)_interpolationTarget)
			{
				_interpolationTarget.localPosition = default(Vector3);
				_interpolationTarget.localRotation = Quaternion.identity;
				if (SyncScale)
				{
					_interpolationTarget.localScale = Vector3.one;
				}
			}
			if (_rootIsDirtyFromInterpolation || resimulation)
			{
				CopyToEngine();
			}
		}

		void IAfterTick.AfterTick()
		{
			if (_remainingSimulationsCount-- <= 2)
			{
				CopyToBuffer();
			}
		}

		private void CaptureExtras(ref NetworkPhysicsData data)
		{
			data.LinearVelocity = _physicsBody.LinearVelocity;
			data.AngularVelocity = _physicsBody.AngularVelocity;
		}

		private void ApplyExtras(ref NetworkPhysicsData data)
		{
			_physicsBody.LinearVelocity = data.LinearVelocity;
			_physicsBody.AngularVelocity = data.AngularVelocity;
			_physicsBody.EncodedConstraints = data.Constraints;
		}

		protected virtual void CopyToBuffer()
		{
			Transform transform = _transform;
			bool syncParent = SyncParent;
			bool flag = !syncParent;
			if (syncParent)
			{
				if (base.IsMainTRSP)
				{
					Transform parent = transform.parent;
					NetworkBehaviour component;
					if (parent == null)
					{
						_transformData.AreaOfInterestOverride = default(NetworkId);
						_transformData.Parent = default(NetworkBehaviourId);
					}
					else if (parent.TryGetComponent<NetworkBehaviour>(out component))
					{
						if (_aoiEnabled)
						{
							SetAreaOfInterestOverride(component.Object);
						}
						_transformData.Parent = component;
					}
					else
					{
						_transformData.AreaOfInterestOverride = default(NetworkId);
						_transformData.Parent = NetworkTRSPData.NonNetworkedParent;
						flag = true;
					}
				}
				else
				{
					_transformData.AreaOfInterestOverride = default(NetworkId);
				}
			}
			flag |= (bool)_interpolationTarget;
			Vector3 position = (flag ? _transform.position : _transform.localPosition);
			Quaternion rotation = (flag ? _transform.rotation : _transform.localRotation);
			_transformData.Position = position;
			_transformData.Rotation = rotation;
			if (!_physicsBody.Kinematic)
			{
				CaptureExtras(ref _physicsData);
			}
			_physicsData.FlagsAndConstraints = (flags: _physicsBody.Flags, constraints: _physicsBody.EncodedConstraints);
			if (SyncScale)
			{
				_transformData.Scale = transform.localScale;
			}
		}

		protected virtual void CopyToEngine(bool forceAwake = false)
		{
			if (_physicsBody == null)
			{
				return;
			}
			NetworkRigidbodyFlags item = _physicsData.FlagsAndConstraints.flags;
			Transform transform = _transform;
			bool syncParent = SyncParent;
			bool flag = (item & NetworkRigidbodyFlags.IsSleeping) != 0;
			bool flag2 = (item & NetworkRigidbodyFlags.IsKinematic) != 0;
			bool sleeping = _physicsBody.Sleeping;
			bool worldSpace = !syncParent;
			AbstractPhysicsBody physicsBody;
			bool sleeping2;
			if (sleeping != flag)
			{
				physicsBody = _physicsBody;
				if (flag)
				{
					if (!IsRigidbodyBelowSleepingThresholds(_physicsBody))
					{
						goto IL_0080;
					}
					sleeping2 = true;
				}
				else
				{
					if (IsRigidbodyBelowSleepingThresholds(_physicsBody))
					{
						goto IL_0080;
					}
					sleeping2 = false;
				}
				goto IL_008d;
			}
			goto IL_00a1;
			IL_00a1:
			if (syncParent)
			{
				Transform parent = transform.parent;
				if (_transformData.Parent != default(NetworkBehaviourId))
				{
					bool flag3 = _transformData.Parent == NetworkTRSPData.NonNetworkedParent;
					worldSpace = flag3;
					if (base.Runner.TryFindBehaviour(_transformData.Parent, out var behaviour))
					{
						Transform transform2 = behaviour.transform;
						if ((object)transform2 != parent)
						{
							transform.SetParent(transform2);
							if ((bool)_interpolationTarget)
							{
								_interpolationTarget.localPosition = default(Vector3);
								_interpolationTarget.localRotation = Quaternion.identity;
							}
						}
					}
					else if (!flag3)
					{
						Debug.LogError("Cannot find parent NetworkBehaviour.");
					}
				}
				else if ((bool)parent)
				{
					transform.SetParent(null);
				}
			}
			Vector3 position = _transformData.Position;
			Quaternion rotation = _transformData.Rotation;
			if ((bool)_interpolationTarget)
			{
				worldSpace = true;
			}
			bool flag4 = sleeping && flag;
			if (!flag4 || forceAwake)
			{
				SetPosRotToTransform(transform, position, rotation, worldSpace);
				_physicsBody.Position = transform.position;
				_physicsBody.Rotation = transform.rotation;
				_rootIsDirtyFromInterpolation = false;
			}
			if (SyncScale)
			{
				transform.localScale = _transformData.Scale;
			}
			if (flag2 != _physicsBody.Kinematic)
			{
				_physicsBody.Kinematic = flag2;
			}
			if (!flag2 && !flag4)
			{
				ApplyExtras(ref _physicsData);
			}
			return;
			IL_0080:
			sleeping2 = _physicsBody.Sleeping;
			goto IL_008d;
			IL_008d:
			physicsBody.Sleeping = sleeping2;
			sleeping = _physicsBody.Sleeping;
			goto IL_00a1;
		}

		void IInterestEnter.InterestEnter(PlayerRef player)
		{
			CopyToEngine(forceAwake: true);
		}

		private bool IsRigidbodyBelowSleepingThresholds(AbstractPhysicsBody physicsBody)
		{
			if (Is3D)
			{
				float sqrMagnitude = physicsBody.LinearVelocity.sqrMagnitude;
				float num = physicsBody.Mass * sqrMagnitude;
				Vector3 angularVelocity = physicsBody.AngularVelocity;
				Vector3 inertiaTensor = physicsBody.InertiaTensor;
				return (num + inertiaTensor.x * (angularVelocity.x * angularVelocity.x) + inertiaTensor.y * (angularVelocity.y * angularVelocity.y) + inertiaTensor.z * (angularVelocity.z * angularVelocity.z)) / (2f * physicsBody.Mass) <= UnityEngine.Physics.sleepThreshold;
			}
			if (((Vector2)physicsBody.LinearVelocity).sqrMagnitude > Physics2D.linearSleepTolerance * Physics2D.linearSleepTolerance)
			{
				return false;
			}
			float z = physicsBody.AngularVelocity.z;
			return z * z <= Physics2D.angularSleepTolerance * Physics2D.angularSleepTolerance;
		}

		private bool IsStateBelowSleepingThresholds(AbstractPhysicsBody physicsBody, NetworkPhysicsData data)
		{
			if (Is3D)
			{
				float mass = physicsBody.Mass;
				float num = mass * ((Vector3)data.LinearVelocity).sqrMagnitude;
				Vector3 vector = data.AngularVelocity;
				Vector3 inertiaTensor = _physicsBody.InertiaTensor;
				return (num + inertiaTensor.x * (vector.x * vector.x) + inertiaTensor.y * (vector.y * vector.y) + inertiaTensor.z * (vector.z * vector.z)) / (2f * mass) <= UnityEngine.Physics.sleepThreshold;
			}
			if (((Vector2)data.LinearVelocity).sqrMagnitude > Physics2D.linearSleepTolerance * Physics2D.linearSleepTolerance)
			{
				return false;
			}
			float z = data.AngularVelocity.Z;
			return z * z <= Physics2D.angularSleepTolerance * Physics2D.angularSleepTolerance;
		}

		public override void Render()
		{
			if (_doNotInterpolate || base.Object.RenderSource == RenderSource.Latest)
			{
				return;
			}
			Transform interpolationTarget = _interpolationTarget;
			bool flag = interpolationTarget;
			if (TryGetSnapshotsBuffers(out var from, out var to, out var alpha))
			{
				NetworkTRSPData networkTRSPData = from.ReinterpretState<NetworkTRSPData>();
				NetworkTRSPData networkTRSPData2 = to.ReinterpretState<NetworkTRSPData>();
				int teleportKey = networkTRSPData.TeleportKey;
				int teleportKey2 = networkTRSPData2.TeleportKey;
				bool syncScale = SyncScale;
				Vector3 position = networkTRSPData.Position;
				Quaternion rotation = networkTRSPData.Rotation;
				Vector3 position2 = networkTRSPData2.Position;
				Quaternion rotation2 = networkTRSPData2.Rotation;
				bool syncParent = SyncParent;
				bool num = teleportKey != teleportKey2;
				bool worldSpace = !SyncParent;
				if (num)
				{
					networkTRSPData2 = networkTRSPData;
				}
				if (syncParent)
				{
					Transform parent = _transform.parent;
					if (networkTRSPData.Parent != default(NetworkBehaviourId))
					{
						bool flag2 = networkTRSPData.Parent == NetworkTRSPData.NonNetworkedParent;
						worldSpace = flag2;
						if (base.Runner.TryFindBehaviour(networkTRSPData.Parent, out var behaviour))
						{
							Transform transform = behaviour.transform;
							if (parent != transform)
							{
								_transform.SetParent(transform);
								_rootIsDirtyFromInterpolation = true;
							}
						}
						else if (!flag2)
						{
							Debug.LogError($"Parent of this object is not present {networkTRSPData.Parent} {networkTRSPData.Parent.Behaviour}.");
							return;
						}
						if (networkTRSPData.Parent != networkTRSPData2.Parent)
						{
							if (flag)
							{
								SetPosRotToTransform(interpolationTarget, position2, rotation2, worldSpace: true);
								_targIsDirtyFromInterpolation = true;
							}
							else
							{
								SetPosRotToTransform(_transform, position2, rotation2, worldSpace);
								_rootIsDirtyFromInterpolation = true;
							}
							if (syncScale)
							{
								_transform.localScale = networkTRSPData2.Scale;
							}
							return;
						}
					}
					else
					{
						if (parent != null)
						{
							_transform.SetParent(null);
							_rootIsDirtyFromInterpolation = true;
						}
						if (networkTRSPData.Parent != networkTRSPData2.Parent)
						{
							if (flag)
							{
								SetPosRotToTransform(interpolationTarget, position2, rotation2, worldSpace: true);
								_targIsDirtyFromInterpolation = true;
							}
							else
							{
								SetPosRotToTransform(_transform, position2, rotation2, worldSpace);
								_rootIsDirtyFromInterpolation = true;
							}
							if (syncScale)
							{
								_transform.localScale = networkTRSPData2.Scale;
							}
							return;
						}
					}
				}
				if (IsStateBelowSleepingThresholds(_physicsBody, _physicsData))
				{
					return;
				}
				Vector3 pos = Vector3.Lerp(position, position2, alpha);
				Quaternion rot = Quaternion.Slerp(rotation, rotation2, alpha);
				if (flag)
				{
					SetPosRotToTransform(interpolationTarget, pos, rot, worldSpace: true);
					if (syncScale)
					{
						Vector3 localScale = Vector3.Lerp(networkTRSPData.Scale, networkTRSPData2.Scale, alpha);
						interpolationTarget.localScale = localScale;
					}
					_targIsDirtyFromInterpolation = true;
				}
				else
				{
					Vector3 localScale2 = (syncScale ? Vector3.Lerp(networkTRSPData.Scale, networkTRSPData2.Scale, alpha) : default(Vector3));
					SetPosRotToTransform(_transform, pos, rot, worldSpace);
					if (syncScale)
					{
						base.transform.localScale = localScale2;
					}
					_rootIsDirtyFromInterpolation = true;
				}
			}
			else
			{
				Debug.LogWarning("No interpolation data");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetPosRotToTransform(Transform t, Vector3 pos, Quaternion rot, bool worldSpace)
		{
			if (worldSpace)
			{
				t.position = pos;
				t.rotation = rot;
			}
			else
			{
				t.localPosition = pos;
				t.localRotation = rot;
			}
		}
	}
}

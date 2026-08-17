using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Object.Prediction.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using UnityEngine;

namespace FishNet.Example.Prediction.Rigidbodies
{
	public class RigidbodyPrediction : NetworkBehaviour
	{
		public struct MoveData : IReplicateData
		{
			public bool Jump;

			public float Horizontal;

			public float Vertical;

			private uint _tick;

			public MoveData(bool jump, float horizontal, float vertical)
			{
				Jump = jump;
				Horizontal = horizontal;
				Vertical = vertical;
				_tick = 0u;
			}

			public void Dispose()
			{
			}

			public uint GetTick()
			{
				return _tick;
			}

			public void SetTick(uint value)
			{
				_tick = value;
			}
		}

		public struct ReconcileData : IReconcileData
		{
			public Vector3 Position;

			public Quaternion Rotation;

			public Vector3 Velocity;

			public Vector3 AngularVelocity;

			private uint _tick;

			public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
			{
				Position = position;
				Rotation = rotation;
				Velocity = velocity;
				AngularVelocity = angularVelocity;
				_tick = 0u;
			}

			public void Dispose()
			{
			}

			public uint GetTick()
			{
				return _tick;
			}

			public void SetTick(uint value)
			{
				_tick = value;
			}
		}

		[SerializeField]
		private float _jumpForce = 15f;

		[SerializeField]
		private float _moveRate = 15f;

		private Rigidbody _rigidbody;

		private float _nextJumpTime;

		private bool _jump;

		public NetworkObject BulletPrefab;

		private bool _spawnBullet;

		private bool _despawnBullet;

		private NetworkObject _lastSpawnedBullet;

		private ReplicateUserLogicDelegate<MoveData> _replicateULDelegate___Move;

		private ReconcileUserLogicDelegate<ReconcileData> _reconcileULDelegate___Reconciliation;

		private BasicQueue<MoveData> _serverReplicates___Move;

		private List<MoveData> _clientReplicates___Move;

		private ReconcileData _reconcileData___Move;

		private MoveData[] Move___serverReplicateReadBuffer;

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPredictionFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPredictionFishNet_002EDemos_002Edll_Excuted;

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			Awake_UserLogic_FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_FishNet_002EDemos_002Edll();
			NetworkInitialize__Late();
		}

		private void OnDestroy()
		{
			if (InstanceFinder.TimeManager != null)
			{
				InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
				InstanceFinder.TimeManager.OnPostTick -= TimeManager_OnPostTick;
			}
		}

		public override void OnStartClient()
		{
			base.PredictionManager.OnPreReplicateReplay += PredictionManager_OnPreReplicateReplay;
		}

		public override void OnStopClient()
		{
			base.PredictionManager.OnPreReplicateReplay -= PredictionManager_OnPreReplicateReplay;
		}

		private void Update()
		{
			if (base.IsOwner)
			{
				if (Input.GetKeyDown(KeyCode.RightAlt))
				{
					_rigidbody.velocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
				}
				if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextJumpTime)
				{
					_nextJumpTime = Time.time + 1f;
					_jump = true;
				}
				else if (Input.GetKeyDown(KeyCode.LeftShift))
				{
					_spawnBullet = true;
				}
			}
		}

		private void PredictionManager_OnPreReplicateReplay(uint arg1, PhysicsScene arg2, PhysicsScene2D arg3)
		{
			if (!base.IsServer)
			{
				AddGravity();
			}
		}

		private void TimeManager_OnTick()
		{
			if (base.IsOwner)
			{
				Reconciliation(default(ReconcileData), asServer: false);
				BuildMoveData(out var md);
				Move(md, asServer: false);
				TryDespawnBullet();
				TrySpawnBullet();
			}
			if (base.IsServer)
			{
				Move(default(MoveData), asServer: true);
			}
			AddGravity();
		}

		private void TimeManager_OnPostTick()
		{
			if (base.IsServer)
			{
				ReconcileData rd = new ReconcileData(base.transform.position, base.transform.rotation, _rigidbody.velocity, _rigidbody.angularVelocity);
				Reconciliation(rd, asServer: true);
			}
		}

		private void BuildMoveData(out MoveData md)
		{
			md = default(MoveData);
			float axisRaw = Input.GetAxisRaw("Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Vertical");
			if (axisRaw != 0f || axisRaw2 != 0f || _jump)
			{
				md = new MoveData(_jump, axisRaw, axisRaw2);
				_jump = false;
			}
		}

		private void TrySpawnBullet()
		{
			if (_spawnBullet)
			{
				_spawnBullet = false;
				NetworkObject networkObject = (_lastSpawnedBullet = UnityEngine.Object.Instantiate(BulletPrefab, base.transform.position + base.transform.forward * 1f, base.transform.rotation));
				PredictedBullet component = networkObject.GetComponent<PredictedBullet>();
				Vector3 vector = base.transform.forward * 20f;
				component.SetStartingForce(vector);
				component.SetVelocity(vector);
				Spawn(networkObject, base.Owner);
			}
		}

		private void TryDespawnBullet()
		{
			if (_despawnBullet)
			{
				_despawnBullet = false;
				_lastSpawnedBullet?.Despawn();
			}
		}

		private void AddGravity()
		{
			_rigidbody.AddForce(Physics.gravity * 2f);
		}

		[Replicate]
		private void Move(MoveData md, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
		{
			if (!Replicate_ExitEarly_A(asServer, replaying, allowServerControl: false))
			{
				if (asServer)
				{
					Replicate_NonOwner(_replicateULDelegate___Move, _serverReplicates___Move, md, allowServerControl: false, channel);
				}
				else
				{
					Replicate_Owner(_replicateULDelegate___Move, 0u, _clientReplicates___Move, md, channel);
				}
			}
		}

		[Reconcile]
		private void Reconciliation(ReconcileData rd, bool asServer, Channel channel = Channel.Unreliable)
		{
			if (!Reconcile_ExitEarly_A(asServer, out channel))
			{
				if (asServer)
				{
					Reconcile_Server(0u, rd, channel);
				}
				else
				{
					Reconcile_Client(_reconcileULDelegate___Reconciliation, _replicateULDelegate___Move, _clientReplicates___Move, _reconcileData___Move, channel);
				}
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPredictionFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPredictionFishNet_002EDemos_002Edll_Excuted = true;
				_reconcileULDelegate___Reconciliation = Reconciliation___UL;
				_replicateULDelegate___Move = Move___UL;
				_serverReplicates___Move = new BasicQueue<MoveData>();
				_clientReplicates___Move = new List<MoveData>();
				RegisterReplicateRpc(0u, Reader_Replicate___Move);
				RegisterReconcileRpc(0u, Reader_Reconcile___Reconciliation);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPredictionFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPredictionFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void Move___UL(MoveData md, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
		{
			Vector3 force = new Vector3(md.Horizontal, 0f, md.Vertical) * _moveRate;
			_rigidbody.AddForce(force);
			if (md.Jump)
			{
				_rigidbody.AddForce(new Vector3(0f, _jumpForce, 0f), ForceMode.Impulse);
			}
		}

		private void Reconciliation___UL(ReconcileData rd, bool asServer, Channel channel = Channel.Unreliable)
		{
			base.transform.position = rd.Position;
			base.transform.rotation = rd.Rotation;
			_rigidbody.velocity = rd.Velocity;
			_rigidbody.angularVelocity = rd.AngularVelocity;
		}

		public override void ClearReplicateCache_Virtual(bool asServer)
		{
			if (asServer)
			{
				_serverReplicates___Move.Clear();
			}
			else
			{
				_clientReplicates___Move.Clear();
			}
		}

		private void Reader_Replicate___Move(PooledReader PooledReader0, NetworkConnection NetworkConnection1, Channel Channel2)
		{
			Replicate_Reader(PooledReader0, NetworkConnection1, Move___serverReplicateReadBuffer, _serverReplicates___Move, Channel2);
		}

		private void Reader_Reconcile___Reconciliation(PooledReader PooledReader0, Channel Channel1)
		{
			Reconcile_Reader(PooledReader0, ref _reconcileData___Move, Channel1);
		}

		private void Awake_UserLogic_FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_FishNet_002EDemos_002Edll()
		{
			_rigidbody = GetComponent<Rigidbody>();
			InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
			InstanceFinder.TimeManager.OnPostTick += TimeManager_OnPostTick;
		}
	}
}

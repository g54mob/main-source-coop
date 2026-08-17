using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using NomadDrive.Features.Rope;
using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[DefaultExecutionOrder(10000)]
	public class GasPumpBody : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private Transform nozzleRestPoint;

		[SerializeField]
		private NomadDrive.Features.Rope.Rope hoseRope;

		[SerializeField]
		private Transform ropeEndPoint;

		[SerializeField]
		private GameObject nozzlePrefab;

		[Header("Rope Stretch (dynamic length)")]
		[Tooltip("Slack rope length used when the nozzle rests near the pump. Higher = more droop/coil at rest.")]
		[SerializeField]
		private float ropeRestLength = 2.3f;

		[Tooltip("Extra slack kept once the nozzle is pulled past the rest length. Small value = taut hose when extended.")]
		[SerializeField]
		private float ropeExtendSlack = 0.3f;

		private const float RestPositionMatchEpsilon = 0.1f;

		private GasPumpNozzle _spawnedNozzle;

		private MeshRenderer _ropeRenderer;

		private bool _ropeConnected;

		private bool _subscribed;

		private Transform _ropeStartTransform;

		public Transform NozzleRestPoint => nozzleRestPoint;

		private void Awake()
		{
			if (hoseRope != null)
			{
				_ropeRenderer = hoseRope.GetComponent<MeshRenderer>();
				if (_ropeRenderer != null)
				{
					_ropeRenderer.enabled = false;
				}
			}
		}

		private void Start()
		{
			InitializeAsync().Forget();
		}

		private void LateUpdate()
		{
			if (!_ropeConnected || hoseRope == null)
			{
				return;
			}
			if (_ropeStartTransform == null)
			{
				HandleNozzleLost();
				return;
			}
			if (_ropeStartTransform != null && ropeEndPoint != null)
			{
				float num = Vector3.Distance(_ropeStartTransform.position, ropeEndPoint.position);
				hoseRope.SetLength(Mathf.Max(ropeRestLength, num + ropeExtendSlack));
			}
			hoseRope.RopeUpdate();
		}

		private async UniTaskVoid InitializeAsync()
		{
			if (nozzleRestPoint == null || nozzlePrefab == null)
			{
				EvilLogger.LogError("[GasPumpBody] " + base.name + " is missing nozzleRestPoint or nozzlePrefab.", "InitializeAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\LiquidTransferSystem\\Scripts\\GasPumpBody.cs", 85);
				return;
			}
			await UniTask.WaitUntil(() => NetworkServer.active || NetworkClient.active, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			if (NetworkServer.active)
			{
				SpawnNozzle();
			}
			SubscribeToNozzleRegistry();
			TryBindFromRegistry();
		}

		private void SpawnNozzle()
		{
			GameObject obj = Object.Instantiate(nozzlePrefab, nozzleRestPoint.position, nozzleRestPoint.rotation);
			NetworkServer.Spawn(obj);
			if (obj.TryGetComponent<GasPumpNozzle>(out var component))
			{
				_spawnedNozzle = component;
				component.SetPumpBody(this);
			}
		}

		private void SubscribeToNozzleRegistry()
		{
			if (!_subscribed)
			{
				_subscribed = true;
				GasPumpNozzle.OnNozzleRegistered += OnNozzleRegistered;
			}
		}

		private void UnsubscribeFromNozzleRegistry()
		{
			if (_subscribed)
			{
				_subscribed = false;
				GasPumpNozzle.OnNozzleRegistered -= OnNozzleRegistered;
			}
		}

		private void OnNozzleRegistered(GasPumpNozzle nozzle)
		{
			TryBindNozzle(nozzle);
		}

		private void TryBindFromRegistry()
		{
			TryBindNozzle(FindMatchingNozzle());
		}

		private void TryBindNozzle(GasPumpNozzle nozzle)
		{
			if (!_ropeConnected && MatchesRestPoint(nozzle))
			{
				ConnectRopeToNozzle(nozzle.RopeStartPoint);
				UnsubscribeFromNozzleRegistry();
			}
		}

		private void HandleNozzleLost()
		{
			_ropeConnected = false;
			_ropeStartTransform = null;
			if (_ropeRenderer != null)
			{
				_ropeRenderer.enabled = false;
			}
			SubscribeToNozzleRegistry();
			TryBindFromRegistry();
		}

		private GasPumpNozzle FindMatchingNozzle()
		{
			IReadOnlyList<GasPumpNozzle> registry = GasPumpNozzle.Registry;
			float num = 0.010000001f;
			GasPumpNozzle result = null;
			for (int i = 0; i < registry.Count; i++)
			{
				GasPumpNozzle gasPumpNozzle = registry[i];
				if (!(gasPumpNozzle == null))
				{
					float sqrMagnitude = (gasPumpNozzle.PumpRestPosition - nozzleRestPoint.position).sqrMagnitude;
					if (sqrMagnitude <= num)
					{
						num = sqrMagnitude;
						result = gasPumpNozzle;
					}
				}
			}
			return result;
		}

		private bool MatchesRestPoint(GasPumpNozzle nozzle)
		{
			if (nozzle == null)
			{
				return false;
			}
			return (nozzle.PumpRestPosition - nozzleRestPoint.position).sqrMagnitude <= 0.010000001f;
		}

		public void ConnectRopeToNozzle(Transform nozzleRopeStart)
		{
			if (!(hoseRope == null) && !(nozzleRopeStart == null))
			{
				hoseRope.SetStartAttach(nozzleRopeStart);
				_ropeStartTransform = nozzleRopeStart;
				if (ropeEndPoint != null)
				{
					hoseRope.SetEndAttach(ropeEndPoint);
				}
				hoseRope.SetDirty();
				_ropeConnected = true;
				if (_ropeRenderer != null)
				{
					_ropeRenderer.enabled = true;
				}
			}
		}

		private void OnDestroy()
		{
			UnsubscribeFromNozzleRegistry();
			if (NetworkServer.active && _spawnedNozzle != null)
			{
				NetworkServer.Destroy(_spawnedNozzle.gameObject);
			}
		}
	}
}

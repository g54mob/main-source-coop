using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NomadDrive.Features.EvilRoads.Cable;
using NomadDrive.Features.FloatingOrigin;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.EvilRoads
{
	public class EvilRoadsManager : MonoBehaviour, IFloatingOriginShiftable
	{
		[SerializeField]
		private Transform roadsContainer;

		[SerializeField]
		private RoadObjectConfig lightPoleConfig;

		[SerializeField]
		private EvilRoadConfig roadConfig;

		[Tooltip("Optional separate config for branch spurs (dirt material, narrower width). Null falls back to roadConfig.")]
		[SerializeField]
		private EvilRoadConfig branchRoadConfig;

		[Tooltip("Optional roadside objects for branch spurs (their own poles). Null = no side objects on branches.")]
		[SerializeField]
		private RoadObjectConfig branchLightPoleConfig;

		[Tooltip("Optional random scattered signs for main roads (RandomScattered distribution mode). Null = no signs.")]
		[SerializeField]
		private RoadObjectConfig roadSignConfig;

		[Tooltip("Optional random scattered signs for branch spurs. Null = no signs on branches.")]
		[SerializeField]
		private RoadObjectConfig branchRoadSignConfig;

		[FormerlySerializedAs("divisionRoads")]
		[SerializeField]
		private List<EvilRoad> evilRoads = new List<EvilRoad>();

		[Header("Connection System")]
		[SerializeField]
		private RoadConnectionManager connectionManager;

		[Inject]
		private ICableManager _cableManager;

		private void Awake()
		{
			if (connectionManager == null)
			{
				GameObject gameObject = new GameObject("RoadConnectionManager");
				gameObject.transform.SetParent(base.transform);
				connectionManager = gameObject.AddComponent<RoadConnectionManager>();
			}
			if (connectionManager != null && roadConfig != null)
			{
				connectionManager.SetConnectionDistance(roadConfig.connectionDistance);
			}
		}

		private void OnEnable()
		{
			FloatingOriginManager.RegisterShiftable(this);
		}

		private void OnDisable()
		{
			FloatingOriginManager.UnregisterShiftable(this);
		}

		public void OnOriginShift(Vector3 delta)
		{
			if (roadsContainer != null)
			{
				roadsContainer.position += delta;
			}
		}

		private void ClearAllRoads()
		{
			foreach (EvilRoad evilRoad in evilRoads)
			{
				Object.DestroyImmediate(evilRoad.gameObject);
			}
			evilRoads.Clear();
		}

		public EvilRoad CreateRoad(Vector3[] points)
		{
			EvilRoad evilRoad = new GameObject("Road").AddComponent<EvilRoad>();
			evilRoad.transform.SetParent(roadsContainer);
			evilRoad.Setup(roadConfig, this, _cableManager);
			if (connectionManager != null)
			{
				evilRoad.SetConnectionManager(connectionManager);
			}
			evilRoad.Create(points);
			evilRoads.Add(evilRoad);
			ActivateRoadDecorations(evilRoad, lightPoleConfig, isBranch: false);
			return evilRoad;
		}

		public EvilRoad CreateRoad(Vector3[] points, Terrain terrain)
		{
			EvilRoad evilRoad = new GameObject((terrain != null) ? "Road_Snapped" : "Road").AddComponent<EvilRoad>();
			evilRoad.transform.SetParent(roadsContainer);
			evilRoad.Setup(roadConfig, this, _cableManager);
			if (connectionManager != null)
			{
				evilRoad.SetConnectionManager(connectionManager);
			}
			evilRoad.Create(points, terrain);
			evilRoads.Add(evilRoad);
			ActivateRoadDecorations(evilRoad, lightPoleConfig, isBranch: false);
			return evilRoad;
		}

		public async UniTask<EvilRoad> CreateRoadAsync(Vector3[] points, EvilRoadConfig configOverride = null, RoadObjectConfig roadObjectOverride = null, bool overrideRoadObjects = false, RoadBoundaryTangents boundary = default(RoadBoundaryTangents))
		{
			EvilRoad evilRoad = new GameObject("Road").AddComponent<EvilRoad>();
			evilRoad.transform.SetParent(roadsContainer);
			evilRoad.Setup((configOverride != null) ? configOverride : roadConfig, this, _cableManager);
			evilRoad.SetBoundaryTangents(boundary);
			if (connectionManager != null)
			{
				evilRoad.SetConnectionManager(connectionManager);
			}
			await evilRoad.CreateAsync(points);
			if (evilRoad == null)
			{
				return null;
			}
			evilRoads.Add(evilRoad);
			await UniTask.Yield();
			if (evilRoad == null)
			{
				return null;
			}
			await ActivateRoadDecorationsAsync(evilRoad, overrideRoadObjects ? roadObjectOverride : lightPoleConfig, overrideRoadObjects);
			return evilRoad;
		}

		public async UniTask<EvilRoad> CreateRoadAsync(Vector3[] points, Terrain terrain, EvilRoadConfig configOverride = null, RoadObjectConfig roadObjectOverride = null, bool overrideRoadObjects = false, RoadBoundaryTangents boundary = default(RoadBoundaryTangents))
		{
			string text = ((terrain != null) ? "Road_Snapped" : "Road");
			EvilRoad evilRoad = new GameObject(text).AddComponent<EvilRoad>();
			evilRoad.transform.SetParent(roadsContainer);
			evilRoad.Setup((configOverride != null) ? configOverride : roadConfig, this, _cableManager);
			evilRoad.SetBoundaryTangents(boundary);
			if (connectionManager != null)
			{
				evilRoad.SetConnectionManager(connectionManager);
			}
			RoadObjectConfig poleConfig = (overrideRoadObjects ? roadObjectOverride : lightPoleConfig);
			await RoadBuildScheduler.Enqueue(async delegate
			{
				if (!(evilRoad == null))
				{
					await evilRoad.CreateAsync(points, terrain, !overrideRoadObjects);
					if (!(evilRoad == null))
					{
						evilRoads.Add(evilRoad);
						await UniTask.Yield();
						if (!(evilRoad == null))
						{
							await ActivateRoadDecorationsAsync(evilRoad, poleConfig, overrideRoadObjects);
						}
					}
				}
			});
			if (evilRoad == null)
			{
				return null;
			}
			return evilRoad;
		}

		public UniTask<EvilRoad> CreateBranchRoadAsync(Vector3[] points, Terrain terrain)
		{
			return CreateRoadAsync(points, terrain, (branchRoadConfig != null) ? branchRoadConfig : roadConfig, branchLightPoleConfig, overrideRoadObjects: true);
		}

		private void ActivateRoadDecorations(EvilRoad road, RoadObjectConfig poleConfig, bool isBranch)
		{
			if (!(road == null))
			{
				if (poleConfig != null)
				{
					road.ActivateRoadObject(poleConfig);
				}
				RoadObjectConfig roadObjectConfig = (isBranch ? branchRoadSignConfig : roadSignConfig);
				if (roadObjectConfig != null)
				{
					road.ActivateRoadObject(roadObjectConfig);
				}
			}
		}

		private async UniTask ActivateRoadDecorationsAsync(EvilRoad road, RoadObjectConfig poleConfig, bool isBranch)
		{
			if (road == null)
			{
				return;
			}
			if (poleConfig != null)
			{
				await road.ActivateRoadObjectAsync(poleConfig);
			}
			if (!(road == null))
			{
				RoadObjectConfig roadObjectConfig = (isBranch ? branchRoadSignConfig : roadSignConfig);
				if (roadObjectConfig != null)
				{
					await road.ActivateRoadObjectAsync(roadObjectConfig);
				}
			}
		}

		public void DestroyRoad(EvilRoad evilRoad)
		{
			if (evilRoads.Contains(evilRoad))
			{
				evilRoads.Remove(evilRoad);
				Object.DestroyImmediate(evilRoad.gameObject);
			}
		}

		public RoadConnectionManager GetConnectionManager()
		{
			return connectionManager;
		}

		public void SetConnectionDistance(float distance)
		{
			if (connectionManager != null)
			{
				connectionManager.SetConnectionDistance(distance);
			}
		}

		public bool IsLazyLoadingEnabled()
		{
			if (roadConfig != null)
			{
				return roadConfig.enableLazyLoading;
			}
			return false;
		}

		public string GetLazyLoadingReferenceTag()
		{
			if (!(roadConfig != null))
			{
				return "Player";
			}
			return roadConfig.lazyLoadingReferenceTag;
		}

		public float GetLazyLoadingDistance()
		{
			if (!(roadConfig != null))
			{
				return 500f;
			}
			return roadConfig.lazyLoadingDistance;
		}

		public bool IsDynamicUpgradeEnabled()
		{
			if (roadConfig != null)
			{
				return roadConfig.enableDynamicUpgrade;
			}
			return false;
		}

		public float GetUpgradeCheckInterval()
		{
			if (!(roadConfig != null))
			{
				return 2f;
			}
			return roadConfig.upgradeCheckInterval;
		}

		public float GetUpgradeDistanceThreshold()
		{
			if (!(roadConfig != null))
			{
				return 0.8f;
			}
			return roadConfig.upgradeDistanceThreshold;
		}

		public float GetRoadWidth()
		{
			if (!(roadConfig != null))
			{
				return 10f;
			}
			return roadConfig.roadWidth;
		}
	}
}

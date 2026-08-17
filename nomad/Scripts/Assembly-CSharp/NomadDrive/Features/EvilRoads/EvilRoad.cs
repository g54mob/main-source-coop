using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.EvilRoads.Cable;
using NomadDrive.Features.Player;
using NomadDrive.Features.WorldGeneration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Splines;

namespace NomadDrive.Features.EvilRoads
{
	public class EvilRoad : MonoBehaviour
	{
		public UnityEvent<EvilRoad> onRoadCreated;

		public UnityEvent<EvilRoad> onRoadDestroyed;

		private SplineContainer _splineContainer;

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private EvilRoadConfig _roadConfig;

		private EvilRoadsManager _divisionRoadsManager;

		private Terrain _associatedTerrain;

		private ICableManager _cableManager;

		private RoadMeshGenerator _meshGenerator;

		private RoadTerrainDeformer _terrainDeformer;

		private RoadVegetationClearer _vegetationClearer;

		private RoadObjectPlacer _objectPlacer;

		private Dictionary<string, List<GameObject>> _placedRoadObjects = new Dictionary<string, List<GameObject>>();

		private RoadConnectionManager _connectionManager;

		private bool _isConnected;

		private bool _isActive = true;

		private RoadBoundaryTangents _boundaryTangents;

		private void Awake()
		{
			InitializeComponents();
		}

		private void InitializeComponents()
		{
			if (!TryGetComponent<MeshFilter>(out _meshFilter))
			{
				_meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (!TryGetComponent<MeshRenderer>(out _meshRenderer))
			{
				_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			if (!TryGetComponent<SplineContainer>(out _splineContainer))
			{
				_splineContainer = base.gameObject.AddComponent<SplineContainer>();
			}
			if (!TryGetComponent<SurfaceTypeComponent>(out var component))
			{
				component = base.gameObject.AddComponent<SurfaceTypeComponent>();
				component.surfaceType = SurfaceType.Concrete;
			}
		}

		public void Setup(EvilRoadConfig roadConfig, EvilRoadsManager divisionRoadsManager, ICableManager cableManager = null)
		{
			_divisionRoadsManager = divisionRoadsManager;
			_cableManager = cableManager;
			SetRoadConfig(roadConfig);
			_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			_meshRenderer.material = roadConfig.roadMaterial;
			WorldGenerationTagUtility.ApplyTagSafe(base.gameObject, (roadConfig != null) ? roadConfig.roadTag : null);
			InitializeModules();
		}

		private void SetRoadConfig(EvilRoadConfig roadConfig)
		{
			_roadConfig = roadConfig;
		}

		public void SetBoundaryTangents(RoadBoundaryTangents boundaryTangents)
		{
			_boundaryTangents = boundaryTangents;
		}

		private void InitializeModules()
		{
			_meshGenerator = new RoadMeshGenerator(_roadConfig, _associatedTerrain);
			if (_associatedTerrain != null)
			{
				_terrainDeformer = new RoadTerrainDeformer(_roadConfig, _associatedTerrain);
				_vegetationClearer = new RoadVegetationClearer(_roadConfig, _associatedTerrain);
			}
			_objectPlacer = new RoadObjectPlacer(_roadConfig, _associatedTerrain, _cableManager);
		}

		public EvilRoad Create(Vector3[] points)
		{
			if (points.Length < 2)
			{
				return null;
			}
			CreateSplineFromPoints(points);
			GenerateRoadMesh();
			onRoadCreated?.Invoke(this);
			RegisterForManualConnection();
			return this;
		}

		public async UniTask<EvilRoad> CreateAsync(Vector3[] points)
		{
			if (points.Length < 2)
			{
				return null;
			}
			CreateSplineFromPoints(points);
			await UniTask.Yield();
			GenerateRoadMesh();
			await UniTask.Yield();
			onRoadCreated?.Invoke(this);
			RegisterForManualConnection();
			return this;
		}

		public async UniTask<EvilRoad> CreateAsync(Vector3[] points, Terrain terrain, bool deferTreeRebuild = true)
		{
			if (points.Length < 2)
			{
				return null;
			}
			if (terrain == null)
			{
				return await CreateAsync(points);
			}
			_ = Time.realtimeSinceStartup;
			_associatedTerrain = terrain;
			InitializeModules();
			CreateSplineFromPoints(points);
			await UniTask.Yield();
			if (_vegetationClearer != null)
			{
				await _vegetationClearer.ClearVegetationAroundRoadAsync(_splineContainer.Spline, deferTreeRebuild);
			}
			_ = Time.realtimeSinceStartup;
			GenerateRoadMesh();
			_ = Time.realtimeSinceStartup;
			await UniTask.Yield();
			if (_terrainDeformer != null && _meshFilter != null && _meshFilter.sharedMesh != null)
			{
				await _terrainDeformer.DeformTerrainToRoadMeshAsync(_meshFilter.sharedMesh, base.transform);
			}
			_ = Time.realtimeSinceStartup;
			onRoadCreated?.Invoke(this);
			RegisterForManualConnection();
			return this;
		}

		public EvilRoad Create(Vector3[] points, Terrain terrain)
		{
			if (points.Length < 2)
			{
				return null;
			}
			if (terrain == null)
			{
				return Create(points);
			}
			_associatedTerrain = terrain;
			InitializeModules();
			CreateSplineFromPoints(points);
			if (_vegetationClearer != null)
			{
				_vegetationClearer.ClearVegetationAroundRoad(_splineContainer.Spline);
			}
			GenerateRoadMesh();
			if (_terrainDeformer != null && _meshFilter != null && _meshFilter.sharedMesh != null)
			{
				_terrainDeformer.DeformTerrainToRoadMesh(_meshFilter.sharedMesh, base.transform);
			}
			onRoadCreated?.Invoke(this);
			RegisterForManualConnection();
			return this;
		}

		private void CreateSplineFromPoints(Vector3[] points)
		{
			if (_splineContainer == null)
			{
				EvilLogger.LogError("SplineContainer is null!", "CreateSplineFromPoints", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\EvilRoad.cs", 261);
				return;
			}
			if (_splineContainer.Spline == null)
			{
				_splineContainer.Spline = new Spline();
			}
			_splineContainer.Spline.Clear();
			List<Vector3> list = ValidatePoints(points);
			if (list.Count < 2)
			{
				EvilLogger.LogError("Not enough valid points for spline creation!", "CreateSplineFromPoints", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\EvilRoad.cs", 274);
				return;
			}
			CreateBezierSpline(list);
			_splineContainer.Spline.Closed = false;
		}

		private List<Vector3> ValidatePoints(Vector3[] points)
		{
			List<Vector3> list = new List<Vector3>();
			foreach (Vector3 vector in points)
			{
				if (!RoadHeightCalculator.HasNaN(vector) && (list.Count <= 0 || !(Vector3.Distance(list[list.Count - 1], vector) < 0.1f)))
				{
					list.Add(vector);
				}
			}
			return list;
		}

		private void CreateBezierSpline(List<Vector3> points)
		{
			for (int i = 0; i < points.Count; i++)
			{
				Vector3 vector = points[i];
				Vector3 tangentIn = Vector3.zero;
				Vector3 tangentOut = Vector3.zero;
				CalculateTangents(points, i, out tangentIn, out tangentOut);
				BezierKnot item = new BezierKnot(vector, tangentIn, tangentOut);
				_splineContainer.Spline.Add(item);
			}
		}

		private void CalculateTangents(List<Vector3> points, int index, out Vector3 tangentIn, out Vector3 tangentOut)
		{
			tangentIn = Vector3.zero;
			tangentOut = Vector3.zero;
			if (index == 0 && points.Count > 1)
			{
				Vector3 normalized = (points[1] - points[0]).normalized;
				float num = Vector3.Distance(points[0], points[1]) * 0.3f;
				if (_boundaryTangents.HasValue)
				{
					normalized = new Vector3(_boundaryTangents.StartDir.x, normalized.y, _boundaryTangents.StartDir.z).normalized;
				}
				tangentOut = normalized * num;
			}
			else if (index == points.Count - 1 && points.Count > 1)
			{
				Vector3 normalized2 = (points[index] - points[index - 1]).normalized;
				float num2 = Vector3.Distance(points[index - 1], points[index]) * 0.3f;
				if (_boundaryTangents.HasValue)
				{
					normalized2 = new Vector3(_boundaryTangents.EndDir.x, normalized2.y, _boundaryTangents.EndDir.z).normalized;
				}
				tangentIn = -normalized2 * num2;
			}
			else if (index > 0 && index < points.Count - 1)
			{
				Vector3 normalized3 = (points[index] - points[index - 1]).normalized;
				Vector3 normalized4 = (points[index + 1] - points[index]).normalized;
				Vector3 normalized5 = (normalized3 + normalized4).normalized;
				float num3 = Vector3.Distance(points[index - 1], points[index]) * 0.3f;
				float num4 = Vector3.Distance(points[index], points[index + 1]) * 0.3f;
				tangentIn = -normalized5 * num3;
				tangentOut = normalized5 * num4;
			}
		}

		private void GenerateRoadMesh()
		{
			if (_splineContainer?.Spline == null || _meshGenerator == null)
			{
				EvilLogger.LogError("Cannot generate mesh: missing spline or mesh generator", "GenerateRoadMesh", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\EvilRoad.cs", 376);
				return;
			}
			Mesh mesh = _meshGenerator.GenerateRoadMesh(_splineContainer.Spline, GetInstanceID());
			if (mesh != null)
			{
				_meshFilter.mesh = mesh;
				_meshGenerator.UpdateMaterialTiling(_meshRenderer);
				SetupMeshCollider(mesh);
			}
		}

		private void SetupMeshCollider(Mesh roadMesh)
		{
			MeshCollider meshCollider = GetComponent<MeshCollider>();
			if (meshCollider == null)
			{
				meshCollider = base.gameObject.AddComponent<MeshCollider>();
			}
			meshCollider.sharedMesh = roadMesh;
			meshCollider.convex = false;
		}

		private void DestroyRoad()
		{
			DeactivateAllRoadObjects();
			onRoadDestroyed?.Invoke(this);
			Object.DestroyImmediate(base.gameObject);
		}

		private void ConnectToNearbyRoadButton()
		{
			if (_connectionManager != null)
			{
				_connectionManager.ConnectSpecificRoad(this);
			}
		}

		public void ActivateRoadObject(RoadObjectConfig config)
		{
			if (config == null || (config.distributionMode != ObjectDistributionMode.RandomScattered && config.roadObject == null) || this == null)
			{
				return;
			}
			DeactivateRoadObject(config);
			if (_objectPlacer == null || _splineContainer?.Spline == null)
			{
				return;
			}
			List<GameObject> list = _objectPlacer.PlaceRoadObjectsForConfig(_splineContainer.Spline, config, base.transform, base.gameObject);
			if (list.Count > 0)
			{
				if (!_placedRoadObjects.ContainsKey(config.name))
				{
					_placedRoadObjects[config.name] = new List<GameObject>();
				}
				_placedRoadObjects[config.name].AddRange(list);
			}
		}

		public async UniTask ActivateRoadObjectAsync(RoadObjectConfig config)
		{
			if (config == null || (config.distributionMode != ObjectDistributionMode.RandomScattered && config.roadObject == null) || this == null)
			{
				return;
			}
			DeactivateRoadObject(config);
			if (_objectPlacer == null || _splineContainer?.Spline == null)
			{
				return;
			}
			List<GameObject> list = await _objectPlacer.PlaceRoadObjectsForConfigAsync(_splineContainer.Spline, config, base.transform, base.gameObject);
			if (!(this == null) && list.Count > 0)
			{
				if (!_placedRoadObjects.ContainsKey(config.name))
				{
					_placedRoadObjects[config.name] = new List<GameObject>();
				}
				_placedRoadObjects[config.name].AddRange(list);
			}
		}

		public void DeactivateRoadObject(RoadObjectConfig config)
		{
			if (!(config == null))
			{
				if (config.enableCableConnections && _cableManager != null)
				{
					_cableManager.UnregisterEdgeObjects(this);
				}
				DeactivateRoadObjectByName(config.name);
			}
		}

		public void DeactivateAllRoadObjects()
		{
			if (_cableManager != null)
			{
				_cableManager.UnregisterEdgeObjects(this);
			}
			foreach (List<GameObject> value in _placedRoadObjects.Values)
			{
				foreach (GameObject item in value)
				{
					if (item != null)
					{
						Object.DestroyImmediate(item);
					}
				}
			}
			_placedRoadObjects.Clear();
		}

		private void DeactivateRoadObjectByName(string configName)
		{
			if (!_placedRoadObjects.TryGetValue(configName, out var value))
			{
				return;
			}
			foreach (GameObject item in value)
			{
				if (item != null)
				{
					Object.DestroyImmediate(item);
				}
			}
			value.Clear();
		}

		public void SetConnectionManager(RoadConnectionManager connectionManager)
		{
			_connectionManager = connectionManager;
		}

		public Spline GetSpline()
		{
			return _splineContainer?.Spline;
		}

		public EvilRoadConfig GetRoadConfig()
		{
			return _roadConfig;
		}

		public Terrain GetAssociatedTerrain()
		{
			return _associatedTerrain;
		}

		public EvilRoad CreateFromSpline(Spline spline, Terrain terrain = null)
		{
			if (spline == null || spline.Count == 0)
			{
				return null;
			}
			_associatedTerrain = terrain;
			InitializeModules();
			if (_splineContainer == null)
			{
				_splineContainer = base.gameObject.AddComponent<SplineContainer>();
			}
			_splineContainer.Spline = spline;
			if (terrain != null && !base.gameObject.name.Contains("MergedRoad") && _vegetationClearer != null)
			{
				_vegetationClearer.ClearVegetationAroundRoad(spline);
			}
			GenerateRoadMesh();
			if (_terrainDeformer != null && _meshFilter != null && _meshFilter.sharedMesh != null && terrain != null && !base.gameObject.name.Contains("MergedRoad"))
			{
				_terrainDeformer.DeformTerrainToRoadMesh(_meshFilter.sharedMesh, base.transform);
			}
			onRoadCreated?.Invoke(this);
			return this;
		}

		public void SetActive(bool active)
		{
			_isActive = active;
			base.gameObject.SetActive(active);
		}

		public bool IsActive()
		{
			return _isActive;
		}

		public void SetConnected(bool connected)
		{
			_isConnected = connected;
		}

		public bool IsConnected()
		{
			return _isConnected;
		}

		public Dictionary<string, List<GameObject>> GetPlacedRoadObjects()
		{
			return _placedRoadObjects;
		}

		public void ClearPlacedRoadObjects()
		{
			_placedRoadObjects.Clear();
		}

		public void UpdatePlacedRoadObjectsFromChildren()
		{
			_placedRoadObjects.Clear();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (child != null && child.gameObject != null)
				{
					string key = child.name.Replace("(Clone)", "").Trim();
					if (!_placedRoadObjects.ContainsKey(key))
					{
						_placedRoadObjects[key] = new List<GameObject>();
					}
					_placedRoadObjects[key].Add(child.gameObject);
				}
			}
		}

		private void RegisterForManualConnection()
		{
			if (_connectionManager != null)
			{
				_connectionManager.RegisterRoad(this);
			}
		}
	}
}

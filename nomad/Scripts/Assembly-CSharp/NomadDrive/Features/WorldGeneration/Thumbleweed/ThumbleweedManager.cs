using System;
using System.Collections;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Player;
using NomadDrive.Features.WorldGeneration.Utilities;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.WorldGeneration.Thumbleweed
{
	public class ThumbleweedManager : MonoBehaviour, IFloatingOriginShiftable
	{
		[Header("Config")]
		[SerializeField]
		private ThumbleweedConfig config;

		[Header("Runtime (debug)")]
		[SerializeField]
		private bool logSpawnEvents;

		[Inject]
		private IWorldGenerator _worldGenerator;

		[Inject]
		private IPlayerService _playerService;

		private readonly Dictionary<Vector2Int, List<Thumbleweed>> _byChunk = new Dictionary<Vector2Int, List<Thumbleweed>>();

		private readonly List<Thumbleweed> _allActive = new List<Thumbleweed>();

		private readonly List<Vector3> _chunkSpawnPositionsBuffer = new List<Vector3>();

		private Transform _cameraTransform;

		private Coroutine _cullRoutine;

		private bool _subscribed;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		private void OnEnable()
		{
			FloatingOriginManager.RegisterShiftable(this);
			if (_worldGenerator == null)
			{
				base.enabled = false;
			}
			else
			{
				if (config == null || !config.HasValidVariants())
				{
					return;
				}
				_worldGenerator.OnTileFullyLoaded += HandleTileLoaded;
				_worldGenerator.OnTileUnloaded += HandleTileUnloaded;
				_subscribed = true;
				if (_playerService != null)
				{
					_playerService.OnPlayerRegistered += HandlePlayerRegistered;
					if (_playerService.IsPlayerSpawned)
					{
						HandlePlayerRegistered();
					}
				}
				_cullRoutine = StartCoroutine(CullLoop());
			}
		}

		private void OnDisable()
		{
			FloatingOriginManager.UnregisterShiftable(this);
			if (_subscribed && _worldGenerator != null)
			{
				_worldGenerator.OnTileFullyLoaded -= HandleTileLoaded;
				_worldGenerator.OnTileUnloaded -= HandleTileUnloaded;
				_subscribed = false;
			}
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= HandlePlayerRegistered;
			}
			if (_cullRoutine != null)
			{
				StopCoroutine(_cullRoutine);
				_cullRoutine = null;
			}
			DestroyAll();
		}

		public void OnOriginShift(Vector3 delta)
		{
			for (int i = 0; i < _allActive.Count; i++)
			{
				Thumbleweed thumbleweed = _allActive[i];
				if (thumbleweed != null)
				{
					thumbleweed.OnOriginShift(delta);
				}
			}
		}

		private void HandlePlayerRegistered()
		{
			if (_playerService != null && _playerService.TryGetCameraTransform(out var cameraTransform))
			{
				_cameraTransform = cameraTransform;
			}
		}

		private void HandleTileLoaded(Vector2Int chunkCoord, Terrain terrain)
		{
			if (config == null || !config.HasValidVariants() || terrain == null || _allActive.Count >= config.GlobalMaxActive)
			{
				return;
			}
			System.Random random = _worldGenerator.SeedManager.CreateRandom($"Thumbleweed_Chunk_{chunkCoord.x}_{chunkCoord.y}");
			if (random.NextDouble() > (double)config.SpawnChancePerChunk)
			{
				return;
			}
			int num = Mathf.Max(0, config.SpawnCountPerChunkRange.x);
			int num2 = Mathf.Max(num, config.SpawnCountPerChunkRange.y);
			int num3 = ((num2 <= num) ? num : (num + random.Next(0, num2 - num + 1)));
			if (num3 <= 0)
			{
				return;
			}
			Vector3 vector = ComputeChunkWindDirection(random);
			float num4 = Mathf.Lerp(config.WindStrengthRange.x, config.WindStrengthRange.y, (float)random.NextDouble());
			Vector3 windForce = vector * num4;
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = terrain.transform.position;
			Vector3 size = terrainData.size;
			float tileSize = _worldGenerator.TileSize;
			Vector3 vector2 = new Vector3((float)chunkCoord.x * tileSize, 0f, (float)chunkCoord.y * tileSize);
			_chunkSpawnPositionsBuffer.Clear();
			float num5 = config.MinDistanceBetweenSpawns * config.MinDistanceBetweenSpawns;
			int num6 = num3 * 4;
			int num7 = 0;
			while (num7 < num3 && num6-- > 0 && _allActive.Count < config.GlobalMaxActive)
			{
				float num8 = (float)random.NextDouble();
				float num9 = (float)random.NextDouble();
				float num10 = vector2.x + num8 * tileSize;
				float num11 = vector2.z + num9 * tileSize;
				Vector3 worldPosition = new Vector3(num10, 0f, num11);
				float x = Mathf.Clamp01((num10 - position.x) / size.x);
				float y = Mathf.Clamp01((num11 - position.z) / size.z);
				if (Vector3.Angle(terrainData.GetInterpolatedNormal(x, y), Vector3.up) > config.MaxSlopeAngle)
				{
					continue;
				}
				float num12 = terrain.SampleHeight(worldPosition) + position.y;
				Vector3 vector3 = new Vector3(num10, num12 + config.SphereRadius + config.SpawnHeightEpsilon, num11);
				bool flag = false;
				for (int i = 0; i < _chunkSpawnPositionsBuffer.Count; i++)
				{
					if ((_chunkSpawnPositionsBuffer[i] - vector3).sqrMagnitude < num5)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ThumbleweedPrefabVariant thumbleweedPrefabVariant = PickVariant(random);
					if (thumbleweedPrefabVariant != null && !(thumbleweedPrefabVariant.Prefab == null))
					{
						float scale = Mathf.Lerp(config.ScaleRange.x, config.ScaleRange.y, (float)random.NextDouble());
						float num13 = Mathf.Lerp(config.InitialSpinRange.x, config.InitialSpinRange.y, (float)random.NextDouble());
						float x2 = (float)(random.NextDouble() * 2.0 - 1.0);
						float y2 = (float)(random.NextDouble() * 2.0 - 1.0);
						float z = (float)(random.NextDouble() * 2.0 - 1.0);
						Vector3 spinTorque = new Vector3(x2, y2, z).normalized * num13;
						SpawnInstance(thumbleweedPrefabVariant.Prefab, scale, vector3, chunkCoord, windForce, spinTorque);
						_chunkSpawnPositionsBuffer.Add(vector3);
						num7++;
					}
				}
			}
			_ = logSpawnEvents;
		}

		public bool SpawnAtPosition(Vector3 worldPos, Vector3 horizontalWindDirection)
		{
			if (config == null || !config.HasValidVariants())
			{
				return false;
			}
			if (_allActive.Count >= config.GlobalMaxActive)
			{
				return false;
			}
			Vector3 vector = new Vector3(horizontalWindDirection.x, 0f, horizontalWindDirection.z);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.right;
			}
			vector.Normalize();
			float num = UnityEngine.Random.Range(config.WindStrengthRange.x, config.WindStrengthRange.y);
			Vector3 windForce = vector * num;
			ThumbleweedPrefabVariant thumbleweedPrefabVariant = PickVariantUnity();
			if (thumbleweedPrefabVariant == null || thumbleweedPrefabVariant.Prefab == null)
			{
				return false;
			}
			float scale = Mathf.Lerp(config.ScaleRange.x, config.ScaleRange.y, UnityEngine.Random.value);
			float num2 = Mathf.Lerp(config.InitialSpinRange.x, config.InitialSpinRange.y, UnityEngine.Random.value);
			Vector3 spinTorque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * num2;
			float num3 = ((_worldGenerator != null) ? _worldGenerator.TileSize : 100f);
			Vector2Int chunkCoord = new Vector2Int(Mathf.FloorToInt(worldPos.x / num3), Mathf.FloorToInt(worldPos.z / num3));
			Vector3 pos = worldPos + Vector3.up * (config.SphereRadius + config.SpawnHeightEpsilon);
			SpawnInstance(thumbleweedPrefabVariant.Prefab, scale, pos, chunkCoord, windForce, spinTorque);
			return true;
		}

		private ThumbleweedPrefabVariant PickVariantUnity()
		{
			IReadOnlyList<ThumbleweedPrefabVariant> prefabVariants = config.PrefabVariants;
			if (prefabVariants == null || prefabVariants.Count == 0)
			{
				return null;
			}
			float num = 0f;
			for (int i = 0; i < prefabVariants.Count; i++)
			{
				ThumbleweedPrefabVariant thumbleweedPrefabVariant = prefabVariants[i];
				if (thumbleweedPrefabVariant != null && thumbleweedPrefabVariant.Prefab != null && thumbleweedPrefabVariant.Weight > 0f)
				{
					num += thumbleweedPrefabVariant.Weight;
				}
			}
			if (num <= 0f)
			{
				return null;
			}
			float num2 = UnityEngine.Random.value * num;
			float num3 = 0f;
			for (int j = 0; j < prefabVariants.Count; j++)
			{
				ThumbleweedPrefabVariant thumbleweedPrefabVariant2 = prefabVariants[j];
				if (thumbleweedPrefabVariant2 != null && !(thumbleweedPrefabVariant2.Prefab == null) && !(thumbleweedPrefabVariant2.Weight <= 0f))
				{
					num3 += thumbleweedPrefabVariant2.Weight;
					if (num2 <= num3)
					{
						return thumbleweedPrefabVariant2;
					}
				}
			}
			return null;
		}

		private Vector3 ComputeChunkWindDirection(System.Random rng)
		{
			Vector3 vector = new Vector3(config.BaseWindDirectionXZ.x, 0f, config.BaseWindDirectionXZ.y);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.right;
			}
			vector.Normalize();
			return Quaternion.AngleAxis(((float)rng.NextDouble() * 2f - 1f) * config.WindDirectionJitterDegrees, Vector3.up) * vector;
		}

		private ThumbleweedPrefabVariant PickVariant(System.Random rng)
		{
			IReadOnlyList<ThumbleweedPrefabVariant> prefabVariants = config.PrefabVariants;
			if (prefabVariants == null || prefabVariants.Count == 0)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < prefabVariants.Count; i++)
			{
				ThumbleweedPrefabVariant thumbleweedPrefabVariant = prefabVariants[i];
				if (thumbleweedPrefabVariant != null && thumbleweedPrefabVariant.Prefab != null && thumbleweedPrefabVariant.Weight > 0f)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			ThumbleweedPrefabVariant[] array = new ThumbleweedPrefabVariant[num];
			int num2 = 0;
			for (int j = 0; j < prefabVariants.Count; j++)
			{
				ThumbleweedPrefabVariant thumbleweedPrefabVariant2 = prefabVariants[j];
				if (thumbleweedPrefabVariant2 != null && thumbleweedPrefabVariant2.Prefab != null && thumbleweedPrefabVariant2.Weight > 0f)
				{
					array[num2++] = thumbleweedPrefabVariant2;
				}
			}
			return WeightedRandom.Select(array, (ThumbleweedPrefabVariant p) => p.Weight, rng);
		}

		private void SpawnInstance(GameObject prefab, float scale, Vector3 pos, Vector2Int chunkCoord, Vector3 windForce, Vector3 spinTorque)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab, pos, Quaternion.identity, base.transform);
			gameObject.transform.localScale = Vector3.one * scale;
			if (!gameObject.TryGetComponent<Thumbleweed>(out var component))
			{
				EvilLogger.LogError("[ThumbleweedManager] Prefab '" + prefab.name + "' is missing a Thumbleweed component; destroying instance.", "SpawnInstance", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\Thumbleweed\\ThumbleweedManager.cs", 308);
				UnityEngine.Object.Destroy(gameObject);
				return;
			}
			component.Configure(config.RigidbodyMass, config.LinearDrag, config.AngularDrag, config.SphereRadius, config.PhysicsMaterial);
			component.Enable(pos, chunkCoord, windForce, spinTorque);
			if (!_byChunk.TryGetValue(chunkCoord, out var value))
			{
				value = new List<Thumbleweed>();
				_byChunk[chunkCoord] = value;
			}
			value.Add(component);
			_allActive.Add(component);
		}

		private void HandleTileUnloaded(Vector2Int chunkCoord)
		{
			if (!_byChunk.TryGetValue(chunkCoord, out var value))
			{
				return;
			}
			for (int i = 0; i < value.Count; i++)
			{
				Thumbleweed thumbleweed = value[i];
				if (!(thumbleweed == null))
				{
					_allActive.Remove(thumbleweed);
					UnityEngine.Object.Destroy(thumbleweed.gameObject);
				}
			}
			value.Clear();
			_byChunk.Remove(chunkCoord);
		}

		private IEnumerator CullLoop()
		{
			WaitForSeconds wait = new WaitForSeconds(config.CullCheckInterval);
			float enableSq = config.EnableDistance * config.EnableDistance;
			float disableSq = config.DisableDistance * config.DisableDistance;
			float destroySq = config.DestroyDistance * config.DestroyDistance;
			while (true)
			{
				yield return wait;
				Vector3 cameraPosition = GetCameraPosition();
				if (cameraPosition == Vector3.zero && _cameraTransform == null)
				{
					continue;
				}
				for (int num = _allActive.Count - 1; num >= 0; num--)
				{
					Thumbleweed thumbleweed = _allActive[num];
					if (thumbleweed == null)
					{
						_allActive.RemoveAt(num);
					}
					else if (thumbleweed.ElapsedSeconds > config.MaxLifetimeSeconds || thumbleweed.TraveledDistance > config.MaxTravelDistance)
					{
						RemoveInstance(thumbleweed);
					}
					else
					{
						float sqrMagnitude = (thumbleweed.transform.position - cameraPosition).sqrMagnitude;
						if (sqrMagnitude > destroySq)
						{
							RemoveInstance(thumbleweed);
						}
						else
						{
							bool activeSelf = thumbleweed.gameObject.activeSelf;
							if (activeSelf && sqrMagnitude > disableSq)
							{
								thumbleweed.SetSimulationActive(active: false);
							}
							else if (!activeSelf && sqrMagnitude < enableSq)
							{
								thumbleweed.SetSimulationActive(active: true);
							}
						}
					}
				}
			}
		}

		private Vector3 GetCameraPosition()
		{
			if (_cameraTransform != null)
			{
				return _cameraTransform.position;
			}
			if (Camera.main != null)
			{
				return Camera.main.transform.position;
			}
			return Vector3.zero;
		}

		private void RemoveInstance(Thumbleweed t)
		{
			if (!(t == null))
			{
				if (_byChunk.TryGetValue(t.ChunkCoord, out var value))
				{
					value.Remove(t);
				}
				_allActive.Remove(t);
				UnityEngine.Object.Destroy(t.gameObject);
			}
		}

		private void DestroyAll()
		{
			for (int num = _allActive.Count - 1; num >= 0; num--)
			{
				Thumbleweed thumbleweed = _allActive[num];
				if (thumbleweed != null)
				{
					UnityEngine.Object.Destroy(thumbleweed.gameObject);
				}
			}
			_allActive.Clear();
			_byChunk.Clear();
		}
	}
}

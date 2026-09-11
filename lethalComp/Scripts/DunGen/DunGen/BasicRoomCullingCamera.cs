using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace DunGen
{
	[AddComponentMenu("DunGen/Culling/Adjacent Room Culling (Multi-Camera)")]
	public class BasicRoomCullingCamera : MonoBehaviour
	{
		protected struct RendererData
		{
			public Renderer Renderer;

			public bool Enabled;

			public RendererData(Renderer renderer, bool enabled)
			{
				Renderer = renderer;
				Enabled = enabled;
			}
		}

		protected struct LightData
		{
			public Light Light;

			public bool Enabled;

			public LightData(Light light, bool enabled)
			{
				Light = light;
				Enabled = enabled;
			}
		}

		protected struct ReflectionProbeData
		{
			public ReflectionProbe Probe;

			public bool Enabled;

			public ReflectionProbeData(ReflectionProbe probe, bool enabled)
			{
				Probe = probe;
				Enabled = enabled;
			}
		}

		public int AdjacentTileDepth = 1;

		public bool CullBehindClosedDoors = true;

		public Transform TargetOverride;

		public bool CullInEditor;

		public bool CullLights = true;

		protected bool isCulling;

		protected bool isDirty;

		protected DungeonGenerator generator;

		protected Tile currentTile;

		protected List<Dungeon> dungeons = new List<Dungeon>();

		protected List<Tile> allTiles = new List<Tile>();

		protected List<Door> allDoors = new List<Door>();

		protected List<Tile> visibleTiles = new List<Tile>();

		protected Dictionary<Tile, List<RendererData>> rendererVisibilities = new Dictionary<Tile, List<RendererData>>();

		protected Dictionary<Tile, List<LightData>> lightVisibilities = new Dictionary<Tile, List<LightData>>();

		protected Dictionary<Tile, List<ReflectionProbeData>> reflectionProbeVisibilities = new Dictionary<Tile, List<ReflectionProbeData>>();

		protected Dictionary<Door, List<RendererData>> doorRendererVisibilities = new Dictionary<Door, List<RendererData>>();

		public bool IsReady { get; protected set; }

		protected virtual void Awake()
		{
			RuntimeDungeon runtimeDungeon = UnityUtil.FindObjectByType<RuntimeDungeon>();
			if (runtimeDungeon != null)
			{
				generator = runtimeDungeon.Generator;
				generator.OnGenerationComplete += OnDungeonGenerationComplete;
				if (generator.Status == GenerationStatus.Complete)
				{
					AddDungeon(generator.CurrentDungeon);
				}
			}
		}

		protected virtual void OnDestroy()
		{
			if (generator != null)
			{
				generator.OnGenerationComplete -= OnDungeonGenerationComplete;
			}
		}

		protected virtual void OnEnable()
		{
			if (RenderPipelineManager.currentPipeline != null)
			{
				RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
				RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
			}
			else
			{
				Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(EnableCulling));
				Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(DisableCulling));
			}
		}

		protected virtual void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(EnableCulling));
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(DisableCulling));
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			EnableCulling(camera);
		}

		private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			DisableCulling(camera);
		}

		private void OnDungeonGenerationComplete(DungeonGenerator generator)
		{
			if ((generator.AttachmentSettings == null || generator.AttachmentSettings.TileProxy == null) && dungeons.Count > 0)
			{
				RemoveDungeon(dungeons[dungeons.Count - 1]);
			}
			AddDungeon(generator.CurrentDungeon);
		}

		protected virtual void EnableCulling(Camera camera)
		{
			SetCullingEnabled(camera, enabled: true);
		}

		protected virtual void DisableCulling(Camera camera)
		{
			SetCullingEnabled(camera, enabled: false);
		}

		protected void SetCullingEnabled(Camera camera, bool enabled)
		{
			if (IsReady && !(camera == null) && camera.gameObject == base.gameObject)
			{
				SetIsCulling(enabled);
			}
		}

		protected virtual void LateUpdate()
		{
			if (!IsReady)
			{
				return;
			}
			Transform transform = ((TargetOverride != null) ? TargetOverride : base.transform);
			if (currentTile == null || !currentTile.Bounds.Contains(transform.position))
			{
				foreach (Tile allTile in allTiles)
				{
					if (!(allTile == null) && allTile.Bounds.Contains(transform.position))
					{
						currentTile = allTile;
						break;
					}
				}
				isDirty = true;
			}
			if (!isDirty)
			{
				return;
			}
			UpdateCulling();
			foreach (Tile allTile2 in allTiles)
			{
				if (allTile2 != null && !visibleTiles.Contains(allTile2))
				{
					UpdateRendererList(allTile2);
				}
			}
		}

		protected void UpdateRendererList(Tile tile)
		{
			if (!rendererVisibilities.TryGetValue(tile, out var value))
			{
				value = (rendererVisibilities[tile] = new List<RendererData>());
			}
			else
			{
				value.Clear();
			}
			Renderer[] componentsInChildren = tile.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				value.Add(new RendererData(renderer, renderer.enabled));
			}
			if (CullLights)
			{
				if (!lightVisibilities.TryGetValue(tile, out var value2))
				{
					value2 = (lightVisibilities[tile] = new List<LightData>());
				}
				else
				{
					value2.Clear();
				}
				Light[] componentsInChildren2 = tile.GetComponentsInChildren<Light>();
				foreach (Light light in componentsInChildren2)
				{
					value2.Add(new LightData(light, light.enabled));
				}
			}
			if (!reflectionProbeVisibilities.TryGetValue(tile, out var value3))
			{
				value3 = (reflectionProbeVisibilities[tile] = new List<ReflectionProbeData>());
			}
			else
			{
				value3.Clear();
			}
			ReflectionProbe[] componentsInChildren3 = tile.GetComponentsInChildren<ReflectionProbe>();
			foreach (ReflectionProbe reflectionProbe in componentsInChildren3)
			{
				value3.Add(new ReflectionProbeData(reflectionProbe, reflectionProbe.enabled));
			}
		}

		protected void SetIsCulling(bool isCulling)
		{
			this.isCulling = isCulling;
			for (int i = 0; i < allTiles.Count; i++)
			{
				Tile tile = allTiles[i];
				if (visibleTiles.Contains(tile))
				{
					continue;
				}
				if (rendererVisibilities.TryGetValue(tile, out var value))
				{
					foreach (RendererData item in value)
					{
						if (item.Renderer != null)
						{
							item.Renderer.enabled = !isCulling && item.Enabled;
						}
					}
				}
				if (CullLights && lightVisibilities.TryGetValue(tile, out var value2))
				{
					foreach (LightData item2 in value2)
					{
						if (item2.Light != null)
						{
							item2.Light.enabled = !isCulling && item2.Enabled;
						}
					}
				}
				if (!reflectionProbeVisibilities.TryGetValue(tile, out var value3))
				{
					continue;
				}
				foreach (ReflectionProbeData item3 in value3)
				{
					if (item3.Probe != null)
					{
						item3.Probe.enabled = !isCulling && item3.Enabled;
					}
				}
			}
			foreach (Door allDoor in allDoors)
			{
				bool flag = visibleTiles.Contains(allDoor.DoorwayA.Tile) || visibleTiles.Contains(allDoor.DoorwayB.Tile);
				if (!doorRendererVisibilities.TryGetValue(allDoor, out var value4))
				{
					continue;
				}
				foreach (RendererData item4 in value4)
				{
					if (item4.Renderer != null)
					{
						item4.Renderer.enabled = (isCulling ? flag : item4.Enabled);
					}
				}
			}
		}

		protected void UpdateCulling()
		{
			isDirty = false;
			visibleTiles.Clear();
			if (currentTile != null)
			{
				visibleTiles.Add(currentTile);
			}
			int num = 0;
			for (int i = 0; i < AdjacentTileDepth; i++)
			{
				int count = visibleTiles.Count;
				for (int j = num; j < count; j++)
				{
					foreach (Doorway usedDoorway in visibleTiles[j].UsedDoorways)
					{
						Tile tile = usedDoorway.ConnectedDoorway.Tile;
						if (visibleTiles.Contains(tile))
						{
							continue;
						}
						if (CullBehindClosedDoors)
						{
							Door doorComponent = usedDoorway.DoorComponent;
							if (doorComponent != null && doorComponent.ShouldCullBehind)
							{
								continue;
							}
						}
						visibleTiles.Add(tile);
					}
				}
				num = count;
			}
		}

		public void SetDungeon(Dungeon newDungeon)
		{
			if (!(newDungeon == null))
			{
				ClearAllDungeons();
				AddDungeon(newDungeon);
			}
		}

		public void AddDungeon(Dungeon dungeon)
		{
			if (dungeon == null || dungeons.Contains(dungeon))
			{
				return;
			}
			IEnumerable<Door> allDoorsInDungeon = GetAllDoorsInDungeon(dungeon);
			dungeons.Add(dungeon);
			allTiles.AddRange(dungeon.AllTiles);
			allDoors.AddRange(allDoorsInDungeon);
			foreach (Door item in allDoorsInDungeon)
			{
				List<RendererData> list = new List<RendererData>();
				doorRendererVisibilities[item] = list;
				Renderer[] componentsInChildren = item.GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren)
				{
					list.Add(new RendererData(renderer, renderer.enabled));
				}
				item.OnDoorStateChanged += OnDoorStateChanged;
			}
			IsReady = true;
			isDirty = true;
		}

		private void RemoveNullKeys<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary)
		{
			TKey[] array = dictionary.Keys.Where((TKey val) => val == null).ToArray();
			foreach (TKey key in array)
			{
				if (dictionary.ContainsKey(key))
				{
					dictionary.Remove(key);
				}
			}
		}

		public void RemoveDungeon(Dungeon dungeon)
		{
			if (dungeon == null || !dungeons.Contains(dungeon))
			{
				return;
			}
			dungeons.Remove(dungeon);
			allTiles.RemoveAll((Tile x) => !x);
			visibleTiles.RemoveAll((Tile x) => !x);
			allDoors.RemoveAll((Door x) => !x);
			RemoveNullKeys(ref rendererVisibilities);
			RemoveNullKeys(ref lightVisibilities);
			RemoveNullKeys(ref reflectionProbeVisibilities);
			foreach (Tile allTile in dungeon.AllTiles)
			{
				if (!(allTile == null))
				{
					if (allTiles.Contains(allTile))
					{
						allTiles.Remove(allTile);
					}
					if (visibleTiles.Contains(allTile))
					{
						visibleTiles.Remove(allTile);
					}
					if (rendererVisibilities.ContainsKey(allTile))
					{
						rendererVisibilities.Remove(allTile);
					}
					if (lightVisibilities.ContainsKey(allTile))
					{
						lightVisibilities.Remove(allTile);
					}
					if (reflectionProbeVisibilities.ContainsKey(allTile))
					{
						reflectionProbeVisibilities.Remove(allTile);
					}
				}
			}
			foreach (GameObject door in dungeon.Doors)
			{
				if (!(door == null) && door.TryGetComponent<Door>(out var component))
				{
					if (allDoors.Contains(component))
					{
						allDoors.Remove(component);
					}
					if (doorRendererVisibilities.ContainsKey(component))
					{
						doorRendererVisibilities.Remove(component);
					}
					component.OnDoorStateChanged -= OnDoorStateChanged;
				}
			}
		}

		public void ClearAllDungeons()
		{
			IsReady = false;
			foreach (Door allDoor in allDoors)
			{
				if (allDoor != null)
				{
					allDoor.OnDoorStateChanged -= OnDoorStateChanged;
				}
			}
			dungeons.Clear();
			allTiles.Clear();
			visibleTiles.Clear();
			allDoors.Clear();
			rendererVisibilities.Clear();
			lightVisibilities.Clear();
			reflectionProbeVisibilities.Clear();
			doorRendererVisibilities.Clear();
		}

		protected IEnumerable<Door> GetAllDoorsInDungeon(Dungeon dungeon)
		{
			foreach (GameObject door in dungeon.Doors)
			{
				if (!(door == null))
				{
					Door component = door.GetComponent<Door>();
					if (component != null)
					{
						yield return component;
					}
				}
			}
		}

		protected virtual void OnDoorStateChanged(Door door, bool isOpen)
		{
			isDirty = true;
		}
	}
}

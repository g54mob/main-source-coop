using System;
using System.Collections;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes.MatrixGenerators;
using MapMagic.Products;
using MapMagic.Terrains;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Outputs", name = "Objects", section = 2, colorType = typeof(TransitionsList), iconName = "GeneratorIcons/ObjectsOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Objects")]
	public class ObjectsOutput : OutputGenerator, IInlet<TransitionsList>, IUnit, IPrepare
	{
		public class ApplyObjectsData : IApplyDataRoutine, IApplyData
		{
			public ObjectsPool.Prototype[] prototypes;

			public List<Transition>[] transitions;

			public float terrainHeight;

			public int objsPerIteration = 500;

			public int Resolution => 0;

			public void Apply(Terrain terrain)
			{
				terrain.transform.parent.GetComponent<TerrainTile>().objectsPool.Reposition(prototypes, transitions);
			}

			public IEnumerator ApplyRoutine(Terrain terrain)
			{
				ObjectsPool objectsPool = terrain.transform.parent.GetComponent<TerrainTile>().objectsPool;
				IEnumerator e = objectsPool.RepositionRoutine(prototypes, transitions, objsPerIteration);
				while (e.MoveNext())
				{
					yield return null;
				}
			}
		}

		public GameObject[] prefabs = new GameObject[1];

		public PositioningSettings posSettings;

		public BiomeBlend biomeBlend = BiomeBlend.Random;

		public OutputLevel outputLevel = OutputLevel.Main;

		public bool guiMultiprefab;

		public bool guiProperties;

		public bool allowReposition = true;

		public bool instantiateClones;

		public int seed = 12345;

		public bool objHeight = true;

		public bool relativeHeight = true;

		public bool guiHeight;

		public bool useRotation = true;

		public bool takeTerrainNormal;

		public bool rotateYonly;

		public bool regardPrefabRotation;

		public bool guiRotation;

		public bool useScale = true;

		public bool scaleYonly;

		public bool regardPrefabScale;

		public bool guiScale;

		public static FinalizeAction finalizeAction = Finalize;

		public override OutputLevel OutputLevel => outputLevel;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsOut.cs", 50);
		}

		public static PositioningSettings CreatePosSettings(ObjectsOutput output)
		{
			return new PositioningSettings
			{
				objHeight = output.objHeight,
				relativeHeight = output.relativeHeight,
				guiHeight = output.guiHeight,
				useRotation = output.useRotation,
				takeTerrainNormal = output.takeTerrainNormal,
				rotateYonly = output.rotateYonly,
				regardPrefabRotation = output.regardPrefabRotation,
				guiRotation = output.guiRotation,
				useScale = output.useScale,
				scaleYonly = output.scaleYonly,
				regardPrefabScale = output.regardPrefabScale,
				guiScale = output.guiScale
			};
		}

		public void Prepare(TileData data, Terrain terrain)
		{
			for (int i = 0; i < prefabs.Length; i++)
			{
				if (prefabs[i] == null)
				{
					prefabs[i] = null;
				}
			}
		}

		public List<ObjectsPool.Prototype> GetPrototypes()
		{
			List<ObjectsPool.Prototype> list = new List<ObjectsPool.Prototype>();
			for (int i = 0; i < prefabs.Length; i++)
			{
				if (!prefabs[i].IsNull())
				{
					list.Add(new ObjectsPool.Prototype
					{
						prefab = prefabs[i],
						allowReposition = allowReposition,
						instantiateClones = instantiateClones,
						regardPrefabRotation = posSettings.regardPrefabRotation,
						regardPrefabScale = posSettings.regardPrefabScale
					});
				}
			}
			return list;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop == null || !stop.stop)
			{
				TransitionsList product = data.ReadInletProduct(this);
				if (enabled)
				{
					data.StoreOutput(this, typeof(ObjectsOutput), this, product);
					data.MarkFinalize(Finalize, stop);
				}
				else
				{
					data.RemoveFinalize(finalizeAction);
				}
			}
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Subscribe()
		{
			Graph.OnOutputFinalized = (Action<Type, TileData, IApplyData, StopToken>)Delegate.Combine(Graph.OnOutputFinalized, new Action<Type, TileData, IApplyData, StopToken>(FinalizeIfHeightFinalized));
		}

		private static void FinalizeIfHeightFinalized(Type type, TileData tileData, IApplyData applyData, StopToken stop)
		{
			if (type == typeof(HeightOutput200))
			{
				tileData.MarkFinalize(finalizeAction, stop);
			}
		}

		public static void Finalize(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			Dictionary<ObjectsPool.Prototype, List<Transition>> dictionary = new Dictionary<ObjectsPool.Prototype, List<Transition>>();
			foreach (var (objectsOutput, transitionsList, matrixWorld) in data.Outputs<ObjectsOutput, TransitionsList, MatrixWorld>(typeof(ObjectsOutput), inSubs: true))
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				if (transitionsList == null || (matrixWorld != null && matrixWorld.IsEmpty()))
				{
					continue;
				}
				if (objectsOutput.posSettings == null)
				{
					objectsOutput.posSettings = CreatePosSettings(objectsOutput);
				}
				List<ObjectsPool.Prototype> prototypes = objectsOutput.GetPrototypes();
				if (prototypes.Count == 0)
				{
					continue;
				}
				foreach (ObjectsPool.Prototype item in prototypes)
				{
					if (!dictionary.ContainsKey(item))
					{
						dictionary.Add(item, new List<Transition>());
					}
				}
				Noise noise = new Noise(data.random, objectsOutput.seed);
				for (int i = 0; i < transitionsList.count; i++)
				{
					Transition trs = transitionsList.arr[i];
					if (data.area.active.Contains(trs.pos) && !PositioningSettings.SkipOnBiome(ref trs, objectsOutput.biomeBlend, matrixWorld, data.random))
					{
						objectsOutput.posSettings.MoveRotateScale(ref trs, data);
						trs.pos -= (Vector3)data.area.active.worldPos;
						float num = noise.Random(trs.hash);
						ObjectsPool.Prototype key = prototypes[(int)(num * (float)prototypes.Count)];
						dictionary[key].Add(trs);
					}
				}
			}
			if (stop == null || !stop.stop)
			{
				ApplyObjectsData applyObjectsData = new ApplyObjectsData
				{
					prototypes = dictionary.Keys.ToArray(),
					transitions = dictionary.Values.ToArray(),
					terrainHeight = data.globals.height,
					objsPerIteration = data.globals.objectsNumPerFrame
				};
				Graph.OnOutputFinalized?.Invoke(typeof(ObjectsOutput), data, applyObjectsData, stop);
				data.MarkApply(applyObjectsData);
			}
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
			if (posSettings == null)
			{
				posSettings = CreatePosSettings(this);
			}
			_ = terrain.terrainData.size;
			ObjectsPool objectsPool = terrain.transform.parent.GetComponent<TerrainTile>().objectsPool;
			List<ObjectsPool.Prototype> prototypes = GetPrototypes();
			objectsPool.ClearPrototypes(prototypes.ToArray());
		}
	}
}

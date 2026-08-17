using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes.MatrixGenerators;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Outputs", name = "Trees", section = 2, colorType = typeof(TransitionsList), iconName = "GeneratorIcons/TreesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Objects")]
	public class TreesOutput : OutputGenerator, IInlet<TransitionsList>, IUnit, IPrepare
	{
		public class ApplyTreesData : IApplyData
		{
			public TreeInstance[] treeInstances;

			public TreePrototype[] treePrototypes;

			public int Resolution => 0;

			public void Read(Terrain terrain)
			{
				TerrainData terrainData = terrain.terrainData;
				treeInstances = terrainData.treeInstances;
				treePrototypes = terrainData.treePrototypes;
			}

			public void Apply(Terrain terrain)
			{
				if (treePrototypes.Contains((TreePrototype p) => p.prefab == null))
				{
					RemoveNullPrototypes(ref treePrototypes, ref treeInstances);
				}
				if (treePrototypes.Length == 0 && terrain.terrainData.treeInstanceCount != 0)
				{
					terrain.terrainData.treeInstances = new TreeInstance[0];
					terrain.terrainData.treePrototypes = new TreePrototype[0];
				}
				terrain.terrainData.treePrototypes = treePrototypes;
				terrain.terrainData.treeInstances = treeInstances;
			}
		}

		public GameObject[] prefabs = new GameObject[1];

		public PositioningSettings posSettings;

		public BiomeBlend biomeBlend = BiomeBlend.Random;

		public OutputLevel outputLevel = OutputLevel.Main;

		public bool guiMultiprefab;

		public bool guiProperties;

		public int seed = 12345;

		public Color color = Color.white;

		public Color lightmapColor = Color.white;

		public float bendFactor;

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
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsOut.cs", 250);
		}

		public static PositioningSettings CreatePosSettings(TreesOutput output)
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

		public List<TreePrototype> GetPrototypes()
		{
			List<TreePrototype> list = new List<TreePrototype>();
			for (int i = 0; i < prefabs.Length; i++)
			{
				if (!prefabs[i].IsNull())
				{
					list.Add(new TreePrototype
					{
						prefab = prefabs[i],
						bendFactor = bendFactor
					});
				}
			}
			return list;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			TransitionsList product = data.ReadInletProduct(this);
			if (stop == null || !stop.stop)
			{
				if (enabled)
				{
					data.StoreOutput(this, typeof(TreesOutput), this, product);
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
			List<TreeInstance> list = new List<TreeInstance>();
			List<TreePrototype> list2 = new List<TreePrototype>();
			int num = 0;
			foreach (var (treesOutput, transitionsList, matrixWorld) in data.Outputs<TreesOutput, TransitionsList, MatrixWorld>(typeof(TreesOutput), inSubs: true))
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				if (transitionsList == null || (matrixWorld != null && matrixWorld.IsEmpty()))
				{
					continue;
				}
				if (treesOutput.posSettings == null)
				{
					treesOutput.posSettings = CreatePosSettings(treesOutput);
				}
				Noise noise = new Noise(data.random, treesOutput.seed);
				TreePrototype[] array = new TreePrototype[treesOutput.prefabs.Length];
				for (int i = 0; i < treesOutput.prefabs.Length; i++)
				{
					array[i] = new TreePrototype
					{
						prefab = treesOutput.prefabs[i],
						bendFactor = treesOutput.bendFactor
					};
				}
				list2.AddRange(array);
				for (int j = 0; j < transitionsList.count; j++)
				{
					Transition trs = transitionsList.arr[j];
					if (data.area.active.Contains(trs.pos) && !PositioningSettings.SkipOnBiome(ref trs, treesOutput.biomeBlend, matrixWorld, data.random))
					{
						treesOutput.posSettings.MoveRotateScale(ref trs, data);
						int num2 = (int)(noise.Random(trs.hash) * (float)treesOutput.prefabs.Length);
						TreeInstance item = new TreeInstance
						{
							position = (trs.pos - (Vector3)data.area.active.worldPos) / data.area.active.worldSize.x
						};
						if (!(item.position.x < 0f) && !(item.position.z < 0f) && !(item.position.x > 1f) && !(item.position.z > 1f))
						{
							item.position.y = trs.pos.y / data.globals.height;
							item.rotation = trs.Yaw;
							item.widthScale = trs.scale.x;
							item.heightScale = trs.scale.y;
							item.prototypeIndex = num + num2;
							item.color = treesOutput.color;
							item.lightmapColor = treesOutput.lightmapColor;
							list.Add(item);
						}
					}
				}
				num += treesOutput.prefabs.Length;
			}
			if (stop == null || !stop.stop)
			{
				ApplyTreesData applyTreesData = new ApplyTreesData
				{
					treePrototypes = list2.ToArray(),
					treeInstances = list.ToArray()
				};
				Graph.OnOutputFinalized?.Invoke(typeof(TreesOutput), data, applyTreesData, stop);
				data.MarkApply(applyTreesData);
			}
		}

		public static void RemoveNullPrototypes(List<TreePrototype> prototypes, List<TreeInstance> instances)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int count = prototypes.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				if (prototypes[i].prefab != null)
				{
					dictionary.Add(i, num);
					num++;
				}
			}
			for (int num2 = count - 1; num2 >= 0; num2--)
			{
				if (prototypes[num2].prefab == null)
				{
					prototypes.RemoveAt(num2);
				}
			}
			for (int num3 = instances.Count - 1; num3 >= 0; num3--)
			{
				if (!dictionary.TryGetValue(instances[num3].prototypeIndex, out var value))
				{
					instances.RemoveAt(num3);
				}
				else if (instances[num3].prototypeIndex != value)
				{
					TreeInstance value2 = instances[num3];
					value2.prototypeIndex = value;
					instances[num3] = value2;
				}
			}
		}

		public static void RemoveNullPrototypes(ref TreePrototype[] prototypes, ref TreeInstance[] instances)
		{
			List<TreePrototype> list = new List<TreePrototype>(prototypes);
			List<TreeInstance> list2 = new List<TreeInstance>(instances);
			RemoveNullPrototypes(list, list2);
			prototypes = list.ToArray();
			instances = list2.ToArray();
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
			if (posSettings == null)
			{
				posSettings = CreatePosSettings(this);
			}
			TerrainData terrainData = terrain.terrainData;
			TreePrototype[] treePrototypes = terrainData.treePrototypes;
			TreeInstance[] treeInstances = terrainData.treeInstances;
			List<TreePrototype> list = new List<TreePrototype>();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < treePrototypes.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < prefabs.Length; j++)
				{
					if (treePrototypes[i].prefab == prefabs[j] && treePrototypes[i].bendFactor < bendFactor + 0.0001f && treePrototypes[i].bendFactor > bendFactor - 0.0001f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					dictionary.Add(i, list.Count);
					list.Add(treePrototypes[i]);
				}
			}
			List<TreeInstance> list2 = new List<TreeInstance>();
			for (int k = 0; k < treeInstances.Length; k++)
			{
				if (dictionary.TryGetValue(treeInstances[k].prototypeIndex, out var value))
				{
					TreeInstance item = treeInstances[k];
					item.prototypeIndex = value;
					list2.Add(item);
				}
			}
			terrainData.treeInstances = list2.ToArray();
			terrainData.treePrototypes = list.ToArray();
		}
	}
}

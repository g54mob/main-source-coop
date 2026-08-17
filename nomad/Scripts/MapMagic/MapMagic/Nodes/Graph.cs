using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Den.Tools;
using MapMagic.Expose;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	[HelpURL("https://gitlab.com/denispahunov/mapmagic/wikis/home")]
	[CreateAssetMenu(menuName = "MapMagic/Empty Graph", fileName = "Graph.asset", order = 101)]
	public class Graph : ScriptableObject, ISerializationCallbackReceiver
	{
		[NonSerialized]
		public Generator[] generators = new Generator[0];

		[NonSerialized]
		public Dictionary<IInlet<object>, IOutlet<object>> links = new Dictionary<IInlet<object>, IOutlet<object>>();

		[NonSerialized]
		public Auxiliary[] groups = new Auxiliary[0];

		[NonSerialized]
		public Noise random = new Noise(12345, 32768);

		[NonSerialized]
		public Exposed exposed = new Exposed();

		[NonSerialized]
		public Override defaults = new Override();

		public int serializedVersion;

		public static Action<Graph, TileData> OnBeforeClearPrepareGenerate;

		public static Action<Generator, TileData> OnBeforeNodeCleared;

		public static Action<Generator, TileData> OnAfterNodeGenerated;

		public static Action<Type, TileData, IApplyData, StopToken> OnOutputFinalized;

		public bool guiShowDependent;

		public bool guiShowShared;

		public bool guiShowExposed;

		public bool guiShowDebug;

		public Vector2 guiMiniPos = new Vector2(20f, 20f);

		public Vector2 guiMiniAnchor = new Vector2(0f, 0f);

		[SerializeField]
		private GraphSerializer200Beta serializer200beta;

		public static Graph debugGraph;

		public static Graph Create(Graph src = null, bool inThread = false)
		{
			Graph graph = ((!inThread) ? ScriptableObject.CreateInstance<Graph>() : ((Graph)FormatterServices.GetUninitializedObject(typeof(Graph))));
			if (src == null)
			{
				graph.generators = new Generator[0];
			}
			else
			{
				graph.generators = (Generator[])Serializer.DeepCopy(src.generators);
				graph.random = src.random;
			}
			return graph;
		}

		public void Add(Generator gen)
		{
			if (generators.Contains(gen))
			{
				throw new Exception("Could not add generator " + gen?.ToString() + " since it is already in graph");
			}
			ArrayTools.Add(ref generators, gen);
		}

		public void Add(Auxiliary grp)
		{
			if (groups.Contains(grp))
			{
				throw new Exception("Could not add group " + grp?.ToString() + " since it is already in graph");
			}
			ArrayTools.Add(ref groups, grp);
		}

		public void Add(Group grp)
		{
			if (groups.Contains(grp))
			{
				throw new Exception("Could not add group " + grp?.ToString() + " since it is already in graph");
			}
			ArrayTools.Add(ref groups, grp);
		}

		public void Remove(Generator gen)
		{
			if (!generators.Contains(gen))
			{
				throw new Exception("Could not remove generator " + gen?.ToString() + " since it is not in graph");
			}
			UnlinkGenerator(gen);
			exposed.RemoveUnused(this);
			ArrayTools.Remove(ref generators, gen);
		}

		public void Remove(Group grp)
		{
			if (!groups.Contains(grp))
			{
				throw new Exception("Could not remove group " + grp?.ToString() + " since it is not in graph");
			}
			ArrayTools.Remove(ref groups, grp);
		}

		public Generator[] Import(Graph other, bool createOverride = false)
		{
			Graph graph = ScriptableObject.CreateInstance<Graph>();
			DeepCopy(other, graph);
			ArrayTools.AddRange(ref generators, graph.generators);
			foreach (KeyValuePair<IInlet<object>, IOutlet<object>> link in graph.links)
			{
				links.Add(link.Key, link.Value);
			}
			ArrayTools.AddRange(ref groups, graph.groups);
			Dictionary<ulong, ulong> oldNewIds = CheckFixIds(new HashSet<IUnit>(graph.generators));
			graph.exposed.ReplaceIds(oldNewIds);
			exposed.AddRange(graph.exposed);
			return graph.generators;
		}

		public Graph Export(HashSet<Generator> gensHash)
		{
			Graph graph = ScriptableObject.CreateInstance<Graph>();
			graph.generators = gensHash.ToArray();
			foreach (KeyValuePair<IInlet<object>, IOutlet<object>> link in links)
			{
				if (gensHash.Contains(link.Key.Gen) && gensHash.Contains(link.Value.Gen))
				{
					graph.links.Add(link.Key, link.Value);
				}
			}
			Generator[] array = graph.generators;
			foreach (Generator generator in array)
			{
				Exposed.Entry[] array2 = exposed[generator.Id];
				if (array2 != null)
				{
					graph.exposed.AddRange(array2);
				}
			}
			Graph graph2 = ScriptableObject.CreateInstance<Graph>();
			DeepCopy(graph, graph2);
			return graph2;
		}

		public Generator[] Duplicate(HashSet<Generator> gens)
		{
			Graph graph = Export(gens);
			Generator[] result = graph.generators;
			Import(graph);
			return result;
		}

		public static void Reposition(IList<Generator> gens, Vector2 newCenter)
		{
			Vector2 zero = Vector2.zero;
			foreach (Generator gen in gens)
			{
				zero += new Vector2(gen.guiPosition.x + gen.guiSize.x / 2f, gen.guiPosition.y);
			}
			zero /= (float)gens.Count;
			Vector2 vector = newCenter - zero;
			foreach (Generator gen2 in gens)
			{
				gen2.guiPosition += vector;
			}
		}

		public Dictionary<ulong, ulong> CheckFixIds(HashSet<IUnit> susGens = null)
		{
			Dictionary<ulong, IUnit> allIds = new Dictionary<ulong, IUnit>();
			Dictionary<ulong, ulong> dictionary = new Dictionary<ulong, ulong>();
			List<ulong> list = new List<ulong>();
			CheckFixIdsRecursively(susGens, allIds, dictionary, list);
			if (dictionary.Count != 0)
			{
				Debug.Log($"Changed generators ids on serialize: {dictionary.Count + list.Count}");
			}
			return dictionary;
		}

		private void CheckFixIdsRecursively(HashSet<IUnit> susGens, Dictionary<ulong, IUnit> allIds, Dictionary<ulong, ulong> oldNewIds, List<ulong> zeroNewIds)
		{
			if (generators == null)
			{
				OnAfterDeserialize();
			}
			foreach (IUnit item in AllUnits())
			{
				if (susGens != null && susGens.Contains(item))
				{
					continue;
				}
				if (allIds.TryGetValue(item.Id, out var value) || item.Id == 0L)
				{
					if (value == item)
					{
						continue;
					}
					ulong num = Id.Generate();
					while (allIds.ContainsKey(num))
					{
						num = Id.Generate();
					}
					if (item.Id != 0L)
					{
						oldNewIds.Add(item.Id, num);
					}
					else
					{
						zeroNewIds.Add(num);
					}
					item.Id = num;
				}
				allIds.Add(item.Id, item);
			}
			foreach (IBiome item2 in UnitsOfType<IBiome>())
			{
				if (item2.SubGraph != null)
				{
					item2.SubGraph.CheckFixIdsRecursively(susGens, allIds, oldNewIds, zeroNewIds);
				}
			}
			if (susGens == null)
			{
				return;
			}
			foreach (IUnit item3 in AllUnits())
			{
				if (!susGens.Contains(item3))
				{
					continue;
				}
				if (allIds.TryGetValue(item3.Id, out var value2) || item3.Id == 0L)
				{
					if (value2 == item3)
					{
						continue;
					}
					ulong num2 = Id.Generate();
					while (allIds.ContainsKey(num2))
					{
						num2 = Id.Generate();
					}
					if (item3.Id != 0L)
					{
						oldNewIds.Add(item3.Id, num2);
					}
					else
					{
						zeroNewIds.Add(num2);
					}
					item3.Id = num2;
				}
				allIds.Add(item3.Id, item3);
			}
		}

		public void Link(IOutlet<object> outlet, IInlet<object> inlet)
		{
			if (outlet == null && links.ContainsKey(inlet))
			{
				links.Remove(inlet);
			}
			else if (links.ContainsKey(inlet))
			{
				links[inlet] = outlet;
			}
			else
			{
				links.Add(inlet, outlet);
			}
			inlet.Gen.version++;
		}

		public void Link(IInlet<object> inlet, IOutlet<object> outlet)
		{
			Link(outlet, inlet);
		}

		public bool CheckLinkValidity(IOutlet<object> outlet, IInlet<object> inlet)
		{
			if (Generator.GetGenericType(outlet) != Generator.GetGenericType(inlet))
			{
				return false;
			}
			if (AreDependent(inlet.Gen, outlet.Gen))
			{
				return false;
			}
			return true;
		}

		public bool AreDependent(Generator prevGen, Generator nextGen)
		{
			if (prevGen == nextGen)
			{
				return true;
			}
			if (nextGen is IInlet<object> key && links.TryGetValue(key, out var value) && AreDependent(prevGen, value.Gen))
			{
				return true;
			}
			if (nextGen is IMultiInlet multiInlet)
			{
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					if (links.TryGetValue(item, out var value2) && AreDependent(prevGen, value2.Gen))
					{
						return true;
					}
				}
			}
			if (nextGen is ICustomDependence customDependence)
			{
				foreach (Generator item2 in customDependence.PriorGens())
				{
					if (AreDependent(prevGen, item2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool AreDependent(Generator prevGen, IInlet<object> nextInlet)
		{
			if (links.TryGetValue(nextInlet, out var value))
			{
				Generator gen = value.Gen;
				return AreDependent(prevGen, gen);
			}
			return false;
		}

		public bool IsLinked(IInlet<object> inlet)
		{
			return links.ContainsKey(inlet);
		}

		public IOutlet<object> GetLink(IInlet<object> inlet)
		{
			if (!links.TryGetValue(inlet, out var value))
			{
				return null;
			}
			return value;
		}

		public void UnlinkInlet(IInlet<object> inlet)
		{
			if (links.ContainsKey(inlet))
			{
				links.Remove(inlet);
			}
			inlet.Gen.version++;
		}

		public void UnlinkOutlet(IOutlet<object> outlet)
		{
			foreach (IInlet<object> item in new List<IInlet<object>>())
			{
				links.Remove(item);
				item.Gen.version++;
			}
		}

		public void UnlinkGenerator(Generator gen)
		{
			List<IInlet<object>> list = new List<IInlet<object>>();
			foreach (KeyValuePair<IInlet<object>, IOutlet<object>> link in links)
			{
				IInlet<object> key = link.Key;
				IOutlet<object> value = link.Value;
				if (key.Gen == gen)
				{
					list.Add(key);
					value.Gen.version++;
				}
				if (value.Gen == gen)
				{
					list.Add(key);
					key.Gen.version++;
				}
			}
			foreach (IInlet<object> item in list)
			{
				links.Remove(item);
			}
			gen.version++;
		}

		private List<IInlet<object>> LinkedInlets(IOutlet<object> outlet)
		{
			List<IInlet<object>> list = new List<IInlet<object>>();
			foreach (KeyValuePair<IInlet<object>, IOutlet<object>> link in links)
			{
				if (link.Value == outlet)
				{
					list.Add(link.Key);
				}
			}
			return list;
		}

		public void ThroughLink(Generator gen)
		{
			IInlet<object> inlet = null;
			IOutlet<object> outlet = null;
			if (gen is IInlet<object> && gen is IOutlet<object>)
			{
				inlet = (IInlet<object>)gen;
				outlet = (IOutlet<object>)gen;
			}
			if (gen is IMultiInlet multiInlet && gen is IOutlet<object>)
			{
				Type genericType = Generator.GetGenericType(gen);
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					if (IsLinked(item) && Generator.GetGenericType(item) == genericType)
					{
						inlet = item;
					}
				}
			}
			if (gen is IInlet<object> && gen is IMultiOutlet multiOutlet)
			{
				Type genericType2 = Generator.GetGenericType(gen);
				foreach (IOutlet<object> item2 in multiOutlet.Outlets())
				{
					if (Generator.GetGenericType(item2) == genericType2)
					{
						outlet = item2;
					}
				}
			}
			if (inlet == null || outlet == null)
			{
				return;
			}
			List<IInlet<object>> list = LinkedInlets(outlet);
			if (list.Count == 0 || !links.TryGetValue(inlet, out var value))
			{
				return;
			}
			foreach (IInlet<object> item3 in list)
			{
				Link(value, item3);
			}
		}

		public void AutoLink(Generator gen, IOutlet<object> outlet)
		{
			Type genericType = Generator.GetGenericType(outlet);
			if (gen is IInlet<object> inlet)
			{
				if (Generator.GetGenericType(inlet) == genericType)
				{
					Link(outlet, inlet);
				}
			}
			else
			{
				if (!(gen is IMultiInlet multiInlet))
				{
					return;
				}
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					if (Generator.GetGenericType(item) == genericType)
					{
						Link(outlet, item);
						break;
					}
				}
			}
		}

		public IEnumerable<Generator> GetGenerators(Predicate<Generator> predicate)
		{
			int i = -1;
			for (int g = 0; g < generators.Length; g++)
			{
				i = Array.FindIndex(generators, i + 1, predicate);
				if (i >= 0)
				{
					yield return generators[i];
					continue;
				}
				break;
			}
		}

		public Generator GetGenerator(Predicate<Generator> predicate)
		{
			int num = Array.FindIndex(generators, predicate);
			if (num >= 0)
			{
				return generators[num];
			}
			return null;
		}

		public Generator GetGeneratorById(ulong id)
		{
			for (int i = 0; i < generators.Length; i++)
			{
				if (generators[i].id == id)
				{
					return generators[i];
				}
			}
			return null;
		}

		public IEnumerable<T> GeneratorsOfType<T>()
		{
			for (int g = 0; g < generators.Length; g++)
			{
				Generator generator = generators[g];
				if (generator is T)
				{
					yield return (T)(object)((generator is T) ? generator : null);
				}
			}
		}

		public int GeneratorsCount(Predicate<Generator> predicate)
		{
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < generators.Length; i++)
			{
				num2 = Array.FindIndex(generators, num2 + 1, predicate);
				if (num2 >= 0)
				{
					num++;
				}
			}
			return num;
		}

		public bool ContainsGenerator(Generator gen)
		{
			return GetGenerator((Generator g) => g == gen) != null;
		}

		public bool ContainsGeneratorOfType<T>()
		{
			return GetGenerator((Generator g) => g is T) != null;
		}

		public int GeneratorsCount<T>() where T : class
		{
			return GeneratorsCount(findByType);
			static bool findByType(Generator g)
			{
				return g is T;
			}
		}

		public IEnumerable<IUnit> AllUnits()
		{
			Generator[] array = generators;
			foreach (Generator gen in array)
			{
				yield return gen;
				if (gen is IMultiLayer multiLayer)
				{
					foreach (IUnit layer in multiLayer.Layers)
					{
						yield return layer;
					}
				}
				if (gen is IMultiInlet multiInlet)
				{
					foreach (IInlet<object> item in multiInlet.Inlets())
					{
						yield return item;
					}
				}
				if (!(gen is IMultiOutlet multiOutlet))
				{
					continue;
				}
				foreach (IOutlet<object> item2 in multiOutlet.Outlets())
				{
					yield return item2;
				}
			}
		}

		public IEnumerable<T> UnitsOfType<T>()
		{
			Generator[] array = generators;
			foreach (Generator gen in array)
			{
				if (gen is T)
				{
					yield return (T)(object)((gen is T) ? gen : null);
				}
				if (gen is IMultiLayer multiLayer)
				{
					foreach (IUnit layer in multiLayer.Layers)
					{
						if (layer is T)
						{
							yield return (T)layer;
						}
					}
				}
				if (gen is IMultiInlet multiInlet)
				{
					foreach (IInlet<object> item in multiInlet.Inlets())
					{
						if (item is T)
						{
							yield return (T)item;
						}
					}
				}
				if (!(gen is IMultiOutlet multiOutlet))
				{
					continue;
				}
				foreach (IOutlet<object> item2 in multiOutlet.Outlets())
				{
					if (item2 is T)
					{
						yield return (T)item2;
					}
				}
			}
		}

		public ulong IdsVersions()
		{
			ulong num = 0uL;
			ulong num2 = 0uL;
			Generator[] array = generators;
			foreach (Generator generator in array)
			{
				num += generator.id;
				num2 += generator.version;
				if (!(generator is IMultiLayer multiLayer))
				{
					continue;
				}
				foreach (IUnit layer in multiLayer.Layers)
				{
					num += layer.Id;
				}
			}
			return (num << 16) + num2;
		}

		public string IdsVersionsHash()
		{
			ulong num = IdsVersions();
			RuntimeHelpers.GetHashCode(num);
			return Convert.ToBase64String(BitConverter.GetBytes(num));
		}

		public IEnumerable<Graph> SubGraphs(bool recursively = false)
		{
			foreach (IBiome item in UnitsOfType<IBiome>())
			{
				Graph subGraph = item.SubGraph;
				if (subGraph == null)
				{
					continue;
				}
				yield return item.SubGraph;
				if (!recursively)
				{
					continue;
				}
				foreach (Graph item2 in subGraph.SubGraphs(recursively: true))
				{
					yield return item2;
				}
			}
		}

		public bool ContainsSubGraph(Graph subGraph, bool recursively = false)
		{
			foreach (IBiome item in UnitsOfType<IBiome>())
			{
				Graph subGraph2 = item.SubGraph;
				if (!(subGraph2 == null))
				{
					if (subGraph2 == subGraph)
					{
						return true;
					}
					if (recursively && subGraph2.ContainsSubGraph(subGraph, recursively: true))
					{
						return true;
					}
				}
			}
			return false;
		}

		public IEnumerable<Generator> RelevantGenerators(bool isDraft)
		{
			for (int g = 0; g < generators.Length; g++)
			{
				if (IsRelevant(generators[g], isDraft))
				{
					yield return generators[g];
				}
			}
		}

		public bool IsRelevant(Generator gen, bool isDraft)
		{
			if (gen is OutputGenerator outputGenerator)
			{
				if (isDraft && outputGenerator.OutputLevel.HasFlag(OutputLevel.Draft))
				{
					return true;
				}
				if (!isDraft && outputGenerator.OutputLevel.HasFlag(OutputLevel.Main))
				{
					return true;
				}
			}
			else
			{
				if (gen is IRelevant)
				{
					return true;
				}
				if (gen is IBiome biome && biome.SubGraph != null)
				{
					return true;
				}
				if (gen is IMultiLayer multiLayer)
				{
					foreach (IUnit layer in multiLayer.Layers)
					{
						if (layer is IBiome)
						{
							return true;
						}
					}
				}
				else if (gen.guiPreview)
				{
					return true;
				}
			}
			return false;
		}

		public bool ClearChanged(TileData data, bool totalRebuild = false)
		{
			OnBeforeClearPrepareGenerate?.Invoke(this, data);
			RefreshInputHashIds();
			data.ClearStray(this);
			Dictionary<Generator, bool> processed = new Dictionary<Generator, bool>();
			bool flag = true;
			foreach (Generator item in RelevantGenerators(data.isDraft))
			{
				flag &= ClearChangedRecursive(item, data, processed, totalRebuild);
			}
			return flag;
		}

		private bool ClearChangedRecursive(Generator gen, TileData data, Dictionary<Generator, bool> processed, bool totalRebuild = false)
		{
			if (processed.TryGetValue(gen, out var value))
			{
				return value;
			}
			bool isReady = data.IsReady(gen);
			if (gen is IInlet<object> key && links.TryGetValue(key, out var value2))
			{
				Generator gen2 = value2.Gen;
				if (!processed.TryGetValue(gen, out var value3))
				{
					value3 = ClearChangedRecursive(gen2, data, processed, totalRebuild);
				}
				isReady = isReady && value3;
			}
			if (gen is IMultiInlet multiInlet)
			{
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					if (links.TryGetValue(item, out var value4))
					{
						Generator gen3 = value4.Gen;
						if (!processed.TryGetValue(gen, out var value5))
						{
							value5 = ClearChangedRecursive(gen3, data, processed, totalRebuild);
						}
						isReady = isReady && value5;
					}
				}
			}
			if (gen is ICustomDependence customDependence)
			{
				foreach (Generator item2 in customDependence.PriorGens())
				{
					ClearChangedRecursive(item2, data, processed, totalRebuild);
					if (!data.IsReady(item2))
					{
						isReady = false;
						break;
					}
				}
			}
			if (gen is ICustomClear customClear)
			{
				customClear.OnClearing(this, data, ref isReady, totalRebuild);
			}
			if (!isReady)
			{
				data.ClearReady(gen);
			}
			processed.Add(gen, isReady);
			return isReady;
		}

		public void Prepare(TileData data, Terrain terrain, Override ovd = null)
		{
			OnBeforeClearPrepareGenerate?.Invoke(this, data);
			if (ovd == null)
			{
				ovd = defaults;
			}
			Generator[] array = generators;
			foreach (Generator generator in array)
			{
				if (generator is IPrepare && !data.IsReady(generator))
				{
					Generator generator2 = generator;
					if (Assigner.IsExposed(generator, exposed))
					{
						generator2 = (Generator)Assigner.CopyAndAssign(generator, exposed, ovd);
					}
					((IPrepare)generator2).Prepare(data, terrain);
				}
			}
		}

		public void PrepareRecursive(Generator gen, TileData data, Terrain terrain, Override ovd, HashSet<Generator> processed)
		{
			if (processed.Contains(gen))
			{
				return;
			}
			if (gen is IInlet<object> key && links.TryGetValue(key, out var value))
			{
				PrepareRecursive(value.Gen, data, terrain, ovd, processed);
			}
			if (gen is IMultiInlet multiInlet)
			{
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					if (links.TryGetValue(item, out var value2))
					{
						PrepareRecursive(value2.Gen, data, terrain, ovd, processed);
					}
				}
			}
			if (!(gen is ICustomDependence customDependence))
			{
				return;
			}
			foreach (Generator item2 in customDependence.PriorGens())
			{
				PrepareRecursive(item2, data, terrain, ovd, processed);
			}
		}

		public void Generate(TileData data, StopToken stop = null, Override ovd = null)
		{
			OnBeforeClearPrepareGenerate?.Invoke(this, data);
			RefreshInputHashIds();
			if (ovd == null)
			{
				ovd = defaults;
			}
			foreach (Generator item in RelevantGenerators(data.isDraft))
			{
				if (stop != null && stop.stop)
				{
					break;
				}
				GenerateRecursive(item, data, ovd, stop);
			}
		}

		public void GenerateRecursive(Generator gen, TileData data, Override ovd, StopToken stop = null)
		{
			if ((stop != null && stop.stop) || data.IsReady(gen))
			{
				return;
			}
			_ = gen.version;
			if (gen is IInlet<object> key && links.TryGetValue(key, out var value))
			{
				GenerateRecursive(value.Gen, data, ovd, stop);
			}
			if (gen is IMultiInlet multiInlet)
			{
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					if (links.TryGetValue(item, out var value2))
					{
						GenerateRecursive(value2.Gen, data, ovd, stop);
					}
				}
			}
			if (gen is ICustomDependence customDependence)
			{
				foreach (Generator item2 in customDependence.PriorGens())
				{
					GenerateRecursive(item2, data, ovd, stop);
				}
			}
			if (stop != null && stop.stop)
			{
				return;
			}
			if (data.IsReady(gen))
			{
				throw new Exception(string.Format("Generating twice {0}, id: {1}, draft: {2}, stop: {3}", gen, Id.ToString(gen.id), data.isDraft, (stop != null) ? stop.stop.ToString() : "null"));
			}
			if (gen is IMultiLayer multiLayer)
			{
				foreach (IUnit layer in multiLayer.Layers)
				{
					if (layer.Gen == null)
					{
						layer.SetGen(gen);
					}
					if (layer.Id == 0L)
					{
						layer.Id = Id.Generate();
					}
				}
			}
			Stopwatch.GetTimestamp();
			if (stop == null || !stop.stop)
			{
				Generator generator = gen;
				if (Assigner.IsExposed(gen, exposed))
				{
					generator = (Generator)Assigner.CopyAndAssign(gen, exposed, ovd);
				}
				generator.Generate(data, stop);
				if (stop == null || !stop.stop)
				{
					data.MarkReady(gen);
					OnAfterNodeGenerated?.Invoke(gen, data);
				}
			}
		}

		public void Finalize(TileData data, StopToken stop = null)
		{
			if (stop != null && stop.stop)
			{
				data.ClearFinalize();
				return;
			}
			while (data.FinalizeMarksCount > 0)
			{
				FinalizeAction finalizeAction = data.DequeueFinalize();
				if (stop != null && stop.stop)
				{
					data.ClearFinalize();
					break;
				}
				finalizeAction(data, stop);
			}
		}

		[Obsolete]
		private IEnumerator Apply(TileData data, Terrain terrain, StopToken stop = null)
		{
			if (stop != null && stop.stop)
			{
				yield break;
			}
			while (data.ApplyMarksCount != 0)
			{
				IApplyData applyData = data.DequeueApply();
				if (applyData is IApplyDataRoutine)
				{
					IEnumerator e = ((IApplyDataRoutine)applyData).ApplyRoutine(terrain);
					while (e.MoveNext())
					{
						if (stop != null && stop.stop)
						{
							yield break;
						}
						yield return null;
					}
				}
				else
				{
					applyData.Apply(terrain);
					yield return null;
				}
			}
		}

		public void Purge(Type type, TileData data, Terrain terrain)
		{
			for (int i = 0; i < generators.Length; i++)
			{
				if (generators[i] is OutputGenerator outputGenerator)
				{
					Type type2 = outputGenerator.GetType();
					if (type2 == type || type.IsAssignableFrom(type2))
					{
						outputGenerator.ClearApplied(data, terrain);
					}
				}
			}
			foreach (Graph item in SubGraphs())
			{
				item.Purge(type, data, terrain);
			}
		}

		private void RefreshInputHashIds()
		{
			foreach (KeyValuePair<IInlet<object>, IOutlet<object>> link in links)
			{
				IInlet<object> key = link.Key;
				IOutlet<object> value = link.Value;
				key.LinkedOutletId = value.Id;
				key.LinkedGenId = value.Gen.id;
			}
			foreach (IUnit item in AllUnits())
			{
				if (item is IInlet<object> inlet && !links.ContainsKey(inlet))
				{
					inlet.LinkedOutletId = 0uL;
				}
			}
		}

		public static IEnumerable<(Graph, TileData)> AllGraphsDatas(Graph rootGraph, TileData rootData, bool includeSelf = false)
		{
			if (includeSelf)
			{
				yield return (rootGraph, rootData);
			}
			foreach (IBiome item in rootGraph.UnitsOfType<IBiome>())
			{
				Graph subGraph = item.SubGraph;
				if (subGraph == null)
				{
					continue;
				}
				TileData tileData = item.SubData(rootData);
				if (tileData == null)
				{
					continue;
				}
				foreach (var item2 in AllGraphsDatas(subGraph, tileData))
				{
					yield return item2;
				}
			}
		}

		public float GetGenerateComplexity()
		{
			float num = 0f;
			for (int i = 0; i < generators.Length; i++)
			{
				num = ((!(generators[i] is ICustomComplexity)) ? (num + 1f) : (num + ((ICustomComplexity)generators[i]).Complexity));
			}
			return num;
		}

		public float GetGenerateProgress(TileData data)
		{
			float num = 0f;
			for (int i = 0; i < generators.Length; i++)
			{
				if (generators[i] is ICustomComplexity)
				{
					num += ((ICustomComplexity)generators[i]).Progress(data);
				}
				else if (data.IsReady(generators[i]))
				{
					num += 1f;
				}
			}
			return num;
		}

		public float GetApplyComplexity()
		{
			return GetAllOutputTypes().Count;
		}

		private HashSet<Type> GetAllOutputTypes(HashSet<Type> outputTypes = null)
		{
			if (outputTypes == null)
			{
				outputTypes = new HashSet<Type>();
			}
			for (int i = 0; i < generators.Length; i++)
			{
				if (generators[i] is OutputGenerator)
				{
					Type type = generators[i].GetType();
					if (!outputTypes.Contains(type))
					{
						outputTypes.Add(type);
					}
				}
			}
			foreach (Graph item in SubGraphs())
			{
				item.GetAllOutputTypes(outputTypes);
			}
			return outputTypes;
		}

		public float GetApplyProgress(TileData data)
		{
			return data.ApplyMarksCount;
		}

		public void OnBeforeSerialize()
		{
			if (generators == null)
			{
				OnAfterDeserialize();
			}
			if (generators == null)
			{
				throw new Exception("Could not save graph data, node array is null. Check if graph was loaded successfully");
			}
			if (serializer200beta == null)
			{
				serializer200beta = new GraphSerializer200Beta();
			}
			serializer200beta.Serialize(this);
		}

		public void OnAfterDeserialize()
		{
			if (serializer200beta != null)
			{
				serializer200beta.Deserialize(this);
			}
		}

		public static void DeepCopy(Graph src, Graph dst)
		{
			dst.name = src.name;
			if (dst.serializer200beta == null)
			{
				dst.serializer200beta = new GraphSerializer200Beta();
			}
			dst.serializer200beta.Serialize(src);
			dst.serializer200beta.Deserialize(dst);
		}

		public string DebugUnitName(ulong id, bool useGraphName = true, string prevGraphNames = null)
		{
			foreach (IUnit item in AllUnits())
			{
				if (item.Id == id)
				{
					string text = DebugUnitName(item);
					if (useGraphName)
					{
						text = text + " graph:" + base.name;
					}
					if (prevGraphNames != null)
					{
						text = text + " prev:" + prevGraphNames;
					}
					return text;
				}
			}
			int num = 0;
			foreach (IUnit item2 in AllUnits())
			{
				if (item2 is IBiome biome)
				{
					string prevGraphNames2 = ((!useGraphName) ? $"(b:{num})" : $"{prevGraphNames}.{base.name} (b:{num})");
					string text2 = biome.SubGraph.DebugUnitName(id, useGraphName, prevGraphNames2);
					if (text2 != null)
					{
						return text2;
					}
					num++;
				}
			}
			return null;
		}

		public string[] DebugAllUnits(int subLevel = 0)
		{
			List<string> list = new List<string>();
			foreach (IUnit item in AllUnits())
			{
				list.Add(DebugUnitName(item) + " sub:" + subLevel);
			}
			foreach (IUnit item2 in AllUnits())
			{
				if (item2 is IBiome biome)
				{
					list.AddRange(biome.SubGraph.DebugAllUnits(subLevel + 1));
				}
			}
			return list.ToArray();
		}

		public string DebugUnitName(IUnit unit)
		{
			string text = unit.GetType().Namespace + "." + unit.GetType().Name;
			string text2 = unit.Gen.GetType().Namespace + "." + unit.Gen.GetType().Name;
			int num = -1;
			for (int i = 0; i < generators.Length; i++)
			{
				if (generators[i] == unit.Gen)
				{
					num = i;
					break;
				}
			}
			int num2 = -1;
			int num3 = 0;
			if (unit.Gen != unit && unit.Gen is IMultiLayer multiLayer)
			{
				foreach (IUnit layer in multiLayer.Layers)
				{
					if (layer == unit)
					{
						num2 = num3;
						break;
					}
					num3++;
				}
			}
			int num4 = -1;
			int num5 = 0;
			if (unit.Gen != unit && unit.Gen is IMultiInlet multiInlet)
			{
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					if (item == unit)
					{
						num4 = num5;
						break;
					}
					num5++;
				}
			}
			int num6 = -1;
			int num7 = 0;
			if (unit.Gen != unit && unit.Gen is IMultiOutlet multiOutlet)
			{
				foreach (IOutlet<object> item2 in multiOutlet.Outlets())
				{
					if (item2 == unit)
					{
						num6 = num7;
						break;
					}
					num7++;
				}
			}
			return $"unit:{text}, gen:{text2}, id:{Id.ToString(unit.Id)}, g:{num}, l:{num2}, i:{num4}, o:{num6}";
		}
	}
}

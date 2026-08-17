using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Core;
using MapMagic.Nodes;
using MapMagic.Nodes.MatrixGenerators;
using MapMagic.Terrains;
using UnityEngine;

namespace MapMagic.Products
{
	public class TileData
	{
		public Area area;

		public Globals globals;

		public Noise random;

		public ulong graphChangeVersion;

		public int graphId;

		public bool isPreview;

		public bool isDraft;

		private Dictionary<ulong, ulong> lastVersion = new Dictionary<ulong, ulong>();

		private Dictionary<ulong, float> progress = new Dictionary<ulong, float>();

		private Dictionary<ulong, object> prepare = new Dictionary<ulong, object>();

		private Dictionary<ulong, object> products = new Dictionary<ulong, object>();

		private Dictionary<ulong, (object, object, object)> outputs = new Dictionary<ulong, (object, object, object)>();

		private HashSet<FinalizeAction> finalizeMarks = new HashSet<FinalizeAction>();

		private Dictionary<Type, IApplyData> applyMarks = new Dictionary<Type, IApplyData>();

		public MatrixWorld heights;

		public Dictionary<ulong, TileData> subDatas = new Dictionary<ulong, TileData>();

		public TileData parentData;

		public MatrixWorld biomeMask;

		public TileData Root
		{
			get
			{
				if (parentData != null)
				{
					return parentData.Root;
				}
				return this;
			}
		}

		public string[] DebugReadyVar => DebugReady(Graph.debugGraph, useGraphName: false);

		public int PrepareCount => prepare.Count;

		public int ProductsCount => products.Count;

		public string[] DebugProductsVar => DebugProducts(Graph.debugGraph, useGraphName: false);

		public int FinalizeMarksCount => finalizeMarks.Count;

		public int ApplyMarksCount => applyMarks.Count;

		public TileData CreateLoadSubData(ulong biomeId, MatrixWorld mask)
		{
			if (!subDatas.TryGetValue(biomeId, out var value))
			{
				value = (TileData)MemberwiseClone();
				value.lastVersion = new Dictionary<ulong, ulong>();
				value.products = new Dictionary<ulong, object>();
				value.prepare = new Dictionary<ulong, object>();
				value.products = new Dictionary<ulong, object>();
				value.outputs = new Dictionary<ulong, (object, object, object)>();
				value.subDatas = new Dictionary<ulong, TileData>();
				value.parentData = this;
				subDatas.Add(biomeId, value);
			}
			value.biomeMask = mask;
			if (value.area == null)
			{
				value.area = area;
			}
			if (value.globals == null)
			{
				value.globals = globals;
			}
			if (value.random == null)
			{
				value.random = random;
			}
			return value;
		}

		public TileData CreateLoadSubData(ulong biomeId)
		{
			return CreateLoadSubData(biomeId, biomeMask);
		}

		public TileData GetSubData(ulong biomeId)
		{
			subDatas.TryGetValue(biomeId, out var value);
			return value;
		}

		public IEnumerable<TileData> AllSubs(bool includeItself = false)
		{
			foreach (KeyValuePair<ulong, TileData> subData in subDatas)
			{
				yield return subData.Value;
			}
			foreach (KeyValuePair<ulong, TileData> subData2 in subDatas)
			{
				TileData value = subData2.Value;
				foreach (TileData item in value.AllSubs())
				{
					yield return item;
				}
			}
		}

		public bool IsReady(ulong genId, ulong version)
		{
			if (lastVersion.TryGetValue(genId, out var value))
			{
				return version == value;
			}
			return false;
		}

		public bool IsReady(Generator gen)
		{
			if (lastVersion.TryGetValue(gen.id, out var value))
			{
				return gen.version == value;
			}
			return false;
		}

		public bool VersionChanged(Generator gen)
		{
			if (lastVersion.TryGetValue(gen.id, out var value))
			{
				return true;
			}
			return gen.version == value;
		}

		public void MarkReady(ulong genId, ulong version)
		{
			if (!lastVersion.ContainsKey(genId))
			{
				lastVersion.Add(genId, version);
			}
			else
			{
				lastVersion[genId] = version;
			}
		}

		public void MarkReady(Generator gen)
		{
			if (!lastVersion.ContainsKey(gen.id))
			{
				lastVersion.Add(gen.id, gen.version);
			}
			else
			{
				lastVersion[gen.id] = gen.version;
			}
		}

		public void ClearReady(ulong genId)
		{
			lastVersion.Remove(genId);
		}

		public void ClearReady(Generator gen)
		{
			lastVersion.Remove(gen.id);
		}

		public bool AreAllRelevantReady(Graph graph, bool inSubs = false)
		{
			foreach (Generator item in graph.RelevantGenerators(isDraft))
			{
				if (!lastVersion.TryGetValue(item.id, out var value))
				{
					return false;
				}
				if (item.version != value)
				{
					return false;
				}
			}
			if (inSubs)
			{
				foreach (IBiome item2 in graph.UnitsOfType<IBiome>())
				{
					Graph subGraph = item2.SubGraph;
					if (!(subGraph == null) && subDatas.TryGetValue(item2.Id, out var value2) && !value2.AreAllRelevantReady(subGraph, inSubs))
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool AllOutputsReady(Graph graph, OutputLevel level, bool inSubs = false)
		{
			foreach (OutputGenerator item in graph.GeneratorsOfType<OutputGenerator>())
			{
				if (item.enabled && item.OutputLevel.HasFlag(level))
				{
					if (!lastVersion.TryGetValue(item.id, out var value))
					{
						return false;
					}
					if (item.version != value)
					{
						return false;
					}
				}
			}
			if (inSubs)
			{
				foreach (IBiome item2 in graph.UnitsOfType<IBiome>())
				{
					Graph subGraph = item2.SubGraph;
					if (subGraph == null)
					{
						continue;
					}
					TileData value2;
					if (isDraft)
					{
						if (!IsReady(item2.Gen))
						{
							return false;
						}
					}
					else if (subDatas.TryGetValue(item2.Id, out value2) && !value2.AllOutputsReady(subGraph, level, inSubs))
					{
						return false;
					}
				}
			}
			return true;
		}

		public string[] DebugReady(Graph rootGraph, bool useGraphName = true)
		{
			string[] array = new string[lastVersion.Count];
			int num = 0;
			foreach (ulong key in lastVersion.Keys)
			{
				array[num] = rootGraph.DebugUnitName(key, useGraphName);
				if (array[num] == null)
				{
					array[num] = "Null id:" + Id.ToString(key);
				}
				num++;
			}
			return array;
		}

		public void SetProgress(ICustomComplexity cc, float progress)
		{
			Generator generator = (Generator)cc;
			if (this.progress.ContainsKey(generator.id))
			{
				this.progress[generator.id] = progress;
			}
			else
			{
				this.progress.Add(generator.id, progress);
			}
		}

		public float GetProgress(ICustomComplexity cc)
		{
			Generator generator = (Generator)cc;
			if (progress.TryGetValue(generator.id, out var value))
			{
				return value;
			}
			return 0f;
		}

		public T ReadPrepare<T>(Generator gen) where T : class
		{
			return (T)ReadPrepare(gen.id);
		}

		public object ReadPrepare(ulong genId)
		{
			if (prepare.TryGetValue(genId, out var value))
			{
				return value;
			}
			return null;
		}

		public void StorePrepare(ulong genId, object obj)
		{
			if (obj == null)
			{
				RemovePrepare(genId);
			}
			if (prepare.ContainsKey(genId))
			{
				prepare[genId] = obj;
			}
			else
			{
				prepare.Add(genId, obj);
			}
		}

		public void RemovePrepare(Generator gen)
		{
			RemoveProduct(gen.id);
		}

		public void RemovePrepare(ulong genId)
		{
			prepare.Remove(genId);
		}

		public void ClearPrepare()
		{
			prepare.Clear();
		}

		public T ReadInletProduct<T>(IInlet<T> inlet) where T : class
		{
			return (T)ReadProduct(inlet.LinkedOutletId);
		}

		public T ReadOutletProduct<T>(IOutlet<T> outlet) where T : class
		{
			return (T)ReadProduct(outlet.Id);
		}

		public object ReadProduct(ulong outletId)
		{
			if (outletId == 0L)
			{
				return null;
			}
			if (products.TryGetValue(outletId, out var value))
			{
				return value;
			}
			return null;
		}

		public void StoreProduct<T>(IOutlet<T> outlet, T product) where T : class
		{
			StoreProduct(outlet.Id, product);
		}

		public void StoreInletProduct(IInlet<object> inlet, object product)
		{
			StoreProduct(inlet.Id, product);
		}

		public void StoreProduct(ulong outletId, object obj)
		{
			if (obj == null)
			{
				RemoveProduct(outletId);
			}
			if (products.ContainsKey(outletId))
			{
				products[outletId] = obj;
			}
			else
			{
				products.Add(outletId, obj);
			}
		}

		public void RemoveProduct(IOutlet<object> outlet)
		{
			RemoveProduct(outlet.Id);
		}

		public void RemoveProduct(ulong outletId)
		{
			products.Remove(outletId);
		}

		public void ClearProducts()
		{
			products.Clear();
		}

		public string[] DebugProducts(Graph rootGraph, bool useGraphName = true)
		{
			string[] array = new string[products.Count];
			int num = 0;
			foreach (ulong key in products.Keys)
			{
				string text = rootGraph.DebugUnitName(key, useGraphName);
				if (text == null)
				{
					text = "Null";
				}
				array[num] = text + " product:" + products[key];
				num++;
			}
			return array;
		}

		public void StoreOutput(IUnit unit, object key, object prototype, object product)
		{
			StoreOutput(unit.Id, key, prototype, product);
		}

		public void StoreOutput(ulong outputId, object key, object prototype, object obj)
		{
			if (outputs.ContainsKey(outputId))
			{
				outputs[outputId] = (key, prototype, obj);
			}
			else
			{
				outputs.Add(outputId, (key, prototype, obj));
			}
		}

		public IEnumerable<(TPrototype, TProduct, MatrixWorld)> Outputs<TPrototype, TProduct, TMask>(object key, bool inSubs = false)
		{
			foreach (var (obj, obj2, item) in Outputs(key, inSubs))
			{
				yield return ((TPrototype)obj, (TProduct)obj2, item);
			}
		}

		public void GatherOutputs<TPrototype, TProduct>(object key, out TPrototype[] prototypes, out TProduct[] products, out MatrixWorld[] masks, bool inSubs = false) where TProduct : class
		{
			List<TPrototype> list = new List<TPrototype>();
			List<TProduct> list2 = new List<TProduct>();
			List<MatrixWorld> list3 = new List<MatrixWorld>();
			foreach (var (obj, obj2, item) in Outputs(key, inSubs))
			{
				list.Add((TPrototype)obj);
				list2.Add((TProduct)obj2);
				list3.Add(item);
			}
			prototypes = list.ToArray();
			products = list2.ToArray();
			masks = list3.ToArray();
		}

		public IEnumerable<(object prototype, object product, MatrixWorld biomeMask)> Outputs(object key, bool inSubs = false)
		{
			foreach (KeyValuePair<ulong, (object, object, object)> output in outputs)
			{
				var (obj, item, item2) = output.Value;
				if (obj == key)
				{
					yield return (prototype: item, product: item2, biomeMask: biomeMask);
				}
			}
			if (!inSubs)
			{
				yield break;
			}
			foreach (TileData value in subDatas.Values)
			{
				foreach (var item3 in value.Outputs(key, inSubs: true))
				{
					yield return item3;
				}
			}
		}

		public int OutputsCount(object key, bool inSubs = false)
		{
			int num = 0;
			foreach (KeyValuePair<ulong, (object, object, object)> output in outputs)
			{
				object item = output.Value.Item1;
				if (item == key)
				{
					num++;
				}
			}
			if (inSubs)
			{
				foreach (KeyValuePair<ulong, TileData> subData in subDatas)
				{
					num += subData.Value.OutputsCount(key, inSubs: true);
				}
			}
			return num;
		}

		public void RemoveOutput(ulong id, bool inSubs = false)
		{
			outputs.Remove(id);
			if (!inSubs)
			{
				return;
			}
			foreach (KeyValuePair<ulong, TileData> subData in subDatas)
			{
				subData.Value.RemoveOutput(id, inSubs);
			}
		}

		public void MarkFinalize(FinalizeAction action, StopToken stop)
		{
			if (!finalizeMarks.Contains(action))
			{
				finalizeMarks.Add(action);
			}
		}

		public void RemoveFinalize(FinalizeAction action)
		{
			finalizeMarks.Remove(action);
		}

		public void ClearFinalize()
		{
			finalizeMarks.Clear();
		}

		public IEnumerable<FinalizeAction> MarkedFinalizeActions(StopToken stop)
		{
			lock (finalizeMarks)
			{
				FinalizeAction heightAction = HeightOutput200.finalizeAction;
				if (finalizeMarks.Contains(heightAction))
				{
					yield return heightAction;
				}
				foreach (FinalizeAction finalizeMark in finalizeMarks)
				{
					Debug.Log("Iterating " + finalizeMarks.GetVersion() + " stop:" + stop.stop);
					if (finalizeMark != heightAction)
					{
						yield return finalizeMark;
					}
				}
			}
		}

		public FinalizeAction DequeueFinalize()
		{
			FinalizeAction finalizeAction = HeightOutput200.finalizeAction;
			if (finalizeMarks.Contains(finalizeAction))
			{
				finalizeMarks.Remove(finalizeAction);
				return finalizeAction;
			}
			FinalizeAction finalizeAction2 = null;
			using (HashSet<FinalizeAction>.Enumerator enumerator = finalizeMarks.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					finalizeAction2 = enumerator.Current;
				}
			}
			finalizeMarks.Remove(finalizeAction2);
			return finalizeAction2;
		}

		public void MarkApply(IApplyData data)
		{
			Type type = data.GetType();
			lock (applyMarks)
			{
				if (applyMarks.ContainsKey(type))
				{
					applyMarks[type] = data;
				}
				else
				{
					applyMarks.Add(type, data);
				}
			}
		}

		public T ApplyOfType<T>() where T : class, IApplyData
		{
			if (applyMarks.TryGetValue(typeof(T), out var value))
			{
				return (T)value;
			}
			return null;
		}

		public IApplyData DequeueApply()
		{
			IApplyData applyData = null;
			int num = -1;
			lock (applyMarks)
			{
				foreach (IApplyData value in applyMarks.Values)
				{
					int applyPriority = GetApplyPriority(value);
					if (applyPriority > num)
					{
						num = applyPriority;
						applyData = value;
					}
				}
			}
			applyMarks.Remove(applyData.GetType());
			return applyData;
		}

		private int GetApplyPriority(IApplyData data)
		{
			if (!(data is HeightOutput200.ApplySetData))
			{
				if (!(data is HeightOutput200.ApplySplitData))
				{
					if (!(data is HeightOutput200.ApplyTexData))
					{
						if (!(data is TexturesOutput200.ApplyData))
						{
							if (data is GrassOutput200.ApplyData)
							{
								return 6;
							}
							return 0;
						}
						return 7;
					}
					return 8;
				}
				return 9;
			}
			return 10;
		}

		public void Clear(bool clearApply = true, bool inSubs = false)
		{
			lastVersion.Clear();
			products.Clear();
			prepare.Clear();
			outputs.Clear();
			lock (finalizeMarks)
			{
				finalizeMarks.Clear();
			}
			if (clearApply)
			{
				applyMarks.Clear();
			}
			heights = null;
			if (!inSubs)
			{
				return;
			}
			foreach (TileData value in subDatas.Values)
			{
				value.Clear(clearApply, inSubs);
			}
			subDatas.Clear();
		}

		public void Remove(Generator gen, bool inSubs = false)
		{
			lastVersion.Remove(gen.id);
			products.Remove(gen.id);
			prepare.Remove(gen.id);
			outputs.Remove(gen.id);
			if (gen is IMultiOutlet multiOutlet)
			{
				foreach (IOutlet<object> item in multiOutlet.Outlets())
				{
					foreach (TileData item2 in AllSubs())
					{
						item2.products.Remove(item.Id);
					}
				}
			}
			if (!inSubs)
			{
				return;
			}
			foreach (TileData value in subDatas.Values)
			{
				value.Remove(gen, inSubs);
			}
		}

		public void ClearStray(Graph graph, bool inSubs = false)
		{
			HashSet<ulong> hashSet = new HashSet<ulong>();
			foreach (IUnit item in graph.AllUnits())
			{
				hashSet.Add(item.Id);
			}
			lastVersion.RemoveNotContained(hashSet);
			products.RemoveNotContained(hashSet);
			prepare.RemoveNotContained(hashSet);
			outputs.RemoveNotContained(hashSet);
			if (!inSubs)
			{
				return;
			}
			foreach (var item2 in Graph.AllGraphsDatas(graph, this))
			{
				var (graph2, _) = item2;
				item2.Item2.ClearStray(graph2, inSubs);
			}
		}
	}
}

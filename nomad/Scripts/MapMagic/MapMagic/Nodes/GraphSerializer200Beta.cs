using System;
using System.Collections.Generic;
using Den.Tools;
using MapMagic.Core;
using MapMagic.Expose;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	public class GraphSerializer200Beta : IGraphSerializer
	{
		private class SerializedDataProt
		{
			public SemVer ver;

			public Generator[] generators;

			public IInlet<object>[] linkInlets;

			public IOutlet<object>[] linkOutlets;

			public Auxiliary[] groups;

			public int seed = 12345;

			public Exposed exposed = new Exposed();

			public Override defaults = new Override();
		}

		public Serializer.Object[] serializedData = new Serializer.Object[0];

		private static readonly Dictionary<string, string> classRenames = new Dictionary<string, string>
		{
			{ "Plugins.", "Den.Tools." },
			{ "MapMagic.Nodes.ObjectsGenerators.PositioningSettings", "MapMagic.Nodes.PositioningSettings" },
			{ ", Tools, Version", ", Den.Tools, Version" },
			{ "MapMagic.Nodes.ObjectsGenerators.BiomeBlend", "MapMagic.Nodes.BiomeBlend" },
			{ "MapMagic.Nodes.MatrixGenerators.MicroSplatOutput200, Assembly-CSharp,", "MapMagic.Nodes.MatrixGenerators.MicroSplatOutput200, MapMagic.MicroSplat," },
			{ "MapMagic.Nodes.MatrixGenerators.MicroSplatOutput200, Assembly-CSharp-firstpass,", "MapMagic.Nodes.MatrixGenerators.MicroSplatOutput200, MapMagic.MicroSplat," }
		};

		public void Serialize(Graph graph)
		{
			Generator[] generators = graph.generators;
			for (int i = 0; i < generators.Length; i++)
			{
				if (generators[i] is ICustomSerialize customSerialize)
				{
					customSerialize.OnBeforeSerialize(graph);
				}
			}
			SerializedDataProt obj = new SerializedDataProt
			{
				ver = MapMagicObject.version,
				generators = graph.generators,
				linkInlets = graph.links.Keys.ToArray(),
				linkOutlets = graph.links.Values.ToArray(),
				groups = graph.groups,
				seed = graph.random.Seed,
				exposed = graph.exposed,
				defaults = graph.defaults
			};
			serializedData = Serializer.Serialize(obj, AfterSerialize);
		}

		public void Deserialize(Graph graph)
		{
			if (serializedData == null)
			{
				return;
			}
			Serializer.Deserialize(serializedData, BeforeDeserialize);
			SerializedDataProt serializedDataProt = (SerializedDataProt)Serializer.Deserialize(serializedData, BeforeDeserialize);
			CheckNullGenerators(serializedDataProt.generators);
			CheckNullLinks(ref serializedDataProt.linkInlets, ref serializedDataProt.linkOutlets);
			graph.generators = serializedDataProt.generators;
			graph.links = new Dictionary<IInlet<object>, IOutlet<object>>();
			graph.links.AddRange(serializedDataProt.linkInlets, serializedDataProt.linkOutlets);
			graph.groups = serializedDataProt.groups;
			graph.exposed = serializedDataProt.exposed;
			if (graph.exposed == null)
			{
				graph.exposed = new Exposed();
			}
			graph.defaults = serializedDataProt.defaults;
			if (graph.defaults == null)
			{
				graph.defaults = new Override();
			}
			graph.random = new Noise(serializedDataProt.seed, 32768);
			graph.CheckFixIds();
			CheckInletsGensAssigned(graph);
			Generator[] generators = graph.generators;
			for (int i = 0; i < generators.Length; i++)
			{
				if (generators[i] is ICustomSerialize customSerialize)
				{
					customSerialize.OnAfterDeserialize(graph);
				}
			}
		}

		public static void CheckInletsGensAssigned(Graph graph)
		{
			Generator[] generators = graph.generators;
			foreach (Generator generator in generators)
			{
				if (generator is IMultiInlet multiInlet)
				{
					foreach (IInlet<object> item in multiInlet.Inlets())
					{
						if (item.Gen == null)
						{
							Debug.Log($"Generator {generator} inlet Gen is not assigned. Fixing.");
							item.SetGen(generator);
						}
					}
				}
				if (!(generator is IMultiOutlet multiOutlet))
				{
					continue;
				}
				foreach (IOutlet<object> item2 in multiOutlet.Outlets())
				{
					if (item2.Gen == null)
					{
						Debug.Log($"Generator {generator} outlet Gen is not assigned. Fixing.");
						item2.SetGen(generator);
					}
				}
			}
		}

		private static void CheckNullGenerators(Generator[] gens)
		{
			for (int i = 0; i < gens.Length; i++)
			{
				if (gens[i] == null)
				{
					gens[i] = new Placeholders.Placeholder();
				}
			}
		}

		private static void CheckNullLinks(ref IInlet<object>[] inlets, ref IOutlet<object>[] outlets)
		{
			if (!inlets.ContainsNull() && !outlets.ContainsNull())
			{
				return;
			}
			List<IInlet<object>> list = new List<IInlet<object>>();
			List<IOutlet<object>> list2 = new List<IOutlet<object>>();
			int num = inlets.Length;
			for (int i = 0; i < num; i++)
			{
				if (inlets[i] != null && outlets[i] != null)
				{
					list.Add(inlets[i]);
					list2.Add(outlets[i]);
				}
			}
			inlets = list.ToArray();
			outlets = list2.ToArray();
		}

		private static void AfterSerialize(object obj, Serializer.Object serObj)
		{
			if (obj is Generator)
			{
				serObj.altType = typeof(Placeholders.InletOutletPlaceholder).AssemblyQualifiedName;
			}
		}

		private static string ModifyTypeName(string typeName)
		{
			if (typeName.StartsWith("MapMagic."))
			{
				typeName = typeName.Replace(", Assembly-CSharp, ", ", MapMagic, ");
			}
			if (typeName.StartsWith("Den.Tools."))
			{
				typeName = typeName.Replace(", Assembly-CSharp, ", ", Tools, ");
			}
			return typeName;
		}

		private static void BeforeDeserialize(Serializer.Object serObj)
		{
			if (serObj.type == "MapMagic.Nodes.Exposed, MapMagic, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
			{
				serObj.type = null;
			}
			if (serObj.type == null)
			{
				return;
			}
			foreach (KeyValuePair<string, string> classRename in classRenames)
			{
				if (serObj.type.Contains(classRename.Key))
				{
					serObj.type = serObj.type.Replace(classRename.Key, classRename.Value);
				}
			}
		}
	}
}

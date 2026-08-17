using System;
using System.Collections.Generic;
using MapMagic.Products;

namespace MapMagic.Nodes
{
	[Serializable]
	[GeneratorMenu(name = "Generic Portal Exit")]
	public class PortalExit<T> : Generator, IOutlet<T>, IUnit, IPortalExit<T>, ICustomDependence, ICustomSerialize where T : class, ICloneable
	{
		public PortalEnter<T> enter;

		[NonSerialized]
		public PortalEnter<T> tempEnter;

		public ulong enterId;

		public void OnBeforeSerialize(Graph graph)
		{
			if (tempEnter != null)
			{
				enterId = tempEnter.Id;
			}
			else
			{
				enterId = 0uL;
			}
		}

		public void OnAfterDeserialize(Graph graph)
		{
			RefreshEnter(graph);
		}

		public IPortalEnter<T> RefreshEnter(Graph graph)
		{
			if (enter != null)
			{
				tempEnter = enter;
				enterId = enter.id;
				enter = null;
			}
			if (tempEnter == null || tempEnter.id != enterId)
			{
				Generator generatorById = graph.GetGeneratorById(enterId);
				if (generatorById != null)
				{
					tempEnter = (PortalEnter<T>)generatorById;
				}
			}
			return tempEnter;
		}

		public void AssignEnter(IPortalEnter<object> ienter, Graph graph)
		{
			if (ienter == null)
			{
				tempEnter = null;
				enterId = 0uL;
			}
			else if (ienter is PortalEnter<T> portalEnter)
			{
				tempEnter = portalEnter;
				enterId = portalEnter.id;
			}
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (tempEnter != null && !stop.stop)
			{
				data.StoreProduct(this, data.ReadInletProduct(tempEnter));
			}
		}

		public IEnumerable<Generator> PriorGens()
		{
			if (tempEnter != null)
			{
				yield return tempEnter;
			}
		}
	}
}

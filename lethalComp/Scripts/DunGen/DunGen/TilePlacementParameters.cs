using System;
using DunGen.Graph;
using UnityEngine;

namespace DunGen
{
	[Serializable]
	public class TilePlacementParameters
	{
		[SerializeField]
		private DungeonArchetype archetype;

		[SerializeField]
		private GraphNode node;

		[SerializeField]
		private GraphLine line;

		public DungeonArchetype Archetype
		{
			get
			{
				return archetype;
			}
			internal set
			{
				archetype = value;
			}
		}

		public GraphNode Node
		{
			get
			{
				return node;
			}
			internal set
			{
				node = value;
			}
		}

		public GraphLine Line
		{
			get
			{
				return line;
			}
			internal set
			{
				line = value;
			}
		}
	}
}

using System;

namespace MapMagic.Terrains
{
	[Serializable]
	public class EdgesSet
	{
		public bool ready;

		public Edges heightEdges = new Edges(0, 0);

		public Edges[] splatEdges;

		public Edges[] controlEdges;

		public Lowered lowered;
	}
}

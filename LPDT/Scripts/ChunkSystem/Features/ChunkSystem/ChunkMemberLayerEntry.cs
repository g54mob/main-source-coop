using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.ChunkSystem
{
	[Serializable]
	public class ChunkMemberLayerEntry
	{
		[field: SerializeField]
		public string Layer { get; private set; }

		[field: SerializeField]
		[field: SerializeReference]
		public List<ChunkStatusUpdateBehaviour> Activators { get; private set; }
	}
}

using System;
using UnityEngine;

namespace Features.ChunkSystem
{
	[Serializable]
	public abstract class ChunkStatusUpdateBehaviour
	{
		public abstract void Batch(GameObject[] objects);

		public abstract void Enable();

		public abstract void Disable();
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.ChunkSystem
{
	[Serializable]
	public struct ChunkLayerData
	{
		public Vector3Int CurrentPosition;

		public ChunkStatus CurrentStatus;

		public List<ChunkStatusUpdateBehaviour> Activators;

		public ChunkLayerData(Vector3Int currentPosition, ChunkStatus currentStatus, List<ChunkStatusUpdateBehaviour> activators)
		{
			CurrentPosition = currentPosition;
			CurrentStatus = currentStatus;
			Activators = activators;
		}
	}
}

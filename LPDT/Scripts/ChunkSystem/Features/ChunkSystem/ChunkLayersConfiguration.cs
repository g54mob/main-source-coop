using UnityEngine;

namespace Features.ChunkSystem
{
	[CreateAssetMenu(fileName = "ChunkLayersConfiguration_Default", menuName = "Configurations/ChunkSystem/ChunkLayersConfiguration")]
	public class ChunkLayersConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public ChunkLayerEntry[] Layers { get; private set; }
	}
}

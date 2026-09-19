using Fusion;
using UnityEngine;

namespace Features.SynchronizedModelsModule.Scripts
{
	[CreateAssetMenu(fileName = "ModelsSynchronizerConfiguration_Default", menuName = "Configurations/ModelsSynchronizer/ModelsSynchronizerConfiguration")]
	public class ModelsSynchronizerConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public NetworkObject JsonModelsSynchronizer { get; private set; }
	}
}

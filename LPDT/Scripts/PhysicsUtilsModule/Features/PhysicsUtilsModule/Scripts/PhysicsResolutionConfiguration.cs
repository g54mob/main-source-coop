using Global.SerializableDictionary;
using UnityEngine;

namespace Features.PhysicsUtilsModule.Scripts
{
	[CreateAssetMenu(fileName = "PhysicsResolutionConfiguration_Default", menuName = "Configurations/PhysicsUtilsModule/PhysicsResolutionConfiguration")]
	public class PhysicsResolutionConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<PhysicsResolutionTier, PhysicsResolutionSettings> PhysicsResolutionSettings { get; private set; }
	}
}

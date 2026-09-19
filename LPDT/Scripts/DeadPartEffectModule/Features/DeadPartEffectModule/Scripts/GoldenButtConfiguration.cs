using System.Collections.Generic;
using Features.LevelObjectSpawnModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.DeadPartEffectModule.Scripts
{
	[CreateAssetMenu(fileName = "GoldenButtConfiguration_Default", menuName = "Configurations/DeadPart/GoldenButtConfiguration")]
	public class GoldenButtConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<int, List<LevelObjectData>> GoldenButtObjects { get; private set; }
	}
}

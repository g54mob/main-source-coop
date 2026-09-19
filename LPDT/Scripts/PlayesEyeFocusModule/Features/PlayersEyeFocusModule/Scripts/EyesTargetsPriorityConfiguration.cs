using Global.SerializableDictionary;
using UnityEngine;

namespace Features.PlayersEyeFocusModule.Scripts
{
	[CreateAssetMenu(fileName = "EyesTargetsPriorityConfiguration_Default", menuName = "Configurations/Player/EyesTargetsPriorityConfiguration")]
	public class EyesTargetsPriorityConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<EyesTarget, int> EyesTargetsPriority { get; private set; }
	}
}

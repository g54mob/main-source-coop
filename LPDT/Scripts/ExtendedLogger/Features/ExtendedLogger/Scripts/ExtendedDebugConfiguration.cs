using Global.SerializableDictionary;
using UnityEngine;

namespace Features.ExtendedLogger.Scripts
{
	[CreateAssetMenu(fileName = "ExtendedDebugConfiguration", menuName = "Configurations/ExtendedLogger/ExtendedDebugConfiguration")]
	public class ExtendedDebugConfiguration : ScriptableObject
	{
		public DebugFilterType DebugFilterType;

		public SerializableDictionary<DebugFilterType, Color> DebugFilterColors;
	}
}

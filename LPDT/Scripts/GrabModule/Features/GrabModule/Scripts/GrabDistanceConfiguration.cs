using Global.SerializableDictionary;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	[CreateAssetMenu(fileName = "GrabDistanceConfiguration_Default", menuName = "Configurations/GrabModule/GrabDistanceConfiguration")]
	public class GrabDistanceConfiguration : ScriptableObject
	{
		public SerializableDictionary<GrabDistanceType, GrabDistanceData> GrabDistanceData;
	}
}

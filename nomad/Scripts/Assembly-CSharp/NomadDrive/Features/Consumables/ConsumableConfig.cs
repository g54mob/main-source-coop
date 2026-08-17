using EvilCore;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.Consumables
{
	[CreateAssetMenu(menuName = "NomadDrive/Consumables/Consumable", fileName = "ConsumableConfig")]
	public class ConsumableConfig : ScriptableObject
	{
		public SerializableDictionary<PlayerStatType, float> playerStatCollection = new SerializableDictionary<PlayerStatType, float>();
	}
}

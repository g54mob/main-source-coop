using Features.WorldTokenModule.Scripts.Views;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.WorldTokenModule.Scripts
{
	[CreateAssetMenu(fileName = "WorldTokenConfiguration_Default", menuName = "Configurations/WorldTokenModule/WorldTokenConfiguration")]
	public class WorldTokenConfiguration : ScriptableObject
	{
		public SerializableDictionary<WorldTokenType, WorldTokenViewBase> WorldTokenByType = new SerializableDictionary<WorldTokenType, WorldTokenViewBase>();

		public SerializableDictionary<WorldTokenType, BigButtWorldTokenViewBase> BigButtWorldTokenByType = new SerializableDictionary<WorldTokenType, BigButtWorldTokenViewBase>();
	}
}

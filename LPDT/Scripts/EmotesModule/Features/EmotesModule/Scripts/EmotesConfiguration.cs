using Fusion;
using UnityEngine;

namespace Features.EmotesModule.Scripts
{
	[CreateAssetMenu(fileName = "EmotesConfiguration_Default", menuName = "Configurations/EmotesModule/EmotesConfiguration")]
	public class EmotesConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<BodyEmoteType, string> EmotesPool { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<HandEmoteType, string> HandEmotesPool { get; private set; }
	}
}

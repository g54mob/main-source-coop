using Global.SerializableDictionary;
using UnityEngine;

namespace Features.EmotesModule.Scripts
{
	[CreateAssetMenu(fileName = "EmotesMapConfig_Default", menuName = "Configurations/EmotesModule/EmotesMapConfig")]
	public class EmotesMapConfig : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<int, EmotesViewData<HandEmoteType>> HandEmotesMap { get; private set; } = new SerializableDictionary<int, EmotesViewData<HandEmoteType>>();

		[field: SerializeField]
		public SerializableDictionary<int, EmotesViewData<FaceEmoteType>> FaceEmotesMap { get; private set; } = new SerializableDictionary<int, EmotesViewData<FaceEmoteType>>();

		[field: SerializeField]
		public SerializableDictionary<int, EmotesViewData<BodyEmoteType>> BodyEmotesMap { get; private set; } = new SerializableDictionary<int, EmotesViewData<BodyEmoteType>>();

		[field: SerializeField]
		public SerializableDictionary<EmoteGroup, HotbarViewData> HotbarEmotesMap { get; private set; } = new SerializableDictionary<EmoteGroup, HotbarViewData>();
	}
}

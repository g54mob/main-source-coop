using System;

namespace Features.EmotesModule.Scripts
{
	public class EmoteByGroupTriggerService : IEmoteByGroupTriggerService
	{
		private readonly IFaceEmotesTriggerService _faceEmotesTriggerService;

		private readonly IHandEmotesTriggerService _handEmotesTriggerService;

		private readonly EmotesMapConfig _emotesMapConfig;

		private readonly IBodyEmotesTriggerService _bodyEmotesTriggerService;

		public EmoteByGroupTriggerService(IFaceEmotesTriggerService faceEmotesTriggerService, IHandEmotesTriggerService handEmotesTriggerService, EmotesMapConfig emotesMapConfig, IBodyEmotesTriggerService bodyEmotesTriggerService)
		{
			_faceEmotesTriggerService = faceEmotesTriggerService;
			_handEmotesTriggerService = handEmotesTriggerService;
			_emotesMapConfig = emotesMapConfig;
			_bodyEmotesTriggerService = bodyEmotesTriggerService;
		}

		public void TriggerEmoteByGroup(EmoteGroup emoteGroup, int emoteNumber)
		{
			switch (emoteGroup)
			{
			case EmoteGroup.Body:
				_bodyEmotesTriggerService.TriggerEmote(GetBodyEmotesByID(emoteNumber));
				break;
			case EmoteGroup.Face:
				_faceEmotesTriggerService.TriggerEmote(GetFaceEmotesByID(emoteNumber), 100f);
				break;
			case EmoteGroup.Hand:
				_handEmotesTriggerService.TriggerHandEmote(GetHandEmotesByID(emoteNumber));
				break;
			default:
				throw new ArgumentOutOfRangeException("emoteGroup", emoteGroup, null);
			}
		}

		private HandEmoteType GetHandEmotesByID(int id)
		{
			if (!_emotesMapConfig.HandEmotesMap.TryGetValue(id, out var value))
			{
				return HandEmoteType.None;
			}
			return value.Type;
		}

		private FaceEmoteType GetFaceEmotesByID(int id)
		{
			if (!_emotesMapConfig.FaceEmotesMap.TryGetValue(id, out var value))
			{
				return FaceEmoteType.None;
			}
			return value.Type;
		}

		private BodyEmoteType GetBodyEmotesByID(int id)
		{
			if (!_emotesMapConfig.BodyEmotesMap.TryGetValue(id, out var value))
			{
				return BodyEmoteType.None;
			}
			return value.Type;
		}
	}
}

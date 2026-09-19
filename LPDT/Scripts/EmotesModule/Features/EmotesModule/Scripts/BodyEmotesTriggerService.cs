using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.EmotesModule.Scripts
{
	public class BodyEmotesTriggerService : IBodyEmotesTriggerService
	{
		private readonly EmotesTriggerModel _emotesTriggerModel;

		public BodyEmotesTriggerService(EmotesTriggerModel emotesTriggerModel)
		{
			_emotesTriggerModel = emotesTriggerModel;
		}

		public void TriggerEmote(BodyEmoteType emoteType)
		{
			if (emoteType != BodyEmoteType.None)
			{
				_emotesTriggerModel.TriggerBodyEmote(emoteType);
			}
		}

		public void TriggerRandomEmote()
		{
			List<BodyEmoteType> list = new List<BodyEmoteType>();
			foreach (BodyEmoteType value in Enum.GetValues(typeof(BodyEmoteType)))
			{
				if (value != BodyEmoteType.None)
				{
					list.Add(value);
				}
			}
			_emotesTriggerModel.TriggerBodyEmote(list[UnityEngine.Random.Range(0, list.Count)]);
		}
	}
}

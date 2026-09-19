using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.EmotesModule.Scripts
{
	public class HandEmotesTriggerService : IHandEmotesTriggerService
	{
		private readonly EmotesTriggerModel _emotesTriggerModel;

		public HandEmotesTriggerService(EmotesTriggerModel emotesTriggerModel)
		{
			_emotesTriggerModel = emotesTriggerModel;
		}

		public void TriggerHandEmote(HandEmoteType handEmote)
		{
			_emotesTriggerModel.TriggerHandEmote(handEmote);
		}

		public void TriggerRandomHandEmote()
		{
			List<HandEmoteType> list = new List<HandEmoteType>();
			foreach (HandEmoteType value in Enum.GetValues(typeof(HandEmoteType)))
			{
				if (value != HandEmoteType.None)
				{
					list.Add(value);
				}
			}
			if (list.Count != 0)
			{
				_emotesTriggerModel.TriggerHandEmote(list[UnityEngine.Random.Range(0, list.Count)]);
			}
		}
	}
}

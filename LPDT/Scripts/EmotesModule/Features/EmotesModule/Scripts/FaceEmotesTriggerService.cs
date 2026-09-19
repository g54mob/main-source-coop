using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.EmotesModule.Scripts
{
	public class FaceEmotesTriggerService : IFaceEmotesTriggerService
	{
		private readonly EmotesTriggerModel _emotesTriggerModel;

		public FaceEmotesTriggerService(EmotesTriggerModel emotesTriggerModel)
		{
			_emotesTriggerModel = emotesTriggerModel;
		}

		public void TriggerEmote(FaceEmoteType emoteType, float percent)
		{
			if (emoteType != FaceEmoteType.None)
			{
				_emotesTriggerModel.TriggerFaceEmote(emoteType, percent);
			}
		}

		public void TriggerRandomEmote(float percent)
		{
			List<FaceEmoteType> list = new List<FaceEmoteType>();
			foreach (FaceEmoteType value in Enum.GetValues(typeof(FaceEmoteType)))
			{
				if (value != FaceEmoteType.None)
				{
					list.Add(value);
				}
			}
			_emotesTriggerModel.TriggerFaceEmote(list[UnityEngine.Random.Range(0, list.Count)], percent);
		}
	}
}

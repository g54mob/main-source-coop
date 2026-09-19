using System;
using Global.SerializableDictionary;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class StoreArcData
	{
		public ConditionalCardType ConditionalCardType;

		public int TotalCards;

		public int MaxConditionalCards;

		public SerializableDictionary<CardItemType, int> MinimumPerType;
	}
}

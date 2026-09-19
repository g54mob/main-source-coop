using System;
using UnityEngine;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	[Serializable]
	public class BlackjackCardMaterialEntry
	{
		[field: SerializeField]
		public BlackjackCardId CardId { get; private set; }

		[field: SerializeField]
		public Material Material { get; private set; }
	}
}

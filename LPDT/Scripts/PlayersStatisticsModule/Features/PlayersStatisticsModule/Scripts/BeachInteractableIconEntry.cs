using System;
using Features.BeachInteractableCommonModule.Scripts;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts
{
	[Serializable]
	public class BeachInteractableIconEntry
	{
		[field: SerializeField]
		public BeachInteractableType BeachInteractableType { get; private set; }

		[field: SerializeField]
		public Sprite Icon { get; private set; }
	}
}

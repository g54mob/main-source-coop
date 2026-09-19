using System;
using UnityEngine;

namespace Features.TipsModule.Scripts.Display
{
	[Serializable]
	public class TipConfigEntry
	{
		[SerializeReference]
		public TipDisplayDataBase Display;
	}
}

using System;
using UnityEngine;

namespace Features.RumModule.Scripts
{
	[Serializable]
	public class RumIconEntry
	{
		[field: SerializeField]
		public RumType RumType { get; private set; }

		[field: SerializeField]
		public Sprite Icon { get; private set; }
	}
}

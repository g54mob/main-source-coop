using System;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Credits
{
	[Serializable]
	public class CreditsEntryData
	{
		[field: SerializeField]
		public string Profession { get; private set; }

		[field: SerializeField]
		public string Name { get; private set; }
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Credits
{
	[Serializable]
	public class CreditsSection
	{
		[field: SerializeField]
		public string Title { get; private set; }

		[field: SerializeField]
		public List<CreditsEntryData> Entries { get; private set; } = new List<CreditsEntryData>();
	}
}

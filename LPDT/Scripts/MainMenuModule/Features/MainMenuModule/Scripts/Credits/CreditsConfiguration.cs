using System.Collections.Generic;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Credits
{
	[CreateAssetMenu(fileName = "CreditsConfiguration_Default", menuName = "Configurations/Credits/CreditsConfiguration")]
	public class CreditsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<CreditsSection> Sections { get; private set; } = new List<CreditsSection>();
	}
}

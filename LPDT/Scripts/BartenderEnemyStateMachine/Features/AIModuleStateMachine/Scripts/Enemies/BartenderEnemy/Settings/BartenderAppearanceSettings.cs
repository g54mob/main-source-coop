using System.Collections.Generic;
using Features.SkinConfiguration.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Settings
{
	[CreateAssetMenu(fileName = "BartenderAppearanceSettings_Default", menuName = "RubberArms/Bartender/Bartender Appearance Settings")]
	public class BartenderAppearanceSettings : ScriptableObject
	{
		[field: SerializeField]
		public List<SkinType> PossibleSkins { get; private set; } = new List<SkinType> { SkinType.Skins10 };

		[field: Header("Dance")]
		[field: Tooltip("Animator trigger names the bartender may use while dancing (same as player body emotes).")]
		[field: SerializeField]
		public List<string> DanceAnimatorTriggers { get; private set; } = new List<string> { "ShoeDanceEmote", "KneesDanceEmote", "JumpDanceEmote" };
	}
}

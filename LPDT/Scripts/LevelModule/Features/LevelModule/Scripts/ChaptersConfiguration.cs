using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[CreateAssetMenu(fileName = "ChaptersConfiguration_Default", menuName = "Configurations/Levels/ChaptersConfiguration")]
	public class ChaptersConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public bool IsWithChaptersLocking { get; private set; } = true;
	}
}

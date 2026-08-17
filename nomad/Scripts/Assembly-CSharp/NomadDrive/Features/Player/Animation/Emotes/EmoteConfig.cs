using UnityEngine;

namespace NomadDrive.Features.Player.Animation.Emotes
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Emote Config")]
	public class EmoteConfig : ScriptableObject
	{
		[Header("Display")]
		public string emoteName;

		public Sprite icon;

		[Header("Animation")]
		public AnimationClip clip;

		public EmoteBodyMode bodyMode;

		public bool isLooping;

		[Header("Timing")]
		public float blendInDuration = 0.2f;

		public float blendOutDuration = 0.2f;
	}
}

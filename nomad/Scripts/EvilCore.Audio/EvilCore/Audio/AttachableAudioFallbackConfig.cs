using Ami.BroAudio;
using UnityEngine;

namespace EvilCore.Audio
{
	[CreateAssetMenu(fileName = "AttachableAudioFallbackConfig", menuName = "NomadDrive/Attachables/Attach Detach Fallback Sound")]
	public class AttachableAudioFallbackConfig : ScriptableObject
	{
		[Header("Fallback Sounds")]
		[SerializeField]
		private SoundID defaultAttachSound;

		[SerializeField]
		private SoundID defaultDetachSound;

		public SoundID DefaultAttachSound => defaultAttachSound;

		public SoundID DefaultDetachSound => defaultDetachSound;
	}
}

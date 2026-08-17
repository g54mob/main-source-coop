using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Damage Audio Config")]
	public class PlayerDamageAudioConfig : ScriptableObject
	{
		[Header("Damage Sound")]
		[Tooltip("All of these sounds are played simultaneously the moment the player takes any direct damage.")]
		[SerializeField]
		private SoundID[] damageSounds;

		public IReadOnlyList<SoundID> DamageSounds => damageSounds;
	}
}

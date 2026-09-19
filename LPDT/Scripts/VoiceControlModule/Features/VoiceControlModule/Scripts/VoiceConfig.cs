using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.VoiceControlModule.Scripts
{
	[CreateAssetMenu(fileName = "VoiceConfig_Default", menuName = "Configurations/Voice/VoiceConfig")]
	public class VoiceConfig : ScriptableObject
	{
		[field: SerializeField]
		public List<PlayerSpeakerHolder> PlayerSpeakers { get; private set; }

		public GameObject GetPlayerSpeaker(PlayerSpeakerType playerSpeakerType)
		{
			return PlayerSpeakers.FirstOrDefault((PlayerSpeakerHolder s) => s.Type == playerSpeakerType).Speaker;
		}
	}
}

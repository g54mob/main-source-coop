using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Player.Animation.Emotes
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Emote Database")]
	public class EmoteDatabase : ScriptableObject
	{
		[SerializeField]
		private List<EmoteConfig> _emotes = new List<EmoteConfig>();

		public IReadOnlyList<EmoteConfig> Emotes => _emotes;

		public int Count => _emotes.Count;

		public EmoteConfig GetEmote(int index)
		{
			if (index < 0 || index >= _emotes.Count)
			{
				return null;
			}
			return _emotes[index];
		}

		public int GetIndex(EmoteConfig emote)
		{
			return _emotes.IndexOf(emote);
		}
	}
}

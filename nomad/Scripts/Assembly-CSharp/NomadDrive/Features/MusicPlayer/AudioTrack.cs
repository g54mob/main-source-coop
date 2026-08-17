using Ami.BroAudio;
using UnityEngine;

namespace NomadDrive.Features.MusicPlayer
{
	[CreateAssetMenu(menuName = "NomadDrive/Music Player/Audio Track", fileName = "NewTrack")]
	public class AudioTrack : ScriptableObject
	{
		[Header("Track Info")]
		[SerializeField]
		private string _trackName;

		[SerializeField]
		private string _artistName;

		[Header("Audio")]
		[SerializeField]
		private SoundID _soundId;

		[Header("Unlock Settings")]
		[SerializeField]
		private bool _unlockedByDefault;

		[Header("UI (Optional)")]
		[SerializeField]
		private Sprite _albumArt;

		public string TrackName => _trackName;

		public string ArtistName => _artistName;

		public SoundID SoundId => _soundId;

		public bool UnlockedByDefault => _unlockedByDefault;

		public Sprite AlbumArt => _albumArt;

		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrEmpty(_artistName))
				{
					return _trackName + " - " + _artistName;
				}
				return _trackName;
			}
		}
	}
}

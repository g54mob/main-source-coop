using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.MusicPlayer
{
	[CreateAssetMenu(menuName = "NomadDrive/Music Player/Library", fileName = "AudioTrackLibrary")]
	public class AudioTrackLibrary : SerializedScriptableObject
	{
		[SerializeField]
		private List<AudioTrack> _allTracks = new List<AudioTrack>();

		[NonSerialized]
		private HashSet<AudioTrack> _unlockedTracks = new HashSet<AudioTrack>();

		[NonSerialized]
		private List<AudioTrack> _unlockedTracksList = new List<AudioTrack>();

		[NonSerialized]
		private bool _isInitialized;

		public int TotalTrackCount => _allTracks.Count;

		public int UnlockedTrackCount => _unlockedTracksList.Count;

		public bool IsInitialized => _isInitialized;

		public event Action<AudioTrack> OnTrackUnlocked;

		public void Initialize()
		{
			if (_isInitialized)
			{
				return;
			}
			_unlockedTracks = new HashSet<AudioTrack>();
			_unlockedTracksList = new List<AudioTrack>();
			foreach (AudioTrack allTrack in _allTracks)
			{
				if (allTrack != null && allTrack.UnlockedByDefault)
				{
					_unlockedTracks.Add(allTrack);
					_unlockedTracksList.Add(allTrack);
				}
			}
			_isInitialized = true;
		}

		public void Reset()
		{
			_isInitialized = false;
			_unlockedTracks?.Clear();
			_unlockedTracksList?.Clear();
			Initialize();
		}

		public bool IsUnlocked(AudioTrack track)
		{
			EnsureInitialized();
			if (track != null)
			{
				return _unlockedTracks.Contains(track);
			}
			return false;
		}

		public bool Unlock(AudioTrack track)
		{
			EnsureInitialized();
			if (track == null)
			{
				return false;
			}
			if (!_allTracks.Contains(track))
			{
				return false;
			}
			if (_unlockedTracks.Contains(track))
			{
				return false;
			}
			_unlockedTracks.Add(track);
			_unlockedTracksList.Add(track);
			this.OnTrackUnlocked?.Invoke(track);
			return true;
		}

		public IReadOnlyList<AudioTrack> GetUnlockedTracks()
		{
			EnsureInitialized();
			return _unlockedTracksList.AsReadOnly();
		}

		public IReadOnlyList<AudioTrack> GetAllTracks()
		{
			return _allTracks.AsReadOnly();
		}

		public AudioTrack GetTrackByIndex(int index)
		{
			EnsureInitialized();
			if (_unlockedTracksList.Count == 0)
			{
				return null;
			}
			if (index < 0 || index >= _unlockedTracksList.Count)
			{
				return null;
			}
			return _unlockedTracksList[index];
		}

		public int GetTrackIndex(AudioTrack track)
		{
			EnsureInitialized();
			return _unlockedTracksList.IndexOf(track);
		}

		public AudioTrack GetNextTrack(AudioTrack current)
		{
			EnsureInitialized();
			if (_unlockedTracksList.Count == 0)
			{
				return null;
			}
			if (current == null)
			{
				return _unlockedTracksList[0];
			}
			int num = _unlockedTracksList.IndexOf(current);
			if (num < 0)
			{
				return _unlockedTracksList[0];
			}
			int index = (num + 1) % _unlockedTracksList.Count;
			return _unlockedTracksList[index];
		}

		public AudioTrack GetPreviousTrack(AudioTrack current)
		{
			EnsureInitialized();
			if (_unlockedTracksList.Count == 0)
			{
				return null;
			}
			if (current == null)
			{
				List<AudioTrack> unlockedTracksList = _unlockedTracksList;
				return unlockedTracksList[unlockedTracksList.Count - 1];
			}
			int num = _unlockedTracksList.IndexOf(current);
			if (num < 0)
			{
				List<AudioTrack> unlockedTracksList2 = _unlockedTracksList;
				return unlockedTracksList2[unlockedTracksList2.Count - 1];
			}
			int index = (num - 1 + _unlockedTracksList.Count) % _unlockedTracksList.Count;
			return _unlockedTracksList[index];
		}

		public bool HasUnlockedTracks()
		{
			EnsureInitialized();
			return _unlockedTracksList.Count > 0;
		}

		private void EnsureInitialized()
		{
			if (!_isInitialized)
			{
				Initialize();
			}
		}

		private void OnEnable()
		{
			_isInitialized = false;
			_unlockedTracks = new HashSet<AudioTrack>();
			_unlockedTracksList = new List<AudioTrack>();
		}

		private void DebugListTracks()
		{
			EnsureInitialized();
			foreach (AudioTrack allTrack in _allTracks)
			{
				IsUnlocked(allTrack);
			}
		}
	}
}

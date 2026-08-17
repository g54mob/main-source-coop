using System;
using System.Collections.Generic;

namespace EvilCore.Recording
{
	public class DirectorSequencer
	{
		private readonly List<DirectorSequenceEntry> _entries = new List<DirectorSequenceEntry>();

		private int _currentIndex;

		private float _elapsed;

		private bool _isPlaying;

		private bool _isPaused;

		private Action<int, TransitionType, float> _onCameraSwitch;

		private Action _onSequenceComplete;

		public bool IsPlaying => _isPlaying;

		public bool IsPaused => _isPaused;

		public int CurrentIndex => _currentIndex;

		public int EntryCount => _entries.Count;

		public void BuildFromCameraList(IReadOnlyList<DirectorCameraSetup> cameras)
		{
			_entries.Clear();
			for (int i = 0; i < cameras.Count; i++)
			{
				_entries.Add(new DirectorSequenceEntry
				{
					cameraIndex = i,
					holdDuration = cameras[i].HoldDuration,
					transitionType = cameras[i].TransitionType,
					blendDuration = cameras[i].BlendDuration
				});
			}
		}

		public void Play(Action<int, TransitionType, float> onCameraSwitch, Action onComplete)
		{
			if (_entries.Count != 0)
			{
				_onCameraSwitch = onCameraSwitch;
				_onSequenceComplete = onComplete;
				_currentIndex = 0;
				_elapsed = 0f;
				_isPlaying = true;
				_isPaused = false;
				DirectorSequenceEntry directorSequenceEntry = _entries[0];
				_onCameraSwitch?.Invoke(directorSequenceEntry.cameraIndex, directorSequenceEntry.transitionType, directorSequenceEntry.blendDuration);
			}
		}

		public void Pause()
		{
			if (_isPlaying)
			{
				_isPaused = true;
			}
		}

		public void Resume()
		{
			if (_isPlaying)
			{
				_isPaused = false;
			}
		}

		public void Stop()
		{
			_isPlaying = false;
			_isPaused = false;
			_currentIndex = 0;
			_elapsed = 0f;
		}

		public void Update(float deltaTime)
		{
			if (!_isPlaying || _isPaused || _entries.Count == 0)
			{
				return;
			}
			_elapsed += deltaTime;
			DirectorSequenceEntry directorSequenceEntry = _entries[_currentIndex];
			if (_elapsed >= directorSequenceEntry.holdDuration)
			{
				_currentIndex++;
				_elapsed = 0f;
				if (_currentIndex >= _entries.Count)
				{
					_isPlaying = false;
					_onSequenceComplete?.Invoke();
				}
				else
				{
					DirectorSequenceEntry directorSequenceEntry2 = _entries[_currentIndex];
					_onCameraSwitch?.Invoke(directorSequenceEntry2.cameraIndex, directorSequenceEntry2.transitionType, directorSequenceEntry2.blendDuration);
				}
			}
		}

		public float GetTotalDuration()
		{
			float num = 0f;
			foreach (DirectorSequenceEntry entry in _entries)
			{
				num += entry.holdDuration;
			}
			return num;
		}

		public float GetElapsedTotal()
		{
			float num = 0f;
			for (int i = 0; i < _currentIndex && i < _entries.Count; i++)
			{
				num += _entries[i].holdDuration;
			}
			return num + _elapsed;
		}

		public float GetProgress()
		{
			float totalDuration = GetTotalDuration();
			if (!(totalDuration > 0f))
			{
				return 0f;
			}
			return GetElapsedTotal() / totalDuration;
		}
	}
}

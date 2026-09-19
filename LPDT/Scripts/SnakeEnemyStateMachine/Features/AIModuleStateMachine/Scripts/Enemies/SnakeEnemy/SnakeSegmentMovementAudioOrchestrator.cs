using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	public class SnakeSegmentMovementAudioOrchestrator : MonoBehaviour
	{
		[SerializeField]
		private SnakeSegmentMovementAudio[] _players;

		[SerializeField]
		private float _minInterval = 0.15f;

		[SerializeField]
		private float _maxInterval = 0.7f;

		[SerializeField]
		private int _minPlayCount = 1;

		[SerializeField]
		private int _maxPlayCount = 4;

		[SerializeField]
		private int _maxConcurrent = 16;

		[SerializeField]
		[Range(0f, 1f)]
		private float _skipChance = 0.1f;

		private int[] _freeIndices;

		private float _nextPlayTime;

		private void Awake()
		{
			if (_players == null || _players.Length == 0)
			{
				_players = GetComponentsInChildren<SnakeSegmentMovementAudio>(includeInactive: true);
			}
			_freeIndices = ((_players.Length != 0) ? new int[_players.Length] : new int[0]);
			ScheduleNext();
		}

		private void Update()
		{
			if (_players != null && _players.Length != 0 && !(Time.time < _nextPlayTime))
			{
				ScheduleNext();
				if (!(Random.value < _skipChance))
				{
					TryPlayBurst();
				}
			}
		}

		private void TryPlayBurst()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < _players.Length; i++)
			{
				SnakeSegmentMovementAudio snakeSegmentMovementAudio = _players[i];
				if (!(snakeSegmentMovementAudio == null))
				{
					if (snakeSegmentMovementAudio.IsPlaying)
					{
						num++;
						continue;
					}
					_freeIndices[num2] = i;
					num2++;
				}
			}
			int num3 = _maxConcurrent - num;
			if (num3 > 0 && num2 != 0)
			{
				int num4 = Mathf.Min(_maxPlayCount, num3, num2);
				int num5 = Random.Range(Mathf.Min(_minPlayCount, num4), num4 + 1);
				for (int j = 0; j < num5; j++)
				{
					int num6 = Random.Range(j, num2);
					int num7 = _freeIndices[j];
					_freeIndices[j] = _freeIndices[num6];
					_freeIndices[num6] = num7;
					_players[_freeIndices[j]].Play();
				}
			}
		}

		private void ScheduleNext()
		{
			_nextPlayTime = Time.time + Random.Range(_minInterval, _maxInterval);
		}

		private void OnValidate()
		{
			if (_minInterval < 0f)
			{
				_minInterval = 0f;
			}
			if (_maxInterval < _minInterval)
			{
				_maxInterval = _minInterval;
			}
			if (_minPlayCount < 1)
			{
				_minPlayCount = 1;
			}
			if (_maxPlayCount < _minPlayCount)
			{
				_maxPlayCount = _minPlayCount;
			}
			if (_maxConcurrent < _maxPlayCount)
			{
				_maxConcurrent = _maxPlayCount;
			}
			if (_players == null || _players.Length == 0)
			{
				_players = GetComponentsInChildren<SnakeSegmentMovementAudio>(includeInactive: true);
			}
		}
	}
}

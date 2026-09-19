using System;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using UnityEngine;

namespace Features.RunningSessionModule.Scripts
{
	[Serializable]
	public class RunningSessionModel : JsonSynchronizableBase<RunningSessionModel>
	{
		[SerializeField]
		private int _currentGameIndex;

		private string _sessionCode;

		public string SessionCode
		{
			get
			{
				return _sessionCode;
			}
			set
			{
				if (_sessionCode != value)
				{
					_sessionCode = value;
					this.OnRunningSessionCodeChanged?.Invoke(_sessionCode);
				}
			}
		}

		public int CurrentGameIndex
		{
			get
			{
				return _currentGameIndex;
			}
			set
			{
				_currentGameIndex = value;
				this.OnCurrentGameIndexChanged?.Invoke(_currentGameIndex);
			}
		}

		public override RPCType RPCType => RPCType.FromStateAuthorityToAll;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<string> OnRunningSessionCodeChanged;

		public event Action<int> OnCurrentGameIndexChanged;

		protected override void SetNewValues(RunningSessionModel synchronizable, bool isSynchronizedOnStart)
		{
			CurrentGameIndex = synchronizable.CurrentGameIndex;
		}
	}
}

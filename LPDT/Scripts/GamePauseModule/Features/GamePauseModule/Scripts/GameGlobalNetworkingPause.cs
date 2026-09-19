using System;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;

namespace Features.GamePauseModule.Scripts
{
	[Serializable]
	public class GameGlobalNetworkingPause : JsonSynchronizableBase<GameGlobalNetworkingPause>
	{
		public bool IsGlobalPausedEnable;

		public bool IsLocalPausedEnable;

		public float NetworkTimeScale = 1f;

		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<bool> OnGlobalPausedChanged;

		public event Action<bool> OnLocalPausedChanged;

		public void SetGlobalPaused(bool isPaused)
		{
			IsGlobalPausedEnable = isPaused;
			Synchronize();
		}

		public void SetLocalPaused(bool isPaused)
		{
			IsLocalPausedEnable = isPaused;
			this.OnLocalPausedChanged?.Invoke(IsLocalPausedEnable);
		}

		public bool AnyPauseEnabled()
		{
			if (!IsGlobalPausedEnable)
			{
				return IsLocalPausedEnable;
			}
			return true;
		}

		protected override void SetNewValues(GameGlobalNetworkingPause synchronizable, bool value)
		{
			IsGlobalPausedEnable = synchronizable.IsGlobalPausedEnable;
			NetworkTimeScale = (IsGlobalPausedEnable ? 0f : 1f);
			this.OnGlobalPausedChanged?.Invoke(IsGlobalPausedEnable);
		}
	}
}

using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class LiftCallLever : MonoBehaviour
	{
		[SerializeField]
		private TwoStateLever _lever;

		private bool _isBound;

		private int _takenPullCount;

		public TwoStateLever Lever => _lever;

		private bool IsReady
		{
			get
			{
				if (_lever != null && _lever.Object != null)
				{
					return _lever.Object.IsValid;
				}
				return false;
			}
		}

		public void Bind()
		{
			if (!_isBound && !(_lever == null))
			{
				_isBound = true;
				_lever.MakeMomentary();
				_takenPullCount = (IsReady ? _lever.PullCount : 0);
			}
		}

		public bool ConsumeCall()
		{
			if (!IsReady)
			{
				return false;
			}
			int pullCount = _lever.PullCount;
			if (pullCount == _takenPullCount)
			{
				return false;
			}
			_takenPullCount = pullCount;
			return true;
		}

		public void DiscardPendingCall()
		{
			if (IsReady)
			{
				_takenPullCount = _lever.PullCount;
			}
		}

		public void SetPullBlocked(bool blocked)
		{
			if (IsReady)
			{
				_lever.SetPullBlocked(blocked);
			}
		}
	}
}

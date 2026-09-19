using System;
using UnityEngine;

namespace Features.BasketballHoopModule.Scripts
{
	public sealed class BasketballHoopTriggerZone : MonoBehaviour
	{
		[SerializeField]
		private BasketballHoopTriggerBand _band;

		[SerializeField]
		private Collider _triggerCollider;

		public event Action<BasketballHoopTriggerBand, Collider> BallEntered;

		public event Action<BasketballHoopTriggerBand, Collider> BallExited;

		private void OnTriggerEnter(Collider other)
		{
			if (PassesBallFilter(other))
			{
				this.BallEntered?.Invoke(_band, other);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (PassesBallFilter(other))
			{
				this.BallExited?.Invoke(_band, other);
			}
		}

		private bool PassesBallFilter(Collider other)
		{
			BasketballBallTeleport component;
			return other.TryGetComponent<BasketballBallTeleport>(out component);
		}

		public void SetColliderEnabled(bool enable)
		{
			_triggerCollider.enabled = enable;
		}
	}
}

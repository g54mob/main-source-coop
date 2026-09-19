using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class CrankLink : MonoBehaviour
	{
		[SerializeField]
		private CrankRotator _firstCrank;

		[SerializeField]
		private CrankRotator _secondCrank;

		[Tooltip("Fastest the driven side may chase the driving side (deg/s).")]
		[SerializeField]
		private float _maxFollowDegreesPerSecond = 720f;

		private float _angleOffset;

		private bool _offsetCaptured;

		private int _lastDriver;

		public void Configure(CrankRotator firstCrank, CrankRotator secondCrank)
		{
			_firstCrank = firstCrank;
			_secondCrank = secondCrank;
			_offsetCaptured = false;
		}

		private void FixedUpdate()
		{
			if (_firstCrank == null || _secondCrank == null || _firstCrank.Object == null || !_firstCrank.Object.IsValid || _secondCrank.Object == null || !_secondCrank.Object.IsValid)
			{
				return;
			}
			if (!_offsetCaptured)
			{
				_angleOffset = _secondCrank.Angle - _firstCrank.Angle;
				_offsetCaptured = true;
				return;
			}
			bool isGrabbed = _firstCrank.IsGrabbed;
			bool isGrabbed2 = _secondCrank.IsGrabbed;
			if (isGrabbed && !isGrabbed2)
			{
				_lastDriver = 1;
			}
			else if (isGrabbed2 && !isGrabbed)
			{
				_lastDriver = 2;
			}
			if (_lastDriver == 1)
			{
				_secondCrank.DriveTowardAngle(_firstCrank.Angle + _angleOffset, _maxFollowDegreesPerSecond);
			}
			else if (_lastDriver == 2)
			{
				_firstCrank.DriveTowardAngle(_secondCrank.Angle - _angleOffset, _maxFollowDegreesPerSecond);
			}
		}
	}
}

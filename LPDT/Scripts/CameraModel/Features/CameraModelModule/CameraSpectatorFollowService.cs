using System;
using UnityEngine;

namespace Features.CameraModelModule
{
	public class CameraSpectatorFollowService : ICameraSpectatorFollowService, ICameraLateResolveStep, IDisposable
	{
		private const string ANCHOR_NAME = "CameraSpectatorFollowAnchor";

		private const float SMOOTH_TIME = 0.05f;

		private const float SNAP_DISTANCE = 3f;

		private readonly CameraModel _cameraModel;

		private Transform _subject;

		private Transform _anchor;

		private Vector3 _followVelocity;

		public bool IsActive => _subject != null;

		public CameraSpectatorFollowService(CameraModel cameraModel)
		{
			_cameraModel = cameraModel;
		}

		public Transform Begin(Transform subject)
		{
			if (subject == null)
			{
				End();
				return null;
			}
			EnsureAnchor();
			_subject = subject;
			_followVelocity = Vector3.zero;
			_anchor.SetPositionAndRotation(subject.position, subject.rotation);
			SetFollowProfileActive(isActive: true);
			return _anchor;
		}

		public void End()
		{
			_subject = null;
			SetFollowProfileActive(isActive: false);
		}

		public void ResolveLate()
		{
			if (!(_subject == null) && !(_anchor == null))
			{
				Vector3 position = _subject.position;
				if ((position - _anchor.position).sqrMagnitude > 9f)
				{
					_followVelocity = Vector3.zero;
					_anchor.SetPositionAndRotation(position, _subject.rotation);
				}
				else
				{
					Vector3 position2 = Vector3.SmoothDamp(_anchor.position, position, ref _followVelocity, 0.05f, float.PositiveInfinity, Time.deltaTime);
					_anchor.SetPositionAndRotation(position2, _subject.rotation);
				}
			}
		}

		public void Dispose()
		{
			_subject = null;
			SetFollowProfileActive(isActive: false);
			if (_anchor != null)
			{
				UnityEngine.Object.Destroy(_anchor.gameObject);
			}
			_anchor = null;
		}

		private void SetFollowProfileActive(bool isActive)
		{
			if (_cameraModel.Cameras.TryGetValue(CameraType.TPCamera, out var value) && !(value == null))
			{
				value.SetSpectatorFollowProfileActive(isActive);
			}
		}

		private void EnsureAnchor()
		{
			if (_anchor == null)
			{
				_anchor = new GameObject("CameraSpectatorFollowAnchor").transform;
			}
		}
	}
}
